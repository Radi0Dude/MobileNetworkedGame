using System;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInAnon : MonoBehaviour
{
    [SerializeField, Header("Only use scene here")]
    UnityEngine.Object mainScene;
    [SerializeField, Header("Only use scene here")]
    UnityEngine.Object nameCreationScene;
    
    string sceneName;
    

    private void Awake()
    {
        if (mainScene == null)
        {
            Debug.LogError("Load to scene is not assigned. Please assign a scene in the inspector.");
            return;
        }
        sceneName = mainScene.name;
    }
   

    public async void SignUpAnon()
    {
        try
        {
            bool hasExistingSession = UnityAuthManager.Instance.HasExistinSession();

            await UnityAuthManager.Instance.SignUpAnon();

            if (hasExistingSession)
            {
                Debug.Log("Existing session found. Skipping anonymous sign-up.");
                var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {UnityCloudCodeManager.Instance.playerNameFinder});
                if(playerData.TryGetValue(UnityCloudCodeManager.Instance.playerNameFinder, out var playerName))
                {
                    sceneName = mainScene.name;
                    SceneManager.LoadScene(sceneName);
                    Debug.Log($"Player name found: {playerName.Value.GetAs<string>()}");
                }
                else
                {
                    Debug.Log("Player name not found. Redirecting to name creation scene.");
                    sceneName = nameCreationScene.name;
                    SceneManager.LoadScene(sceneName);
                    return;
                }
               
                
            }
            else
            {
                await UnityAuthManager.Instance.SignUpAnon();
                sceneName = nameCreationScene.name;
                SceneManager.LoadScene(sceneName);
            }
            
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred during anonymous sign-up: {ex.GetBaseException().Message}");
        }
    }
}


