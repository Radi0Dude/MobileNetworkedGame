using System;
using Unity.Services.Core;
using Unity.Services.Friends;
using UnityEngine;

public class UnityServicesInit : MonoBehaviour
{
    LoadCorrectSceneInit loadCorrectSceneInit;

    async void Awake()
    {
        Invoke(nameof(Setup), 0.1f);
    }

    private async void Setup() 
    {
        try
        {
            await UnityServices.InitializeAsync();
            loadCorrectSceneInit = GetComponentInChildren<LoadCorrectSceneInit>();
            loadCorrectSceneInit.LoadCorrectScene();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
