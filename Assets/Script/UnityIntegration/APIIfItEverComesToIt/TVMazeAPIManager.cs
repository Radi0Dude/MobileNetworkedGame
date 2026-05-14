using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TVMazeAPIManager
{
    private static TVMazeAPIManager _instance;
    public static TVMazeAPIManager Instance => _instance ??= new TVMazeAPIManager();
    public async Task<bool> ShowExists(string showId, int randomIndex)
    {
        string url = $"https://api.tvmaze.com/shows?imdb={showId}";
        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            await GetShowDetails(showId, randomIndex);
            return true;
        }


        if (request.responseCode == 404)
            return false;

        throw new Exception($"Error checking show existence: {request.error}");
    }

    public async Task<TVShow> GetShowDetails(string showId, int randomIndex)
    {
        string url = $"https://api.tvmaze.com/lookup/shows?imdb={showId}";

        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();


        if (request.responseCode == 404)
        {
            //Call cloud code to remove this entry from the database, since it doesn't exist.
            // do this in cloud code
            //jsonFile = JsonUtility.ToJson(database);

            await UnityCloudCodeManager.Instance.RemoveShow(showId);

            throw new Exception($"Error fetching show details: {request.error}");

        }
            
        var shows = JsonConvert.DeserializeObject<TVShow>(request.downloadHandler.text);

        if (shows == null)
            throw new Exception("Show not found");

        return shows;

    }

    public async Task<Sprite> GetShowPoster(string spriteURL) 
    { 
        using var request = UnityWebRequestTexture.GetTexture(spriteURL);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            throw new Exception($"Error fetching image: {request.error}");
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    
}
public class TVShow
{
    [JsonProperty("url")]
    public string url;

    [JsonProperty("name")]
    public string name;

    [JsonProperty("weight")]
    public int weight;

    [JsonProperty("image")]
    public TVShowImage image;
}
public class TVShowImage
{
    [JsonProperty("medium")]
    public string medium;

    [JsonProperty("original")]
    public string original;
}
