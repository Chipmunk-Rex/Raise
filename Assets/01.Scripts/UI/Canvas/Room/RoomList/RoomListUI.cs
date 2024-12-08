using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class RoomListUI : MonoBehaviour
{
    [SerializeField] private RoomList_RoomUI roomPrefab;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private TMP_InputField searchField;
    [SerializeField] public UnityEvent<Lobby> onJoinLobby;
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

                roomUI.OnJoinLobbyBtnClick += OnJoinLobby;
                roomUI.OnOpenLobbyBtnClick += OnOpenLobby;
            }
    }
    private void OnJoinLobby(Lobby lobby)
    {
        Lobby currentJoinedLobby = LobbyManager.Instance.CurrentLobby;
        if (currentJoinedLobby != null)
        {
            if (currentJoinedLobby.Id == lobby.Id)
            {
                onJoinLobby?.Invoke(lobby);
                Debug.Log("Room : You are already in this Room");
                return;
            }
            else
            {
                Debug.Log("Room : are already in another Room");
                return;
            }
        }
        onJoinLobby?.Invoke(lobby);
        LobbyManager.Instance.JoinLobbyById(lobby.Id);
    }

    private void OnOpenLobby(Lobby lobby)
    {
        throw new NotImplementedException();
    }

}
