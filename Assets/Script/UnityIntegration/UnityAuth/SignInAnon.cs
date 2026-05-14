using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the anonymous sign-up process and loads a specified scene upon successful authentication.
/// </summary>


public class SignInAnon : MonoBehaviour
{


    [SerializeField, Header("Only use scene here")]
    UnityEngine.Object tutorialScene;

    string sceneName;

    

    private void Awake()
    {
        if (tutorialScene == null)
        {
            Debug.LogError("Load to scene is not assigned. Please assign a scene in the inspector.");
            return;
        }
        SignUpAnon();
        sceneName = tutorialScene.name;
    }


    public async void SignUpAnon()
    {
        try
        {
            await UnityAuthManager.Instance.SignUpAnon();
            SceneManager.LoadScene(sceneName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred during anonymous sign-up: {ex.GetBaseException().Message}");
        }
    }
}


