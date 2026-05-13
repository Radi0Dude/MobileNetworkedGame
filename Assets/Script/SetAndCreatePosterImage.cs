using System;
using UnityEngine;
using UnityEngine.UI;

public class SetAndCreatePosterImage : MonoBehaviour
{
    [SerializeField]
    GameObject posterImageObject;

    GameObject currentPoster;

    public void SetEverythingPoster(string showName, string showURL, Texture2D poster)
    {
        currentPoster = Instantiate(posterImageObject);
        SetPosterImage(poster);
        SetPosterName(showName);
        SetPosterURL(showURL);
    }

    private void SetPosterURL(string showURL)
    {
        currentPoster.GetComponent<PosterInteractor>().url = showURL;
    }

    private void SetPosterName(string showName) 
    { 
        currentPoster.GetComponent<PosterInteractor>().showName = showName;
    }

    private void SetPosterImage(Texture2D poster)
    {
        currentPoster.transform.SetParent(this.transform, false);
        currentPoster.GetComponent<RawImage>().texture = poster;

    }
}
