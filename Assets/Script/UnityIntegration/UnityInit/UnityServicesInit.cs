using System;
using Unity.Services.Core;
using UnityEngine;

public class UnityServicesInit : MonoBehaviour
{
    SignInAnon signInAnon;

    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            signInAnon = GetComponentInChildren<SignInAnon>();
            signInAnon.SignUpAnon();
        }
        catch (Exception e)
        { 
            Debug.LogException(e);
        }
    }
}
