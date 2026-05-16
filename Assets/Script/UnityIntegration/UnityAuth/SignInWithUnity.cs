using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

public class SignInWithUnity : MonoBehaviour
{
    public async void StartSignInProcces() 
    {
        if (PlayerAccountService.Instance.IsSignedIn) 
        { 
            await UnityAuthManager.Instance.SignInWithUnity();
        }

        try
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch (PlayerAccountsException e)   
        { 
            Debug.LogException(e);
        }
        catch (RequestFailedException e)
        { 
            Debug.LogException(e);
        }

        GetComponent<GoToScene>().GoToSceneEvent();
    }
}
