using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private float offsetY;
    public float minHeightToFollow = 1f;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private float highestY; // Track the highest Y the camera should be

    void Start()
    {
        offsetY = transform.position.y - player.position.y;
        highestY = transform.position.y; // Start with the camera's initial height
    }

    void LateUpdate()
    {
        if (player.position.y > minHeightToFollow)
        {
            float targetY = player.position.y + offsetY;

            // Only update if the target is higher than the current highestY
            if (targetY > highestY)
            {
                highestY = targetY;
            }

            // Smoothly move towards highestY, but never go down
            Vector3 targetPosition = new Vector3(transform.position.x, highestY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}