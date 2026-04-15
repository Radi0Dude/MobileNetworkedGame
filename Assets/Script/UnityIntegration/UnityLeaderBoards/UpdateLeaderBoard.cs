using System;
using UnityEngine;

public class UpdateLeaderBoard : MonoBehaviour
{
    ScoreCounter scoreCounter;

    private void Awake()
    {
        scoreCounter = FindFirstObjectByType<ScoreCounter>();
        scoreCounter.OnScoreChanged += UpdateLeaderboard;
    }

    private void UpdateLeaderboard(int score)
    {
        
    }
}
