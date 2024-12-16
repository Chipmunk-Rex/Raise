using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class RoomList_RoomUI : MonoBehaviour
{
    [SerializeField] Image borderImage;
    [SerializeField] Color joinedRoomColor;
    [SerializeField] Color maxedRoomColor;
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text ownerName;
    [SerializeField] TMP_Text playerCount;
    public event Action<Lobby> OnJoinLobbyBtnClick;
    public event Action<Lobby> OnOpenLobbyBtnClick;
    private Lobby drawingLobby;
    public void DrawUI(Lobby lobby)
    {
        roomName.text = lobby.Name;
        ownerName.text = GetOwnerName(lobby);
        playerCount.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";

        Debug.Log($"\n {lobby.HostId} \n {AuthenticationService.Instance.PlayerId}");
        if (lobby.HostId == AuthenticationService.Instance.PlayerId)
            borderImage.color = joinedRoomColor;
            
        if (lobby.Players.Count == lobby.MaxPlayers)
            borderImage.color = maxedRoomColor;

        drawingLobby = lobby;
    }
    public string GetOwnerName(Lobby lobby)
    {
        string HostId = lobby.HostId;
        Player host = lobby.Players.Find(player => player.Id == HostId);
        return host.Profile?.Name ?? "Unknown";
    }
    public void Join()
    {
        OnJoinLobbyBtnClick?.Invoke(drawingLobby);
    }
    public void Open()
    {
        OnOpenLobbyBtnClick?.Invoke(drawingLobby);
    }
}
