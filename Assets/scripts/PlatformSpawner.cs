using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;
    public Transform player;
    public float platformDistance = 4f;
    public int initialPlatforms = 8;
    public float centerY;
    public float wallLeftX = -14.4f;
    public float wallRightX = 14.4f;
    public float wallSegmentHeight = 16f;
    public ObjectPool pool;
    public int maxPlatforms = 20;

    private float nextPlatformSpawnY;
    private float nextWallSpawnY;
    private bool hasReachedCenter = false;
    private float lastXPosition;
    private int currentPlatformCount = 0;

    void Start()
    {
        pool = FindObjectOfType<ObjectPool>();

        if (pool == null)
        {
            Debug.LogError("ObjectPool component not found in the scene.");
            return;
        }

        nextWallSpawnY = wallSegmentHeight;
        nextPlatformSpawnY = centerY - platformDistance;
        Debug.Log("Initial nextPlatformSpawnY: " + nextPlatformSpawnY);

        PreGeneratePlatforms();
        SpawnWallSegments();
    }

    void Update()
    {
        if (!hasReachedCenter && player.position.y >= centerY)
        {
            hasReachedCenter = true;
        }

        // Spawn platforms when the player is near the next platform spawn position
    if (hasReachedCenter && player.position.y >= nextPlatformSpawnY - Camera.main.orthographicSize)
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnPlatform();
        }
    }

        // Spawn walls when the player is near the next wall spawn position
        if (player.position.y >= nextWallSpawnY - Camera.main.orthographicSize - 3)
        {


                Debug.Log("player" + player.position.y + "  next wall " + nextWallSpawnY + "  camera   " + Camera.main.orthographicSize);
                SpawnWallSegments();
        }

        RecyclePlatforms();
        RecycleWalls(); // Ensure walls are also recycled in the update loop.
    }

    void PreGeneratePlatforms()
    {
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform();
            SpawnWallSegments();
        }
    }

    void SpawnPlatform()
    {
        if (currentPlatformCount >= maxPlatforms)
        {
            return;
        }

        float x;
        const float wallOffset = 3.78f;

        x = Random.Range(wallLeftX - wallOffset, wallRightX + wallOffset);
        lastXPosition = x;
        Vector3 spawnPosition;
        if (player.position.y >= centerY + 10f) // Example condition, you can adjust the threshold as needed
        {
            spawnPosition = new Vector3(x, player.position.y + Camera.main.orthographicSize + platformDistance, 0f);
        }
        else
        {
            spawnPosition = new Vector3(x, nextPlatformSpawnY, 0f);
        }

        GameObject platform = pool.GetPooledObject("Platform");
        if (platform != null)
        {
            platform.transform.position = spawnPosition;
            platform.SetActive(true);
            Debug.Log("Platform spawned at: " + spawnPosition);

            nextPlatformSpawnY += platformDistance;
            currentPlatformCount++;
            Debug.Log("Next platform Y after increment: " + nextPlatformSpawnY + " (Count: " + currentPlatformCount + ")");
        }
        else
        {
            Debug.LogWarning("No inactive Platform objects available in the pool.");
            return;
        }
    }

    void SpawnWallSegments()
    {

        if (pool == null)
        {
            Debug.LogError("ObjectPool component not assigned.");
            return;
        }

        Vector3 leftWallPosition = new Vector3(wallLeftX, nextWallSpawnY, 3.5f);
        GameObject leftWall = pool.GetPooledObject("LeftWall");
        if (leftWall != null)
        {
            leftWall.transform.position = leftWallPosition;
            leftWall.SetActive(true);
            //nextWallSpawnY += wallSegmentHeight / 2;
            Debug.LogWarning("left checking walls  " + nextWallSpawnY);
        }
        else
        {
            Debug.LogWarning("No inactive LeftWall objects available in the pool.");
        }

        Vector3 rightWallPosition = new Vector3(wallRightX, nextWallSpawnY, 3.5f);
        GameObject rightWall = pool.GetPooledObject("RightWall");
        if (rightWall != null)
        {
            rightWall.transform.position = rightWallPosition;
            rightWall.SetActive(true);
            nextWallSpawnY += (wallSegmentHeight) ;
            Debug.LogWarning("right  " + nextWallSpawnY);
        }
        else
        {
            Debug.LogWarning("No inactive RightWall objects available in the pool.");
        }


    }

    void RecyclePlatforms()
    {
        foreach (GameObject platform in pool.GetPooledObjects("Platform"))
        {
            if (platform.activeInHierarchy && platform.transform.position.y < player.position.y - Camera.main.orthographicSize * 2)
            {
                platform.SetActive(false);
                currentPlatformCount--;
            }
        }
    }

    void RecycleWalls()
    {
        float spawnYAbovePlayer = player.position.y + Camera.main.orthographicSize * 2;


        foreach (GameObject leftWall in pool.GetPooledObjects("LeftWall"))
        {
            Debug.Log("Left wall recycled and spawned at: " + spawnYAbovePlayer + "  " + Camera.main.orthographicSize + "   " + (player.position.y - Camera.main.orthographicSize * 2) + "  " + leftWall.transform.position.y);

            if (leftWall.activeInHierarchy && leftWall.transform.position.y < player.position.y - Camera.main.orthographicSize -3 )
            {
                leftWall.SetActive(false);
                Vector3 spawnPosition = new Vector3(leftWall.transform.position.x, spawnYAbovePlayer+4, leftWall.transform.position.z);
                leftWall.transform.position = spawnPosition;
                leftWall.SetActive(true);
                Debug.Log("Left wall recycled and spawned at: " + spawnPosition);
            }
        }

        foreach (GameObject rightWall in pool.GetPooledObjects("RightWall"))
        {
            if (rightWall.activeInHierarchy && rightWall.transform.position.y < player.position.y - Camera.main.orthographicSize -3 )
            {
                rightWall.SetActive(false);
                Vector3 spawnPosition = new Vector3(rightWall.transform.position.x, spawnYAbovePlayer+4, rightWall.transform.position.z);
                rightWall.transform.position = spawnPosition;
                rightWall.SetActive(true);
                Debug.Log("Right wall recycled and spawned at: " + spawnPosition);
            }
        }
    }



}
