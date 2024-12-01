using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LobbySettingSO", menuName = "LobbySettingSO")]
public class LobbySettingSO : ScriptableObject
{
    [SerializeField, Range(1, 30)] public float heartBeatDuration = 10.0f;
    [SerializeField] public uint maxPlayers = 4;
}
