using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public void OnStartGameButtonPressed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }
} 