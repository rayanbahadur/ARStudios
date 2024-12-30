using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityFlipper : MonoBehaviour
{
    public float gravityMagnitude = 9.81f;
    private Rigidbody rb;
    private bool isUpsideDown;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Disable the built-in gravity, we’ll handle it ourselves.
        rb.useGravity = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGravityDirection();
        }
    }

    void FixedUpdate()
    {
        // Apply custom "gravity"
        Vector3 gravityDir = isUpsideDown ? Vector3.up : Vector3.down;
        rb.AddForce(gravityDir * gravityMagnitude, ForceMode.Acceleration);
    }

    void ToggleGravityDirection()
    {
        isUpsideDown = !isUpsideDown;
        // Flip player visually (rotate 180 degrees on X, or whichever axis your character uses)
        // If your model faces +Z for “forward,” flipping on X might make sense:
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.z = isUpsideDown ? 180f : 0f;  // Some characters flip on z-axis
        transform.eulerAngles = currentEuler;

        Debug.Log(isUpsideDown
            ? "Gravity inverted: Walking on ceiling."
            : "Gravity normal: Walking on floor.");
    }
}
