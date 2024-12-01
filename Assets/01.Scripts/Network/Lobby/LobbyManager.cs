using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoSingleton<LobbyManager>
{
    [SerializeField] LobbySettingSO lobbySettingSO;
    // private string lobbyID;
    public string LobbyCode => currentLobby?.LobbyCode;
    public string LobbyID => currentLobby?.Id;
    private Lobby currentLobby;
    public Lobby CurrentLobby => currentLobby;
    public async Task<List<Lobby>> GetLobbiesAsync()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 10,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            List<Lobby> lobbyList = new();
            int Count = 0;
            foreach (Lobby lobby in queryResponse.Results)
            {
                Count += 1;
                if (Count == 100)
                    return null;
                lobbyList.Add(lobby);
            }
            Debug.Log($"LobbyManager : 감지된 로비 개수{lobbyList.Count}");
            return lobbyList;
        }
        catch
        {

        }
        return null;
    }
    public async void CreateLobby(string lobbyName, int maxPlayers)
    {
        maxPlayers = Mathf.Clamp(maxPlayers, 1, (int)lobbySettingSO.maxPlayers);
        currentLobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
        StartLobbyHeartBeat();
    }
    public async void JoinLobbyById(string lobbyID)
    {
        currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyID);
    }
    public async void JoinLobbyByCode(string lobbyCode)
    {
        currentLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
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
        StartCoroutine(LobbyHeartBeat(lobbySettingSO.heartBeatDuration));
    }
    private IEnumerator LobbyHeartBeat(float duration)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(lobbySettingSO.heartBeatDuration);
            if (currentLobby == null)
                break;
            Lobbies.Instance.SendHeartbeatPingAsync(LobbyID);
        }
    }
}
