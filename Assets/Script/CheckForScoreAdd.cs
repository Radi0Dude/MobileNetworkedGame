using UnityEngine;

public class CheckForScoreAdd : MonoBehaviour
{
    GetTVSeriesID getTVSeriesID;
    private void Start()
    {
        if (CheckForNewScore.Instance.hasBeenScored)
        {
            UpdateScore();
        }
        getTVSeriesID = FindAnyObjectByType<GetTVSeriesID>();
    }

    private void UpdateScore()
    {
        getTVSeriesID.GetShowInfo();
    }
}
