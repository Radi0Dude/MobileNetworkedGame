using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using UnityEngine;

public class LoadPlayerName : MonoBehaviour
{
    [SerializeField]
    TMP_Text playerNameText;
    private async void Start()
    {
        if(playerNameText == null)
            playerNameText = FindAnyObjectByType<TMP_Text>();
        var playerNameValue = await UnityCloudCodeManager.Instance.LoadPlayerName();
        if(string.IsNullOrEmpty(playerNameValue))
        {
            playerNameText.text = "Welcome";
            Debug.Log("Player name not found in Cloud Save.");
            return;
        }
        StartCoroutine(WelcomePlayerBack(playerNameValue.ToString()));
    }

    IEnumerator WelcomePlayerBack(string playerName) 
    { 
        playerNameText.text = $"Welcome back, {playerName}!";
        yield return new WaitForSeconds(3f);
        playerNameText.text = playerName;
    }
}
