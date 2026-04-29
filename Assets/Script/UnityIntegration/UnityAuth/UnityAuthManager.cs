using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

public class UnityAuthManager
{

    private static UnityAuthManager _instance;
    public static UnityAuthManager Instance => _instance ??= new UnityAuthManager();

    private UnityAuthManager()
    {
        Debug.Log("UnityAuthManager initialized.");
    }
    public bool HasExistinSession()
    {
        return AuthenticationService.Instance.SessionTokenExists;
    }
    public async Task SignUpAnon()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            SaveAndLoadManager.Instance.SaveCurrentLogInMethod(LogInMethod.Anonymous);
            SaveAndLoadManager.Instance.SaveAnonymousUserId(AuthenticationService.Instance.PlayerId);
            Debug.Log("Anonymous sign-up successful.");
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"An error occurred during anonymous sign-up: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogWarning($"An error occure: {ex.Message}");
        }
    }

    public async Task UpgradeToEmailAndPassword(string email, string password)
    {
        try
        {
            await AuthenticationService.Instance.AddUsernamePasswordAsync(email, password);
            SaveAndLoadManager.Instance.SaveCurrentLogInMethod(LogInMethod.EmailAndPassword);            

            Debug.Log("Sign-in with email and password successful.");
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"An error occurred during sign-in with email and password: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogWarning($"An error occure: {ex.Message}");
        }
    }

    public async Task SignInWithUnity()
    {
        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.LinkWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                Debug.Log("Successfully linked Unity Account to existing Anonymous profile!");
            }
            else
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                Debug.Log("Sign-in with Unity successful.");
            }

            SaveAndLoadManager.Instance.SaveCurrentLogInMethod(LogInMethod.Unity);
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex) 
        { 
            Debug.LogException(ex);
        }
    }

    public async Task ChangeUsername(string username)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"Failed to update player name: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
    public string GetPlayerName()
    {
        try
        {
            string playerName = AuthenticationService.Instance.PlayerName;
            Debug.Log($"Retrieved Player Name: {playerName}");
            return playerName;
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while retrieving Player Name: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> SignInAsync(string id, string password)
    {
        // Simulate an asynchronous sign-in process
        await Task.Delay(1000); // Simulate network delay
        Debug.Log($"Sign-in successful for ID: {id}");
        //player managment id cant be found call sign up and ask them for a confirmation on the password and then sign them in
        return true; // Return true for successful sign-in
    }

    public Task<bool> SignUpAsync(string id, string password)
    {
        // Simulate an asynchronous sign-up process
        return Task.Run(() =>
        {
            Debug.Log($"Sign-up successful for ID: {id}");
            return true; // Return true for successful sign-up
        });
    }

    public Task<string> GetPlayerId()
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"Retrieved Player ID: {playerId}");
            return Task.FromResult(playerId); 

        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while retrieving Player ID: {ex.Message}");
            return Task.FromResult<string>(null);
        }
    }
}
