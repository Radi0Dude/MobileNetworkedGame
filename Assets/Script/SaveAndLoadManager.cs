using UnityEngine;

public class SaveAndLoadManager
{
    private static SaveAndLoadManager _instance;
    public static SaveAndLoadManager Instance => _instance ??= new SaveAndLoadManager();

    public void SavePlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        Debug.Log($"Player name '{playerName}' saved to PlayerPrefs.");
    }
    public void SaveCurrentLogInMethod(LogInMethod logInMethod)
    {
        PlayerPrefs.SetInt("LogInMethod", (int) logInMethod);
        PlayerPrefs.Save();
        Debug.Log($"Current login method '{logInMethod}' saved to PlayerPrefs.");
    }
    public void SaveAnonymousUserId(string userId)
    {
        PlayerPrefs.SetString("AnonymousUserId", userId);
        PlayerPrefs.Save();
        Debug.Log($"Anonymous user ID '{userId}' saved to PlayerPrefs.");
    }
    public string LoadAnonUserId()
    {
        string userId = PlayerPrefs.GetString("AnonymousUserId", string.Empty);
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogWarning("No anonymous user ID found in PlayerPrefs.");
            return string.Empty;
        }
        else
        {
            Debug.Log($"Loaded anonymous user ID: {userId}");
            return userId;
        }
    }
    public LogInMethod GetCurrentLogInMethod()
    {
         return (LogInMethod) PlayerPrefs.GetInt("LogInMethod", (int) LogInMethod.None);
    }
}

public enum LogInMethod
{
    None,
    Anonymous,
    Unity,
    EmailAndPassword
}
