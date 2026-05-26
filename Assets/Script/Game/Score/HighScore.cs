using System.Threading.Tasks;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    [SerializeField]
    int currentScore;
    [SerializeField]
    int weightMultiplier = 2;



    ScoreCounter scoreCounter;
    private async void Start()
    {
        scoreCounter = FindFirstObjectByType<ScoreCounter>();
        await LoadHigScore();
        HighscoreChange();
    }

   

    private void HighscoreChange()
    {
        scoreCounter.UpdateScoreText(currentScore);
    }
    

    private async Task LoadHigScore()
    {
        try
        {
            await SaveAndLoadManager.Instance.LoadShowIDs();
            foreach (var playerShow in SaveAndLoadManager.Instance.playerShows)
            {
                int effectiveWeight = Mathf.Max(10, playerShow.weight);
                currentScore += effectiveWeight * weightMultiplier;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load high score: {ex.Message}");
        }

    }
}

