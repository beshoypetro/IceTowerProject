using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    private bool _isGameOver = false;

    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Gameplay
    }

    public GameState CurrentState { get; private set; }

    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
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
            // SceneManager.LoadScene(mainMenuSceneName);
        }

        CurrentState = GameState.MainMenu;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
        _isGameOver = false;
        CurrentState = GameState.Gameplay;
    }

    public bool isGameOver()
    {
        return this._isGameOver;
    }

    public void GameOver()
    {
        Debug.Log("entered");
        if (_isGameOver) return;

        _isGameOver = true;

        // 1. Show game over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // 2. Slow motion effect
        // StartCoroutine(SlowMotionEffect());

        // 3. Play sound
        // AudioManager.Instance?.PlayGameOverSound();

        // 4. Disable player control
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.enabled = false;
        }
    }

    // public void RestartGame()
    // {
    //     Time.timeScale = 1f;
    //     Time.fixedDeltaTime = 0.02f;
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(
    //         UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    // }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
        CurrentState = GameState.MainMenu;
    }
}