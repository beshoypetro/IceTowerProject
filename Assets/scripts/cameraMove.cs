using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private float offsetY; // Initial vertical offset from the player
    private bool shouldFollow = false; // Flag to start following player
    public float minHeightToFollow = 1f; // Minimum height player needs to reach before following
    public float smoothTime = 0.3f; // Time for smooth damping
    private Vector3 velocity = Vector3.zero; // Reference velocity for smooth damping

    void Start()
    {
        // Set initial offset based on the starting positions of the camera and player
        offsetY = transform.position.y - player.position.y;
    }

    void LateUpdate()
    {
        // Check if the player has reached the camera's vertical position and a minimum height
        if (player.position.y >= transform.position.y - offsetY && player.position.y > minHeightToFollow)
        {
            shouldFollow = true; // Set the flag to follow the player
        }

        // Only update the camera's vertical position if the flag is set
        if (shouldFollow)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, player.position.y + offsetY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
