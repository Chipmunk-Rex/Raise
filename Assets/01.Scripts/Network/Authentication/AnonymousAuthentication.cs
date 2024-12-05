using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;

public class AnonymousAuthentication : Authentication
{
    public override void TryAuth()
    {
        AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
