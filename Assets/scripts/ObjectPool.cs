using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;
    public int poolSize = 10;

    private List<GameObject> pooledPlatforms = new List<GameObject>();
    private List<GameObject> pooledLeftWalls = new List<GameObject>();
    private List<GameObject> pooledRightWalls = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            pooledPlatforms.Add(obj);
        }

        for (int i = 0; i < poolSize/5; i++)
        {
            GameObject obj = Instantiate(leftWallPrefab);
            obj.SetActive(false);
            pooledLeftWalls.Add(obj);
        }

        for (int i = 0; i < poolSize/5; i++)
        {
            GameObject obj = Instantiate(rightWallPrefab);
            obj.SetActive(false);
            pooledRightWalls.Add(obj);
        }
    }

    public GameObject GetPooledObject(string type)
    {
        List<GameObject> pool = null;

        if (type == "Platform") pool = pooledPlatforms;
        else if (type == "LeftWall") pool = pooledLeftWalls;
        else if (type == "RightWall") pool = pooledRightWalls;

        if (pool != null)
        {
            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
        }

        return null; // Return null if no inactive object is available
    }

    // Public method to access pooled objects
    public List<GameObject> GetPooledObjects(string type)
    {
        if (type == "Platform") return pooledPlatforms;
        else if (type == "LeftWall") return pooledLeftWalls;
        else if (type == "RightWall") return pooledRightWalls;
        return null;
    }
}
