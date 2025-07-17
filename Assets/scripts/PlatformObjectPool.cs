using System.Collections.Generic;
using UnityEngine;

public class PlatformObjectPool : MonoBehaviour
{
    // Normal platform prefabs (cycle every 100 platforms)
    public List<GameObject> platformPrefabs;
    
    // Special platform prefabs (change every 100 platforms)
    public List<GameObject> specialPlatformPrefabs;
    
    private Dictionary<GameObject, Queue<GameObject>> _poolDictionary;
    private int _platformSpawnCount = 0;
    private int _currentSpecialIndex = 0;

    void Awake()
    {
        _poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    }

    public GameObject GetPlatform()
    {
        GameObject prefab = GetCurrentPrefab();
        Queue<GameObject> pool;
        
        if (!_poolDictionary.TryGetValue(prefab, out pool))
        {
            pool = new Queue<GameObject>();
            _poolDictionary[prefab] = pool;
        }

        GameObject platform;
        if (pool.Count > 0)
        {
            platform = pool.Dequeue();
        }
        else
        {
            platform = Instantiate(prefab);
            var pooledPlatform = platform.AddComponent<PooledPlatform>();
            pooledPlatform.prefab = prefab;
            pooledPlatform.isSpecial = IsSpecialPlatform(_platformSpawnCount);
        }

        platform.SetActive(true);
        _platformSpawnCount++;
        return platform;
    }

    private GameObject GetCurrentPrefab()
    {
        // Special platforms every 50 platforms
        if (IsSpecialPlatform(_platformSpawnCount))
        {
            // Update special index every 100 platforms
            _currentSpecialIndex = Mathf.FloorToInt(_platformSpawnCount / 100f) % specialPlatformPrefabs.Count;
            
            return specialPlatformPrefabs[_currentSpecialIndex];
        }
        
        // Normal platforms cycle every 100 platforms
        int prefabIndex = (_platformSpawnCount / 100) % platformPrefabs.Count;
        return platformPrefabs[prefabIndex];
    }

    private bool IsSpecialPlatform(int count)
    {
        return count % 50 == 0; // Special at 0, 50, 100, 150, etc.
    }

    public void ReturnPlatform(GameObject platform)
    {
        var pooledInfo = platform.GetComponent<PooledPlatform>();
        if (pooledInfo == null || !_poolDictionary.ContainsKey(pooledInfo.prefab))
        {
            Destroy(platform);
            return;
        }

        platform.SetActive(false);
        _poolDictionary[pooledInfo.prefab].Enqueue(platform);
    }

    public int PlatformCount => _platformSpawnCount;
    public bool IsNextPlatformSpecial => IsSpecialPlatform(_platformSpawnCount);
}

public class PooledPlatform : MonoBehaviour
{
    public GameObject prefab;
    public bool isSpecial;
}