using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Gameplay }
    public GameState CurrentState { get; private set; }

    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Always load the main menu scene on start
        if (SceneManager.GetActiveScene().name != mainMenuSceneName)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        CurrentState = GameState.MainMenu;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
        CurrentState = GameState.Gameplay;
    }
} 