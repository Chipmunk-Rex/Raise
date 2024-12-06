using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCreateUI : MonoBehaviour
{
    [SerializeField] TMP_InputField roomName;
    [SerializeField] Slider maxPlayers;
    [SerializeField] TMP_Text maxPlayersText;
    [SerializeField] Toggle isPrivate;
    [SerializeField] TMP_InputField password;
    public void Create()
    {
        string roomName = this.roomName.text;
        int maxPlayers = (int)Mathf.Lerp(1, LobbyManager.Instance.LobbySettingSO.maxPlayers, this.maxPlayers.value);
        Debug.Log(maxPlayers + " max players");
        bool isPrivate = this.isPrivate.isOn;
        string password = this.password.text;
        Debug.Log(password == null);
        LobbyManager.Instance.CreateLobby(roomName, maxPlayers, isPrivate, password);
    }
    public void OnSliderValueChanged()
    {
        maxPlayersText.text = $"{maxPlayers.value}";
    }
}
