using System.Collections;
using UnityEngine;

public class CheckForScoreAdd : MonoBehaviour
{
    GetTVSeriesID getTVSeriesID;
    private IEnumerator Start()
    {
        getTVSeriesID = FindAnyObjectByType<GetTVSeriesID>();
        yield return new WaitForSeconds(0.5f);
        if (CheckForNewScore.Instance.hasBeenScored)
        {
            UpdateScore();
        }
        
    }

    private async void UpdateScore()
    {
        //await getTVSeriesID.GetShowInfo();
        CheckForNewScore.Instance.hasBeenScored = false;
    }
}
