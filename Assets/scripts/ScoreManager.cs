using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private int score = 0;
    private float highestYReached = float.MinValue;
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
        if (platformY > highestYReached)
        {
            highestYReached = platformY;
            int newScore = Mathf.FloorToInt(highestYReached / platformDistance);
            if (newScore > score)
            {
                score = newScore;
                gameObject.GetComponent<TextMeshProUGUI>().SetText(score.ToString());
            }
        }
    }

    public int GetScore()
    {
        return score;
    }
} 