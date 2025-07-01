using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;

    public int platformPoolSize = 30;
    public int wallPoolSize = 6; // 3 left + 3 right (or more if needed)

    private List<GameObject> pooledPlatforms = new List<GameObject>();
    private List<GameObject> pooledLeftWalls = new List<GameObject>();
    private List<GameObject> pooledRightWalls = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < platformPoolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            pooledPlatforms.Add(obj);
        }

        for (int i = 0; i < wallPoolSize; i++)
        {
            GameObject left = Instantiate(leftWallPrefab);
            left.SetActive(false);
            pooledLeftWalls.Add(left);

            GameObject right = Instantiate(rightWallPrefab);
            right.SetActive(false);
            pooledRightWalls.Add(right);
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

        return null;
    }

    public List<GameObject> GetPooledObjects(string type)
    {
        if (type == "Platform") return pooledPlatforms;
        else if (type == "LeftWall") return pooledLeftWalls;
        else if (type == "RightWall") return pooledRightWalls;
        return null;
    }
}