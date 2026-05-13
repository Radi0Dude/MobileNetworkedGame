using System.Threading.Tasks;
using UnityEngine;

public class GetTVSeriesID : MonoBehaviour
{
    [SerializeField]
    string currentID = string.Empty;
    [SerializeField]
    Sprite currentPoster;

    SetAndCreatePosterImage posterImageSetter;
    private void Start()
    {
        posterImageSetter = FindAnyObjectByType<SetAndCreatePosterImage>();
        GetRandomShow();
    }
    [ContextMenu("Get Random Show")]
    public void GetRandomShow()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("TvSeries");
        TvSeriesDatabase database = JsonUtility.FromJson<TvSeriesDatabase>(jsonFile.text);

        TVSeriesID randomShow = database.tvSeries[Random.Range(0, database.tvSeries.Length)];
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

        var show = await TVMazeAPIManager.Instance.GetShowDetails(currentID);
        Debug.Log($"Show Name: {show.name}, URL: {show.url}, show weight {show.weight}");
        string imageUrl = show.image?.medium ?? "No image available";
        Debug.Log($"Image URL: {imageUrl}");
        currentPoster = await TVMazeAPIManager.Instance.GetShowPoster(imageUrl);
        Texture2D posterTexture = currentPoster.texture;

        await SaveAndLoadManager.Instance.SavePosterToDevice(posterTexture, currentID);
        posterImageSetter.SetEverythingPoster(show.name, show.url, posterTexture);
        GetRandomShow();


    }



    public async Task GetRandomShowCloudCode()
    {
        string showId = await UnityCloudCodeManager.Instance.GetRandomShow();
    }

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