using UnityEngine;

public class HighScore : MonoBehaviour
{
    [SerializeField]
    int currentScore;

    Score score;

    int highScore;

    ScoreCounter scoreCounter;
    private void Awake()
    {
        scoreCounter = FindFirstObjectByType<ScoreCounter>();
        scoreCounter.OnScoreChanged += CheckHighScoreUpdate;
        LoadHighScoreFromJson();
    }

    private void CheckHighScoreUpdate(int score)
    {
        currentScore = score;
        if (currentScore > highScore)
        {
            HighscoreChange();
        }
    }

    private void HighscoreChange()
    {
        highScore = currentScore;
        SaveHigescoreToJson();
    }
    private void SaveHigescoreToJson()
    {
        //Sace to Json if score is higher than what is already saved

    }

    private void LoadHighScoreFromJson()
    {
        //Load from Json and set highscore
        
    }
}

public struct Score
{
    public int highScore;

}