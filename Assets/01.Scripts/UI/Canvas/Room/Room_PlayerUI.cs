using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using TMPro;
using UnityEngine.UI;

public class Room_PlayerUI : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] Image playerProfileImage;
    [SerializeField] Sprite defaultProfileImage;
    public void DrawUI(Lobby lobby, Player player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null");
        }

        playerName.text = player.Data.ContainsKey("PlayerName") ? player.Data["PlayerName"].Value : "Unknown";

        if (player.Data.TryGetValue("profileImage", out PlayerDataObject value))
            playerProfileImage.sprite = GetPlayerProfileImage(value);
        else
            playerProfileImage.sprite = defaultProfileImage;

        if (lobby.HostId == player.Id)
        {
            playerName.text += " (Host)";
        }
    }

    public Sprite GetPlayerProfileImage(PlayerDataObject playerData)
    {
        string profileJson = playerData.Value;
        if (string.IsNullOrEmpty(profileJson))
            return null;
        return null;
    }
}
