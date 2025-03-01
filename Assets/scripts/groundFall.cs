using System.Collections;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 2f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // Reset the effector's rotational offset if no keys are pressed
        if (Input.GetAxisRaw("Vertical") >= 0)
        {
            effector.rotationalOffset = 0f;
        }

        // Check if the player presses the "Down" key to drop through the platform
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            effector.rotationalOffset = 180f;
            StartCoroutine(ResetEffector());
        }
    }

    private IEnumerator ResetEffector()
    {
        yield return new WaitForSeconds(waitTime);
        effector.rotationalOffset = 0f;
    }
}
