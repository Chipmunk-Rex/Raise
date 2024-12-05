using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

public enum AuthenticationState
{
    None,
    Authenticating,
    Authenticated,
    Failed
}

public abstract class Authentication : MonoSingleton<Authentication>
{
    [field: SerializeField] public AuthenticationState State { get; private set; } = AuthenticationState.None;
    [field: SerializeField] public int MaxTryCount { get; private set; } = 6;
    [SerializeField] float authDuration = 3f;
    private async Task<AuthenticationState> DoAuth()
    {
        if (AuthenticationService.Instance.IsAuthorized || State == AuthenticationState.Authenticated)
        {
            State = AuthenticationState.Authenticated;
            return State;
        }
        else
            State = AuthenticationState.Authenticating;

        int tryCount = 0;
        while (State == AuthenticationState.Authenticating || tryCount < MaxTryCount)
        {
            try
            {
                TryAuth();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    State = AuthenticationState.Authenticated;
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                State = AuthenticationState.Failed;
            }

            tryCount++;
            await Task.Delay(Mathf.RoundToInt(authDuration * 1000));
        }
        return State;
    }
    public abstract void TryAuth();
}
