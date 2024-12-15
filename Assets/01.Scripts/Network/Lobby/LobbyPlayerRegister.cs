using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyPlayerRegister : MonoBehaviour
{
    public void RegistePlayer()
    {
        Dictionary<string, PlayerDataObject> data = new(){
            {"PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "unknown")}
        };

        LobbyPlayerRegister le = this;

        Player player = new Player(data: data);
        RegistePlayer(player);
    }

    public void RegistePlayer(Player player)
    {
        LobbyManager.Instance.Player = player;
        Debug.Log("Player Registered");
    }
}
