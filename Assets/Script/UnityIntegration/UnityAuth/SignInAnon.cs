using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInAnon : MonoBehaviour
{
    [SerializeField, Header("Only use scene here")]
    UnityEngine.Object loadToScene;
    string sceneName;

    private void Awake()
    {
        if (loadToScene == null)
        {
            Debug.LogError("Load to scene is not assigned. Please assign a scene in the inspector.");
            return;
        }
        sceneName = loadToScene.name;
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


