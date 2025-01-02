using UnityEngine;

// Ensure the crate can't move diagonally
public class BoxController : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get the Rigidbody's current velocity
        Vector3 velocity = rb.velocity;

        // Allow only left/right or forward/back movement
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z))
        {
            // Keep only X-axis velocity
            velocity.z = 0;
        }
        else
        {
            // Keep only Z-axis velocity
            velocity.x = 0;
        }

        // Apply the clamped velocity back to the Rigidbody
        rb.velocity = velocity;
    }
}
