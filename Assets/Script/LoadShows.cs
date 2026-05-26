using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoadShows : MonoBehaviour
{
    SetAndCreatePosterImage posterImageSetter;

    private void Awake()
    {
        posterImageSetter = FindAnyObjectByType<SetAndCreatePosterImage>();
    }

    private async void Start()
    {
        await LoadShowsOnStart();
    }

    public async Task LoadShowsOnStart()
    {
        List<SaveAndLoadManager.PlayerShows> playerShows = null;

        try
        {
            await SaveAndLoadManager.Instance.LoadShowIDs();


            playerShows = SaveAndLoadManager.Instance.playerShows;
            foreach (var show in playerShows)
            {
                Debug.Log($"Loaded Show: {show.title} (ID: {show.showId}, Weight: {show.weight}, URL: {show.showURL})");
                Texture2D posterTexture = await SaveAndLoadManager.Instance.LoadPoster(show.showId);
                posterImageSetter.SetEverythingPoster(show.title, show.showURL, posterTexture);
            }

        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading shows: {ex.Message}");
        }
    }
}
