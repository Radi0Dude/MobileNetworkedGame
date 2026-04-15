using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
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

    public async Task SignUpAnon()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
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
