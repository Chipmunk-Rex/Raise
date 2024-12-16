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
    [SerializeField] TMP_Text readyOrStartBtn;

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
        callback.LobbyChanged += OnLobbyChanged;
        callback.PlayerDataChanged += OnPlayerDataChanged;

        ILobbyEvents lobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(lobbyId, callback);
        this.lobbyEvents = lobbyEvents;
    }

    private void OnPlayerDataChanged(Dictionary<int, Dictionary<string, ChangedOrRemovedLobbyValue<PlayerDataObject>>> dictionary)
    {
        DrawUI(DrawingLobby);
    }

    private void OnLobbyChanged(ILobbyChanges changes)
    {
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
        bool isHost = IsHost(lobby);
        readyOrStartBtn.text = isHost ? "Start" : "Ready";
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
    private bool IsHost(Lobby lobby)
    {
        string playerId = AuthenticationService.Instance.PlayerId;
        string hostId = lobby.HostId;
        return string.Equals(playerId, hostId, StringComparison.Ordinal);
    }
    public async void OnReadyOrStartBtnClick()
    {
        if (IsHost(DrawingLobby))
        {
            bool isReady = CheckAllPlayersReady(DrawingLobby.HostId);

            if (isReady)
            {
                await LobbyManager.Instance.StartLobbyAsync(DrawingLobby.Id);
            }
        }
        else
        {
            LobbyManager.Instance.Player.Data["isReady"] = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "true");
        }
    }

    private bool CheckAllPlayersReady(string hostId)
    {
        bool isReady = true;
        foreach (Player player in DrawingLobby.Players)
        {
            if(player.Id == hostId)
            {
                continue;
            }

            if (player.Data.TryGetValue("isReady", out PlayerDataObject isReadyData))
            {
                if (isReadyData.Value != "true")
                {
                    isReady = false;
                    break;
                }
            }
        }

        return isReady;
    }
}
