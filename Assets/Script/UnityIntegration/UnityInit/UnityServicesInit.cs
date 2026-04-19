using System;
using Unity.Services.Core;
using Unity.Services.Friends;
using UnityEngine;

public class UnityServicesInit : MonoBehaviour
{
    SignInAnon signInAnon;

    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await FriendsService.Instance.InitializeAsync();
            signInAnon = GetComponentInChildren<SignInAnon>();
            signInAnon.SignUpAnon();
        }
        catch (Exception e)
        { 
            Debug.LogException(e);
        }
    }
}
