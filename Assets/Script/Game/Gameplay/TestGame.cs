using UnityEngine;
using UnityEngine.InputSystem;

public class TestGame : MonoBehaviour
{

    ScoreCounter scoreCounter;

    private void Awake()
    {
        scoreCounter = GetComponent<ScoreCounter>();

    }

    public void AddScore(InputAction.CallbackContext context)
    {
        if(context.performed)
        scoreCounter.Score += 1;
    }
}
