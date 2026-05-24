using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine;

public class UnityCloudSaveManager
{
    private static UnityCloudSaveManager _instance;
    public static UnityCloudSaveManager Instance => _instance ??= new UnityCloudSaveManager();
    public string playerNameFinder = "PlayerName";

    //I used this to save the username before i knew tha unity had that feature built in.
    //public async Task<string> LoadPlayerName()
    //{
    //    var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { UnityCloudCodeManager.Instance.playerNameFinder });
    //    if (playerData.TryGetValue(UnityCloudCodeManager.Instance.playerNameFinder, out var playerName))
    //    {
    //        string playerNameValue = playerName.Value.GetAs<string>();
    //        Debug.Log($"Player name found: {playerName.Value.GetAs<string>()}");
    //        return playerNameValue;            
    //    }
    //    else { return null; }
    //}
    //public async Task SavePlayerName(string name)
    //{
    //    var data = new Dictionary<string, object> { { playerNameFinder, name } };
    //    await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    //}

    public async Task SaveNewShow(string json)
    {
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> { { "PlayerShows", json } });
            Debug.Log("Show data saved successfully to cloud");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save show data to cloud: {ex.Message}");
            throw;
        }
    }
    public async Task<string> LoadAllShows()
    {
        string showsJson = string.Empty;
        try
        {
            Dictionary<string, Item> showsJsonObject = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "PlayerShows" });
            Item showsItem;
            if (showsJsonObject != null && showsJsonObject.ContainsKey("PlayerShows"))
            {
                showsItem = showsJsonObject["PlayerShows"];
                showsJson = showsItem.Value.GetAs<string>();
                return showsJson;
            }
            else
            return string.Empty;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load show data from cloud: {ex.Message}");
            throw;
        }
        
        
    }
}
