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
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text ownerName;
    [SerializeField] TMP_Text playerCount;
    public event Action OnJoinLobbyBtnClick;
    public event Action OnOpenLobbyBtnClick;
    public void DrawUI(Lobby lobby)
    {
        roomName.text = lobby.Name;
        ownerName.text = GetOwnerName(lobby);
        playerCount.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";

        Debug.Log($"\n {lobby.HostId} \n {AuthenticationService.Instance.PlayerId}");
        if (lobby.HostId == AuthenticationService.Instance.PlayerId)
            borderImage.color = joinedRoomColor;

    }
    public string GetOwnerName(Lobby lobby)
    {
        string HostId = lobby.HostId;
        Player host = lobby.Players.Find(player => player.Id == HostId);
        return host.Profile?.Name ?? "Unknown";
    }
    public void Join()
    {
        OnJoinLobbyBtnClick?.Invoke();
    }
    public void Open()
    {
        OnOpenLobbyBtnClick?.Invoke();
    }
}
