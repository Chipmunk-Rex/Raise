using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class RoomUI : MonoBehaviour
{
    [SerializeField] TMP_Text lobbyName;
    [SerializeField] Room_PlayerUI playerUIPrefab;
    [SerializeField] Transform playerListContent;

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
        DrawingLobby = lobby;

        lobbyName.text = lobby.Name;
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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
