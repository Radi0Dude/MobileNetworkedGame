using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    UnityEvent goToSceneEvent;
    [SerializeField]
    string sceneName;

    

    public void GoToSceneEvent() 
    {
        SceneManager.LoadScene(sceneName);
    }
}
