using System;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveAndLoadManager;

public class LoadCorrectSceneInit : MonoBehaviour
{
    [Header("Remember to set the string using the three dots")]

    [SerializeField, Header("Only use scene here")]
    UnityEngine.Object signInAnonLoad;

    [SerializeField, Header("Only use scene here")]
    UnityEngine.Object tutorialScene;

    [SerializeField, Header("Only use scene here")]
    UnityEngine.Object mainGame;

    [SerializeField]
    string signInAnonLoadName;

    [SerializeField]
    string tutorialSceneName;

    [SerializeField]
    string mainGameName;

    [ContextMenu("Set String")]
    public void SetString()
    {
        if (signInAnonLoad != null) signInAnonLoadName = signInAnonLoad.name;
        if (tutorialScene != null) tutorialSceneName = tutorialScene.name;
        if (mainGame != null) mainGameName = mainGame.name;
    }


    public async void LoadCorrectScene()
    {
        LogInMethod logInMethod = SaveAndLoadManager.Instance.GetCurrentLogInMethod();
        try
        {
            
            if (!AuthenticationService.Instance.SessionTokenExists)
            {
                Debug.Log("No session. User is brand new.");
                LoadSceneForNewUsers();
                return;
            }
            else if(logInMethod == LogInMethod.Anonymous)
            {
                await UnityAuthManager.Instance.SignUpAnon();
                string playerInfo = await UnityAuthManager.Instance.GetPlayerName();
                if(string.IsNullOrEmpty(playerInfo))
                {
                    Debug.Log("User has no username. Load scene for people with out username");
                    LoadSceneForPeopleWithOutUsername();
                    return;
                }
                else
                {
                    Debug.Log("User has username. Load scene for returning users");
                    LoadSceneForReturningUsers();
                    return;
                }
            }
            else 
            {
                LoadSceneForReturningUsers();
            }

        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            if(LogInMethod.None == logInMethod)
            {
                LoadSceneForNewUsers();
            }
            else
            {

                LoadSceneForReturningUsers();
            }
        }
        
    }
    private void LoadSceneForNewUsers()
    {
        SceneManager.LoadScene(signInAnonLoadName);
    }
    private void LoadSceneForPeopleWithOutUsername()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }
    private void LoadSceneForReturningUsers()
    {
        SceneManager.LoadScene(mainGameName);
    }
}
