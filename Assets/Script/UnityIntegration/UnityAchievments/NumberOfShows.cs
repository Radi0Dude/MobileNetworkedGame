using UnityEngine;

public class NumberOfShows
{
    private static NumberOfShows _instance;
    public static NumberOfShows Instance => _instance ??= new NumberOfShows();

    public int GetNumberOfShows()
    {
        return SaveAndLoadManager.Instance.playerShows.Count;
    }
}
