using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private int score = 0;
    private float lastPlatformY = float.MinValue;
    [SerializeField] private float platformDistance = 4f; // Set this to your platform spacing

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

    public void PlayerLandedOnPlatform(float platformY)
    {
        if (lastPlatformY == float.MinValue)
        {
            lastPlatformY = platformY;
            return;
        }
        if (platformY - lastPlatformY >= platformDistance)
        {
            score++;
            lastPlatformY = platformY;
            gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString();

        }
    }

    public int GetScore()
    {
        return score;
    }
} 