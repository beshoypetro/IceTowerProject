using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; // Platform prefab to instantiate
    public GameObject leftWallPrefab; // Left wall prefab to instantiate
    public GameObject rightWallPrefab; // Right wall prefab to instantiate
    public Transform player; // Reference to the player
    public float platformDistance = 4f; // Distance between platforms
    public int initialPlatforms = 8; // Number of initial platforms to generate
    public float centerY; // The center Y position of the game
    public float wallLeftX = -14.3f; // Left wall X position
    public float wallRightX = 14.3f; // Right wall X position
    public float wallSegmentHeight = 14f; // Height of each wall segment

    private float nextPlatformSpawnY; // Y coordinate for the next platform spawn
    private float nextWallSpawnY; // Y coordinate for the next wall spawn
    private bool hasReachedCenter = false; // Flag to check if player has reached the center
    private float lastXPosition; // Last X position of the spawned platform

    void Start()
    {
        //nextPlatformSpawnY = 0f ; // Initialize platform spawn position at the top edge of the camera
        nextWallSpawnY = wallSegmentHeight; // Initialize wall spawn position at the top edge of the camera
        PreGeneratePlatforms(); // Generate initial platforms
        SpawnWallSegments(); // Spawn initial walls
    }

    void Update()
    {
        // Check if the player has reached the center of the game
        if (!hasReachedCenter && player.position.y >= centerY)
        {
            hasReachedCenter = true; // Set the flag to start spawning platforms and walls
        }

        // Check if the player has reached the spawn height and has reached the center
        if (hasReachedCenter && player.position.y >= nextPlatformSpawnY - Camera.main.orthographicSize)
        {
            SpawnPlatform(); // Spawn a new platform
        }

        // Check if the player has reached the wall spawn height
        if (player.position.y >= nextWallSpawnY - wallSegmentHeight)
        {
            SpawnWallSegments(); // Spawn new wall segments
            SpawnWallSegments(); // Spawn another set of wall segments
        }
    }

    void PreGeneratePlatforms()
    {
        // Generate platforms below the center
        nextPlatformSpawnY = centerY - (platformDistance * 1); // Adjust to start below center
        for (int i = 0; i < initialPlatforms / 2; i++)
        {
            SpawnPlatform();
        }
        // Generate platforms above the center
        nextPlatformSpawnY = centerY + platformDistance; // Adjust to start above center
        for (int i = 0; i < initialPlatforms / 2; i++)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        float x;
        float minXDistance = 4f; // Minimum horizontal distance from the last platform
        float maxXDistance = 9f; // Maximum horizontal distance from the last platform

        // Ensure the new platform is within the specified range and not far from the last one
        do
        {
            // Random X position within walls
            x = Random.Range((float)(wallLeftX - 3.78), (float)(wallRightX + 3.78));
        } while (Mathf.Abs(x - lastXPosition) < minXDistance || Mathf.Abs(x - lastXPosition) > maxXDistance);

        Vector3 spawnPosition = new Vector3(x, nextPlatformSpawnY, 0f);
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        nextPlatformSpawnY += platformDistance; // Increment next spawn position by fixed distance
        lastXPosition = x; // Update the last X position
    }

        void SpawnWallSegments()
    {
        // Spawn left wall segment at the correct horizontal position
        Vector3 leftWallPosition = new Vector3(wallLeftX, nextWallSpawnY, 0f);
        Instantiate(leftWallPrefab, leftWallPosition, Quaternion.Euler(0, 0, 90)); // Applying 90-degree rotation

        // Spawn right wall segment at the correct horizontal position
        Vector3 rightWallPosition = new Vector3(wallRightX, nextWallSpawnY, 0f);
        Instantiate(rightWallPrefab, rightWallPosition, Quaternion.Euler(0, 0, 90)); // Applying 90-degree rotation

        nextWallSpawnY += wallSegmentHeight; // Increment next wall spawn position by wall segment height
    }
}
