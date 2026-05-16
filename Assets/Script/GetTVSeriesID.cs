using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GetTVSeriesID : MonoBehaviour
{
    [SerializeField]
    string currentID = string.Empty;
    [SerializeField]
    Sprite currentPoster;

    int currentRandomIndex;

    SetAndCreatePosterImage posterImageSetter;
    private async void Start()
    {
        await SaveAndLoadManager.Instance.LoadTVSeriesDataset();
        posterImageSetter = FindAnyObjectByType<SetAndCreatePosterImage>();
        LoadShowsOnStart();
        GetRandomShow();
        
        StartCoroutine(TestingHundredsOfShows());

    }
    [ContextMenu("Test Hundreds of Shows")]
    public IEnumerator TestingHundredsOfShows()
    {
        for (int i = 0; i < 50; i++)
        {
            Debug.Log($"Testing show number {i + 1}");
            GetShowInfo();
            GetRandomShow();
            yield return new WaitForSeconds(Random.Range(0.5f, 0.8f));
        }

    }

    [ContextMenu("Get Random Show")]
    public void GetRandomShow()
    {
       
        

        var database = SaveAndLoadManager.Instance.tvSeriesDatabase;
        currentRandomIndex = Random.Range(0, database.tvSeries.Length);
        
        SaveAndLoadManager.TVSeriesID randomShow = database.tvSeries[currentRandomIndex];
        Debug.Log($"Random TV Series: {randomShow.title} (ID: {randomShow.id})");
        currentID = randomShow.id;
    }
    [ContextMenu("Get Show Info")]
    public async void GetShowInfo(/*string id*/)
    {
        //if (currentID != id && id != string.Empty)
        //{
        //    currentID = id;
        //}
        while (true)
        {
            SaveAndLoadManager.Instance.LoadShowIDs();

            if (!SaveAndLoadManager.Instance.playerShows.Any(s => s.showId == currentID))
                break;

            Debug.Log($"Show with ID {currentID} already exists. Skipping.");
            GetRandomShow();
        }

        var show = await TVMazeAPIManager.Instance.GetShowDetails(currentID, currentRandomIndex);
        Debug.Log($"Show Name: {show.name}, URL: {show.url}, show weight {show.weight}");
        string imageUrl = show.image?.medium ?? "No image available";
        Debug.Log($"Image URL: {imageUrl}");
        currentPoster = await TVMazeAPIManager.Instance.GetShowPoster(imageUrl);
        Texture2D posterTexture = currentPoster.texture;

        await SaveAndLoadManager.Instance.SavePosterToDevice(posterTexture, currentID);
        posterImageSetter.SetEverythingPoster(show.name, show.url, posterTexture);
        SaveAndLoadManager.Instance.SaveShowIDs(currentID, show.weight, show.url, show.name);
        GetRandomShow();


    }
    public async void LoadShowsOnStart() 
    { 
        SaveAndLoadManager.Instance.LoadShowIDs();
        List<SaveAndLoadManager.PlayerShows> playerShows = SaveAndLoadManager.Instance.playerShows;
        foreach (var show in playerShows)
        {
            Debug.Log($"Loaded Show: {show.title} (ID: {show.showId}, Weight: {show.weight}, URL: {show.showURL})");
            Texture2D posterTexture = await SaveAndLoadManager.Instance.LoadPoster(show.showId);
            posterImageSetter.SetEverythingPoster(show.title, show.showURL, posterTexture);
        }
    }
    


    

}

