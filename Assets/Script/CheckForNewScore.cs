using UnityEngine;

public class CheckForNewScore
{
    private static CheckForNewScore _instance;
    public static CheckForNewScore Instance => _instance ??= new CheckForNewScore();

    public bool hasBeenScored 
    {
        get => PlayerPrefs.GetInt("HasBeenScored", 0) == 1;
        set
        {
            PlayerPrefs.SetInt("HasBeenScored", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    
}
