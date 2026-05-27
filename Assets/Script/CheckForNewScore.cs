using UnityEngine;

public class CheckForNewScore
{
    private static CheckForNewScore _instance;
    public static CheckForNewScore Instance => _instance ??= new CheckForNewScore();

    public bool hasBeenScored = false;

    public void ChangeBool()
    {
        hasBeenScored = !hasBeenScored;
    }
}
