using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class RoomUI : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text ownerName;
    [SerializeField] TMP_Text playerCount;
    public void DrawUI(Lobby lobby)
    {
        roomName.text = lobby.Name;
        ownerName.text = GetOwnerName(lobby);
        playerCount.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }
    public string GetOwnerName(Lobby lobby)
    {
        return lobby.Players[0].Profile.Name;
    }
}
