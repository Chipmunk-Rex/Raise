using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyPlayerRegister : MonoBehaviour
{
    public void RegistePlayer()
    {
        PlayerProfile playerProfile = new PlayerProfile("Unknown");
        Player player = new Player(profile: playerProfile);
        RegistePlayer(player);
    }
    public void RegistePlayer(Player player)
    {
        LobbyManager.Instance.Player = player;
        Debug.Log("Player Registered");
    }
}
