using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using System.Text;
using WebSocketSharp;

public class SaveAndLoadManager
{
    
    private static SaveAndLoadManager _instance;
    public static SaveAndLoadManager Instance => _instance ??= new SaveAndLoadManager();

    string folderPath = Path.Combine(Application.persistentDataPath, "PosterFolder");
    public List<PlayerShows> playerShows = new List<PlayerShows>();

    private const string CustomDataId = "TvSeriesDatabaseJSON"; 
    private const string TvSeriesKey = "tvSeries";
    
    public TvSeriesDatabase tvSeriesDatabase;

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
        string path = Path.Combine(folderPath, $"poster_{posterID}.jpeg");
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

    public async Task LoadTVSeriesDataset()
    {
        var result = await CloudSaveService.Instance.Data.Custom.LoadAsync(CustomDataId, new HashSet<string> { TvSeriesKey });
        if (result.TryGetValue(TvSeriesKey, out var json))
        {
            tvSeriesDatabase = JsonUtility.FromJson<TvSeriesDatabase>(json.Value.GetAsString());
            Debug.Log($"Loaded TV series dataset with {tvSeriesDatabase.tvSeries.Length} entries.");
        }
        else
        {
            Debug.LogWarning("TV series dataset not found in Cloud Save.");
        }
    }

    public async void SaveShowIDs(string showIDs, int weight, string showURL, string showName)
    {
        playerShows.Clear();
        await LoadShowIDs();
        playerShows.Add(new PlayerShows { showId = showIDs, weight = weight, showURL = showURL, title = showName });

        PlayerShowsWrapper wrapper = new PlayerShowsWrapper { shows = playerShows };
        string json = JsonUtility.ToJson(wrapper);
        try
        {
            await UnityCloudSaveManager.Instance.SaveNewShow(json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save show IDs to Cloud Save: {ex.Message}");
        }


        PlayerPrefs.SetString("SavedShows", json);
        PlayerPrefs.Save();

    }

    public async Task LoadShowIDs()
    {
        string json = PlayerPrefs.GetString("SavedShows", string.Empty);
        string cloudJson = await UnityCloudSaveManager.Instance.LoadAllShows();   

        //bool check = CompareJsons(json, cloudJson);
        if (cloudJson.IsNullOrEmpty()) 
        {
            await UnityCloudSaveManager.Instance.SaveNewShow(json);
            json  = await UnityCloudSaveManager.Instance.LoadAllShows();
        }
        else
        {
            //if (!check)
            //{
            //    //if i have time then combine them for now im just using the cloud one just in case they have played on another device and have different shows saved
            //    //I know there will be problems if they ever decide to play offline
            //    json = cloudJson;
            //}
            //I know that both does the same thing now, but its just to show that i know they should be combined
            json = cloudJson;
        }
        


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

    bool CompareJsons(string json1, string json2)
    {
        PlayerShowsWrapper wrapperJson1 = JsonUtility.FromJson<PlayerShowsWrapper>(json1);
        PlayerShowsWrapper wrapperJson2 = JsonUtility.FromJson<PlayerShowsWrapper>(json2);
        PlayerShows[] shows1 = wrapperJson1.shows.ToArray();
        PlayerShows[] shows2 = wrapperJson2.shows.ToArray();
        
        if(shows1.Length != shows2.Length)
        {
            return false;
        }
        else
        {
            for(int i = 0; i < shows1.Length; i++)
            {
                if(shows1[i].showId != shows2[i].showId || shows1[i].weight != shows2[i].weight || shows1[i].showURL != shows2[i].showURL || shows1[i].title != shows2[i].title)
                {
                    return false;
                }
            }
            return true;
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

    [System.Serializable]
    public class TvSeriesDatabase
    {
        public TVSeriesID[] tvSeries;
    }
    [System.Serializable]
    public class TVSeriesID
    {
        public string id;
        public string title;
    }
}
