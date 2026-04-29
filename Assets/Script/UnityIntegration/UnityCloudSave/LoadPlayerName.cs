using System.Collections;
using TMPro;
using UnityEngine;

public class LoadPlayerName : MonoBehaviour
{
    [SerializeField]
    TMP_Text playerNameText;

    private void Start()
    {
        if (playerNameText == null)
            playerNameText = FindAnyObjectByType<TMP_Text>();

        string playerNameValue = UnityAuthManager.Instance.GetPlayerName();

        if (string.IsNullOrEmpty(playerNameValue))
        {
            playerNameText.text = "Welcome";
            Debug.Log("Player name not found.");
            return;
        }
        
        StartCoroutine(WelcomePlayerBack(playerNameValue));
    }

    IEnumerator WelcomePlayerBack(string playerName) 
    { 
        playerNameText.text = $"Welcome back, {playerName}!";
        yield return new WaitForSeconds(3f);
        playerNameText.text = playerName;
    }
}
