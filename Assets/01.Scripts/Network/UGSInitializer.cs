using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

public class UGSInitializer : MonoBehaviour
{
    async void Awake()
    {
        await UnityServices.InitializeAsync();
    }
}
