using System;
using Unity.Services.Core;
using Unity.Services.Friends;
using UnityEngine;

public class UnityServicesInit : MonoBehaviour
{
    LoadCorrectSceneInit loadCorrectSceneInit;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static async void InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    void Awake()
    {
        Invoke(nameof(Setup), 0.1f);
    }

    private void Setup() 
    {
        try
        {
            loadCorrectSceneInit = GetComponentInChildren<LoadCorrectSceneInit>();
            loadCorrectSceneInit.LoadCorrectScene();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
