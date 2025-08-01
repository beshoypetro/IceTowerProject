using UnityEngine;

public class PlayerFallDetector : MonoBehaviour
{
    public float fallThreshold = 0.5f; // Extra buffer below camera
    private Camera _mainCamera;
    private bool _isGameOver = false;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (_isGameOver) return;
        
        // Calculate camera bottom position in world space
        float cameraBottom = _mainCamera.transform.position.y - 
                             _mainCamera.orthographicSize;
        
        // Player's bottom position (using collider if available)
        float playerBottom = GetPlayerBottomPosition();
        
        // Check if player has fallen below camera
        if (playerBottom < cameraBottom - fallThreshold)
        {
            TriggerGameOver();
        }
    }

    private float GetPlayerBottomPosition()
    {
        // Try to get collider first
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            return collider.bounds.min.y;
        }
        
        // Fallback to transform position
        return transform.position.y;
    }

    private void TriggerGameOver()
    {
        _isGameOver = true;
        Debug.Log("GAME OVER - Player fell!");
        
        // Implement your game over logic here:
        // 1. Show game over UI
        // 2. Pause game
        // 3. Play sound effect
        // 4. Vibrate device (mobile)
        
        // Example minimal implementation:
        GameManager.Instance?.GameOver();
    }
}
