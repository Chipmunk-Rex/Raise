using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

public class UGSInitializer : MonoBehaviour
{
    public UnityEvent OnInitialized;
    async void Awake()
    {
        await UnityServices.InitializeAsync();
        OnInitialized?.Invoke();
    }
}
