using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SaveAndLoadManager
{
    private static SaveAndLoadManager _instance;
    public static SaveAndLoadManager Instance => _instance ??= new SaveAndLoadManager();

    string folderPath = Path.Combine(Application.persistentDataPath, "PosterFolder");
    public List<PlayerShows> playerShows = new List<PlayerShows>();
    public void SavePlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        Debug.Log($"Player name '{playerName}' saved to PlayerPrefs.");
    }
    public void SaveCurrentLogInMethod(LogInMethod logInMethod)
    {
        PlayerPrefs.SetInt("LogInMethod", (int)logInMethod);
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
        return (LogInMethod)PlayerPrefs.GetInt("LogInMethod", (int)LogInMethod.None);
    }



    public async Task SavePosterToDevice(Texture2D sprite, string posterID)
    {


        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string path = Path.Combine(folderPath, $"poster_{posterID}.jpeg");


        byte[] pngData = sprite.EncodeToJPG();
        await File.WriteAllBytesAsync(path, pngData);

    }



    public async Task<Texture2D> LoadPoster(string posterID)
    {
        string path = Path.Combine(folderPath, $"poster_{posterID}.png");
        Texture2D texture;
        if (File.Exists(path))
        {
            byte[] bytes = await File.ReadAllBytesAsync(path);
            texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return texture;
        }
        else
        {
            Debug.LogWarning($"Poster with ID {posterID} not found at path: {path}");
            return null;
        }

    }

    

    public void SaveShowIDs(string showIDs, int weight, string showURL, string showName)
    {
        playerShows.Clear();
        LoadShowIDs();
        playerShows.Add(new PlayerShows { showId = showIDs, weight = weight, showURL = showURL, title = showName });

        PlayerShowsWrapper wrapper = new PlayerShowsWrapper { shows = playerShows };
        string json = JsonUtility.ToJson(wrapper);


        PlayerPrefs.SetString("SavedShows", json);
        PlayerPrefs.Save();

    }

    public void LoadShowIDs()
    {
        string json = PlayerPrefs.GetString("SavedShows", string.Empty);

        if (!string.IsNullOrEmpty(json))
        {
            PlayerShowsWrapper wrapper = JsonUtility.FromJson<PlayerShowsWrapper>(json);
            playerShows = wrapper.shows ?? new List<PlayerShows>();
        }
        else
        {
            playerShows = new List<PlayerShows>();
        }
    }

    [System.Serializable]
    public class PlayerShowsWrapper
    {
        public List<PlayerShows> shows = new List<PlayerShows>();
    }
    [System.Serializable]
    public class PlayerShows
    {
        //this is here to save every ShowID that the player has collected
        public string showId;
        public int weight;
        public string showURL;
        public string title;

    }
    [System.Serializable]
    public enum LogInMethod
    {
        None,
        Anonymous,
        Unity,
        EmailAndPassword
    }
}
