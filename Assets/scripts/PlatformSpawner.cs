using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;
    public Transform player;
    public float platformDistance = 2f;
    public int initialPlatforms = 20;
    public float wallLeftX = -14.4f;
    public float wallRightX = 14.4f;
    public float wallSegmentHeight = 16f;
    public ObjectPool pool;
    public int maxPlatforms = 20;

    private float nextPlatformSpawnY;
    private float centerY;
    private bool hasReachedCenter = false;
    private float lastXPosition;
    private int currentPlatformCount = 0;
    private Camera mainCam;

    private float topWallY;
    private int initialWallSegments = 3;

    private HashSet<float> activePlatformYPositions = new HashSet<float>();

    [System.Obsolete]
    void Start()
    {
        pool = FindObjectOfType<ObjectPool>();
        mainCam = Camera.main;

        if (pool == null)
        {
            Debug.LogError("ObjectPool component not found in the scene.");
            return;
        }

        centerY = mainCam.transform.position.y;
        float bottomY = centerY;
        nextPlatformSpawnY = bottomY - mainCam.orthographicSize;

        lastXPosition = Random.Range(wallLeftX + 2f, wallRightX - 2f);
        topWallY = bottomY;

        for (int i = 0; i < initialWallSegments; i++)
        {
            SpawnWallSegmentAt(topWallY);
            topWallY += wallSegmentHeight;
        }

        PreGeneratePlatforms();
    }

    void Update()
    {
        if (!hasReachedCenter && player.position.y >= centerY)
        {
            hasReachedCenter = true;
        }

        if (hasReachedCenter && player.position.y >= nextPlatformSpawnY - mainCam.orthographicSize)
        {
            for (int i = 0; i < 3; i++)
            {
                SpawnPlatform(nextPlatformSpawnY);
                nextPlatformSpawnY += platformDistance;
            }
        }

        RecyclePlatforms();
        RecycleWalls();
    }

    void PreGeneratePlatforms()
    {
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform(nextPlatformSpawnY);
            nextPlatformSpawnY += platformDistance;
        }

        Debug.Log("Pre-generated platform Y positions:");
        foreach (var y in activePlatformYPositions)
        {
            Debug.Log($"Platform at Y: {y}");
        }
    }

    void SpawnPlatform(float spawnY)
    {
        if (currentPlatformCount >= maxPlatforms)
            return;

        float roundedY = Mathf.Round(spawnY * 100f) / 100f;
        if (activePlatformYPositions.Contains(roundedY))
        {
            Debug.LogWarning($"Skipped spawning: Y {roundedY} already used.");
            return;
        }

        float x;
        const float wallOffset = 3.78f;
        const float minHorizontalSpacing = 2.0f;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            x = Random.Range(wallLeftX - wallOffset, wallRightX + wallOffset);
            attempts++;
        } while (Mathf.Abs(x - lastXPosition) < minHorizontalSpacing && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            x = lastXPosition + minHorizontalSpacing * (Random.value > 0.5f ? 1 : -1);
            x = Mathf.Clamp(x, wallLeftX - wallOffset, wallRightX + wallOffset);
        }

        lastXPosition = x;

        GameObject platform = pool.GetPooledObject("Platform");
        if (platform != null)
        {
            platform.transform.position = new Vector3(x, roundedY, 0f);
            platform.SetActive(true);
            currentPlatformCount++;
            activePlatformYPositions.Add(roundedY);
            Debug.Log($"Platform spawned at Y: {roundedY}, X: {x}");
        }

        Debug.Log($"Trying to spawn at Y: {spawnY}, Rounded: {roundedY}, Already exists: {activePlatformYPositions.Contains(roundedY)}");
    }

    void SpawnWallSegmentAt(float y)
    {
        Vector3 leftWallPosition = new Vector3(wallLeftX, y, 3.5f);
        GameObject leftWall = pool.GetPooledObject("LeftWall");
        if (leftWall != null)
        {
            leftWall.transform.position = leftWallPosition;
            leftWall.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No available LeftWall in pool!");
        }

        Vector3 rightWallPosition = new Vector3(wallRightX, y, 3.5f);
        GameObject rightWall = pool.GetPooledObject("RightWall");
        if (rightWall != null)
        {
            rightWall.transform.position = rightWallPosition;
            rightWall.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No available RightWall in pool!");
        }
    }

    void RecyclePlatforms()
    {
        foreach (GameObject platform in pool.GetPooledObjects("Platform"))
        {
            if (platform.activeInHierarchy && platform.transform.position.y < player.position.y - mainCam.orthographicSize + 1)
            {
                float y = Mathf.Round(platform.transform.position.y * 100f) / 100f;
                platform.SetActive(false);
                currentPlatformCount--;
                activePlatformYPositions.Remove(y);

                SpawnPlatform(nextPlatformSpawnY);
                nextPlatformSpawnY += platformDistance;
            }
        }
    }

    void RecycleWalls()
    {
        foreach (GameObject leftWall in pool.GetPooledObjects("LeftWall"))
        {
            if (leftWall.activeInHierarchy && player.position.y > leftWall.transform.position.y + wallSegmentHeight)
            {
                float oldY = leftWall.transform.position.y;

                leftWall.SetActive(false);
                leftWall.transform.position = new Vector3(wallLeftX, topWallY, 3.5f);
                leftWall.SetActive(true);

                GameObject rightWall = FindMatchingRightWall(oldY);
                if (rightWall != null)
                {
                    rightWall.SetActive(false);
                    rightWall.transform.position = new Vector3(wallRightX, topWallY, 3.5f);
                    rightWall.SetActive(true);
                }

                topWallY += wallSegmentHeight;
            }
        }
    }

    GameObject FindMatchingRightWall(float y)
    {
        foreach (GameObject rightWall in pool.GetPooledObjects("RightWall"))
        {
            if (rightWall.activeInHierarchy && Mathf.Approximately(rightWall.transform.position.y, y))
            {
                return rightWall;
            }
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var y in activePlatformYPositions)
        {
            Gizmos.DrawLine(new Vector3(-20, y, 0), new Vector3(20, y, 0));
        }
    }
}