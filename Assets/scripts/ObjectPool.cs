using System.Collections.Generic;
using UnityEngine;

public class PlatformIdentity : MonoBehaviour
{
    public int styleIndex;
}

public class ObjectPool : MonoBehaviour
{
    
    private int platformIndex = 0;
        
    public GameObject platformPrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;
    
    // Additional platform styles
    public GameObject platformStyle1Prefab;
    public GameObject platformStyle2Prefab;
    public GameObject platformStyle3Prefab;

    public int platformPoolSize = 30;
    public int wallPoolSize = 6;

    private List<GameObject> pooledPlatforms = new List<GameObject>();
    private List<GameObject> pooledLeftWalls = new List<GameObject>();
    private List<GameObject> pooledRightWalls = new List<GameObject>();
    
    private int platformActivationCount = 0;
    private GameObject[] stylePrefabs;

    void Start()
    {
        // Initialize style prefabs array
        List<GameObject> styles = new List<GameObject>();
        styles.Add(platformPrefab);
        if (platformStyle1Prefab != null) styles.Add(platformStyle1Prefab);
        if (platformStyle2Prefab != null) styles.Add(platformStyle2Prefab);
        if (platformStyle3Prefab != null) styles.Add(platformStyle3Prefab);
        stylePrefabs = styles.ToArray();

        // Create initial platform pool (all default style)
        for (int i = 0; i < platformPoolSize; i++)
        {
            CreateNewPlatform(0);
        }

        // Create wall pools
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

    private GameObject CreateNewPlatform(int styleIndex)
    {
        GameObject obj = Instantiate(stylePrefabs[styleIndex]);
        obj.SetActive(false);
        
        // Add identity component to track style
        PlatformIdentity identity = obj.AddComponent<PlatformIdentity>();
        identity.styleIndex = styleIndex;
        
        pooledPlatforms.Add(obj);
        return obj;
    }

    public GameObject GetPooledPlatform()
    {
        // Determine current style
        int currentStyle = (platformActivationCount / 100) % stylePrefabs.Length;

        // Find matching inactive platform
        foreach (GameObject platform in pooledPlatforms)
        {
            if (!platform.activeInHierarchy)
            {
                PlatformIdentity identity = platform.GetComponent<PlatformIdentity>();
                if (identity.styleIndex == currentStyle)
                {
                    platformActivationCount++;
                    return platform;
                }
            }
        }

        // No available platform found - create new one
        GameObject newPlatform = CreateNewPlatform(currentStyle);
        platformActivationCount++;
        return newPlatform;
    }

    public GameObject GetPooledObject(string type)
    {
        if (type == "Platform")
        {
            return GetPooledPlatform();
        }
        else if (type == "LeftWall")
        {
            foreach (GameObject wall in pooledLeftWalls)
            {
                if (!wall.activeInHierarchy)
                    return wall;
            }
        }
        else if (type == "RightWall")
        {
            foreach (GameObject wall in pooledRightWalls)
            {
                if (!wall.activeInHierarchy)
                    return wall;
            }
        }
        return null;
    }

    // Updated to handle platform requests
    public List<GameObject> GetPooledObjects(string type)
    {
        if (type == "Platform") 
            return pooledPlatforms;
        else if (type == "LeftWall") 
            return pooledLeftWalls;
        else if (type == "RightWall") 
            return pooledRightWalls;
        return null;
    }
}