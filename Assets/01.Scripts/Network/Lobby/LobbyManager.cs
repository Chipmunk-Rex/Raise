using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class LobbyManager : MonoSingleton<LobbyManager>
{
    [field: SerializeField] public LobbySettingSO LobbySettingSO { get; private set; }
    // private string lobbyID;
    public string LobbyCode => currentLobby?.LobbyCode;
    public string LobbyID => currentLobby?.Id;
    private Lobby currentLobby;
    public Lobby CurrentLobby => currentLobby;
    public Player Player { get; set; }
    public async Task<List<Lobby>> GetLobbiesAsync()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 10
            };
            List<Lobby> lobbyList = await GetLobbiesAsync(queryLobbiesOptions);

            return lobbyList;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return null;
    }
    public async Task<List<Lobby>> GetLobbiesAsync(QueryLobbiesOptions queryOption)
    {
        QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryOption);

        List<Lobby> lobbyList = new();
        foreach (Lobby lobby in queryResponse.Results)
        {
            lobbyList.Add(lobby);
        }
        Debug.Log($"LobbyManager : 감지된 로비 개수{lobbyList.Count}");
        return lobbyList;
    }
    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate = false, string password = null)
    {
        Debug.Log("CreateLobby");
        maxPlayers = Mathf.Clamp(maxPlayers, 1, (int)LobbySettingSO.maxPlayers);
        if (password == null || password.Length < 8)
            currentLobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, new CreateLobbyOptions { IsPrivate = isPrivate, Player = this.Player });
        else
            currentLobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, new CreateLobbyOptions { IsPrivate = isPrivate, Player = this.Player, Password = password });

        Debug.Log(currentLobby);
        StartLobbyHeartBeat();
    }
    public async void JoinLobbyById(string lobbyID, string password = null)
    {
        JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
        {
            Player = this.Player
        };
        currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyID, options);
    }
    public async void JoinLobbyByCode(string lobbyCode, string password = null)
    {
        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
        {
            Player = this.Player
        };
        currentLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
    }
    public async void LeaveLobby()
    {
        if (currentLobby == null)
            return;
        await LobbyService.Instance.RemovePlayerAsync(LobbyID, AuthenticationService.Instance.PlayerId);
        currentLobby = null;
    }
    public async void DeleteLobby()
    {
        if (currentLobby == null)
            return;
        await LobbyService.Instance.DeleteLobbyAsync(LobbyID);
        currentLobby = null;
    }
    public void StartLobbyHeartBeat()
    {
        StartCoroutine(LobbyHeartBeat(LobbySettingSO.heartBeatDuration));
    }
    private IEnumerator LobbyHeartBeat(float duration)
    {
        while (true)
        {
            if (currentLobby == null)
                break;
            Lobbies.Instance.SendHeartbeatPingAsync(LobbyID);
            yield return new WaitForSecondsRealtime(LobbySettingSO.heartBeatDuration);
        }
    }
}
