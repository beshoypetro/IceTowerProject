using System.Collections.Generic;
using UnityEngine;

public class WallRecycler : MonoBehaviour
{
    public WallObjectPool pool;
    public float recycleOffset = -15f;

    void Update()
    {
        if (Camera.main == null) return;
        
        float cameraBottomY = Camera.main.transform.position.y - Camera.main.orthographicSize;
        
        if (transform.position.y < cameraBottomY + recycleOffset)
        {
            pool?.ReturnWallToPool(gameObject);
        }
    }
}