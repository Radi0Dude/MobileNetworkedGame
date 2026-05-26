using System;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public int score;

    public event Action<int> OnScoreChanged;

    [SerializeField]
    TMP_Text scoreText;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }

    private void Awake()
    {
        if(scoreText == null)
        {
            Debug.Log("disregard score text");
            return;
        }
        UpdateScoreText(score);
        OnScoreChanged += UpdateScoreText;
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
