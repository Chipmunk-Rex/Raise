using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class RoomListUI : MonoBehaviour
{
    [SerializeField] private RoomList_RoomUI roomPrefab;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private TMP_InputField searchField;
    public async void DrawUI()
    {
        QueryLobbiesOptions queryOption = new QueryLobbiesOptions()
        {
            Count = 10,
            Filters = new List<QueryFilter>()
            {
                new QueryFilter(QueryFilter.FieldOptions.Name,searchField.text, QueryFilter.OpOptions.CONTAINS)
            }
        };

        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }
        List<Lobby> lobbies = await LobbyManager.Instance.GetLobbiesAsync(queryOption);
        if (lobbies != null)
            foreach (Lobby lobby in lobbies)
            {
                RoomList_RoomUI roomUI = Instantiate(roomPrefab, roomListContent);
                roomUI.DrawUI(lobby);
            }
    }
}
