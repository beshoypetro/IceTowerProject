using System.Collections.Generic;
using UnityEngine;

public class WallObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class WallConfig
    {
        public GameObject prefab;
        public Vector2 spawnPosition; // X position for left/right walls
    }

    public WallConfig leftWallConfig;
    public WallConfig rightWallConfig;
    public int initialPoolSize = 10;
    public float wallSpacing = 10f;
    public float spawnOffset = 5f;

    private Dictionary<WallConfig, Queue<GameObject>> _wallPools = 
        new Dictionary<WallConfig, Queue<GameObject>>();
    
    private Transform _player;
    private float _nextSpawnY;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializePools();
        _nextSpawnY = _player.position.y;
    }

    void Update()
    {
        if (_player.position.y > _nextSpawnY - spawnOffset)
        {
            SpawnWallPair();
        }
    }

    private void InitializePools()
    {
        // Initialize left wall pool
        _wallPools[leftWallConfig] = new Queue<GameObject>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddToPool(leftWallConfig);
        }

        // Initialize right wall pool
        _wallPools[rightWallConfig] = new Queue<GameObject>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddToPool(rightWallConfig);
        }
    }

    private void AddToPool(WallConfig config)
    {
        GameObject wall = Instantiate(config.prefab);
        wall.SetActive(false);
        wall.GetComponent<WallRecycler>().pool = this;
        _wallPools[config].Enqueue(wall);
    }

    private void SpawnWallPair()
    {
        SpawnWall(leftWallConfig);
        SpawnWall(rightWallConfig);
        _nextSpawnY += wallSpacing;
    }

    private void SpawnWall(WallConfig config)
    {
        Queue<GameObject> pool = _wallPools[config];
        
        if (pool.Count == 0)
        {
            AddToPool(config);
        }

        GameObject wall = pool.Dequeue();
        wall.transform.position = new Vector2(
            config.spawnPosition.x,
            _nextSpawnY
        );
        wall.SetActive(true);
    }

    public void ReturnWallToPool(GameObject wall)
    {
        wall.SetActive(false);
        
        // Find which pool this wall belongs to
        foreach (var kvp in _wallPools)
        {
            if (wall.CompareTag(kvp.Key.prefab.tag))
            {
                kvp.Value.Enqueue(wall);
                return;
            }
        }
        
        // If not found in any pool, destroy it
        Destroy(wall);
    }
}