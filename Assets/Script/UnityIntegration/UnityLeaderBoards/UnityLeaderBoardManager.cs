using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using UnityEngine;

public class UnityLeaderBoardManager : MonoBehaviour
{
    private static UnityLeaderBoardManager _instance;
    public static UnityLeaderBoardManager Instance => _instance ??= new UnityLeaderBoardManager();

    private static readonly string leaderboardID = "HighScore";

    public async Task AddScore(int score)
    {
        var entry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
    }
    public async Task GetAllScores() 
    { 
        var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID, new GetScoresOptions { Limit = 25 });
    }
}
