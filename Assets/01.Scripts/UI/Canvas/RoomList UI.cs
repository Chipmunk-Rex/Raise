using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class RoomListUI : MonoBehaviour
{
    [SerializeField] private RoomUI roomPrefab;
    [SerializeField] private Transform roomListContent;

    public async void DrawUI()
    {
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }
        List<Lobby> lobbies = await LobbyManager.Instance.GetLobbiesAsync();
        if (lobbies != null)
            foreach (Lobby lobby in lobbies)
            {
                RoomUI roomUI = Instantiate(roomPrefab, roomListContent);
                roomUI.DrawUI(lobby);
            }
    }
}
