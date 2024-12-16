using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoSingleton<RelayManager>
{
    public async Task<string> StartHost(int maxPlayers)
    {
        Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxPlayers);

        string joinCode = null;
        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return joinCode;
    }

    public async void JoinRelay(string joinCode)
    {
            await Relay.Instance.JoinAllocationAsync(joinCode);
    }
}
