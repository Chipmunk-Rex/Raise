using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class RoomUI : MonoBehaviour
{
    [SerializeField] TMP_Text lobbyName;
    [SerializeField] Room_PlayerUI playerUIPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject readyOrStartBtn;

    private void OnPlayerLeft(List<int> list)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerJoined(List<LobbyPlayerJoined> list)
    {

    }
    private ILobbyEvents lobbyEvents;
    private Lobby drawingLobby;
    public Lobby DrawingLobby
    {
        get => drawingLobby;
        private set
        {
            if (lobbyEvents != null)
            {
                lobbyEvents.UnsubscribeAsync();
            }
            SubscribeLobbyEvents(value.Id);

            drawingLobby = value;
        }
    }

    private async void SubscribeLobbyEvents(string lobbyId)
    {
        LobbyEventCallbacks callback = new LobbyEventCallbacks();
        callback.PlayerJoined += OnPlayerJoined;
        callback.PlayerLeft += OnPlayerLeft;

        ILobbyEvents lobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(lobbyId, callback);
        this.lobbyEvents = lobbyEvents;
    }

    public void DrawUI(Lobby lobby)
    {
        DestroyExistUI();
        CreateUI(lobby);
        DrawElements(lobby);
        DrawingLobby = lobby;

        lobbyName.text = lobby.Name;
    }

    private void DrawElements(Lobby lobby)
    {
        string playerId = AuthenticationService.Instance.PlayerId;
        string hostId = lobby.HostId;
        bool isHost = string.Equals(playerId, hostId, StringComparison.Ordinal);
        Debug.Log(isHost);
        Debug.Log(lobby.HostId);
        Debug.Log(lobby.HostId.Length);
        Debug.Log(AuthenticationService.Instance.PlayerId);
        Debug.Log(AuthenticationService.Instance.PlayerId.Length);
        readyOrStartBtn.SetActive(isHost);
    }

    private void CreateUI(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            Room_PlayerUI playerUI = Instantiate(playerUIPrefab, playerListContent);
            playerUI.DrawUI(lobby, player);
        }
    }

    private void DestroyExistUI()
    {
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
    }
}
