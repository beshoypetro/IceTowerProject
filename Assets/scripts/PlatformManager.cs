using System.Collections.Generic;
using UnityEngine;


public class PlatformManager : MonoBehaviour
{
    public PlatformObjectPool pool;
    public Transform player;
    public float spawnYInterval = 2f;
    public float recycleOffset = -10f;
    public float specialPlatformWidth = 4f; // Width for special platforms

    private float _nextSpawnY;
    private readonly List<GameObject> _activePlatforms = new List<GameObject>();

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        _nextSpawnY = player.position.y;
        SpawnInitialPlatforms();
    }

    void Update()
    {
        if (player == null) return;
        
        if (player.position.y > _nextSpawnY - 10f)
        {
            SpawnPlatform();
        }
        
        RecyclePlatforms();
    }

    private void SpawnInitialPlatforms()
    {
        // Spawn starting platform (special)
        SpawnPlatform(true);
        
        // Spawn 9 normal platforms
        for (int i = 0; i < 9; i++)
        {
            SpawnPlatform();
        }
    }

    private void SpawnPlatform(bool forceSpecial = false)
    {
        GameObject platform = pool.GetPlatform();
        PooledPlatform pooled = platform.GetComponent<PooledPlatform>();
        
        float xPos;
        float width = 1f; // Default width
        
        // Position special platforms in center with extra width
        if (forceSpecial || (pooled != null && pooled.isSpecial))
        {
            xPos = 0f;
            width = specialPlatformWidth;
        }
        else
        {
            xPos = Random.Range(-2f, 2f);
        }

        platform.transform.position = new Vector2(xPos, _nextSpawnY);
        ScalePlatform(platform, width);
        
        _activePlatforms.Add(platform);
        _nextSpawnY += spawnYInterval;
    }

    private void ScalePlatform(GameObject platform, float width)
    {
        var collider = platform.GetComponent<BoxCollider2D>();
        var renderer = platform.GetComponent<SpriteRenderer>();
        
        if (collider != null)
        {
            collider.size = new Vector2(width, collider.size.y);
        }
        
        if (renderer != null)
        {
            renderer.size = new Vector2(width, renderer.size.y);
        }
    }

    private void RecyclePlatforms()
    {
        for (int i = _activePlatforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = _activePlatforms[i];
            if (platform.transform.position.y < player.position.y + recycleOffset)
            {
                pool.ReturnPlatform(platform);
                _activePlatforms.RemoveAt(i);
            }
        }
    }
}