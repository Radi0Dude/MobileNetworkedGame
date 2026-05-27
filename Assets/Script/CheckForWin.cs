using UnityEngine;

public class CheckForWin : MonoBehaviour
{

    GoToScene goToScene;

    private void Start()
    {
        //Make sure that it is false on start
        CheckForNewScore.Instance.hasBeenScored = false;
        goToScene = GetComponent<GoToScene>();
    }
    public void CheckForWinCondition()
    {
       
        OnWin();
    }

    public void OnWin() 
    { 
        goToScene.GoToSceneEvent();
    }
}
