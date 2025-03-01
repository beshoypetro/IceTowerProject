using UnityEngine;

public class jump : MonoBehaviour
{

    public Rigidbody2D MyRigidbody2D;
    public float upStrength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //MyRigidbody2D.angularVelocity =
            MyRigidbody2D.linearVelocity = Vector2.up * upStrength;
        }
    }
}
