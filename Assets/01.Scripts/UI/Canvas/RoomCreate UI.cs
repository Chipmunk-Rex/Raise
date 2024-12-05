using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCreateUI : MonoBehaviour
{
    [SerializeField] TMP_InputField roomName;
    [SerializeField] Slider maxPlayers;
    [SerializeField] Toggle isPrivate;
    [SerializeField] TMP_InputField password;
    public void Create()
    {
        string roomName = this.roomName.text;
        int maxPlayers = (int)this.maxPlayers.value;
        bool isPrivate = this.isPrivate.isOn;
        string password = this.password.text;
        LobbyManager.Instance.CreateLobby(roomName, maxPlayers, isPrivate, password);
    }
}
