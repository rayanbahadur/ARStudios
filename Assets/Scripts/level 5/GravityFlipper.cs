using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityFlipper : MonoBehaviour
{
    public float gravityMagnitude = 9.81f; // Strength of gravity
    public float rotationSpeed = 5f;       // Speed of flipping
    private Rigidbody rb;
    private bool isUpsideDown;
    private float targetZRotation; // Target Z-axis rotation for smooth flipping

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable Unity's default gravity
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGravityDirection();
        }

        // Smoothly rotate the Z-axis while preserving X and Y axes
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, targetZRotation);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void FixedUpdate()
    {
        // Apply custom gravity
        Vector3 gravityDirection = isUpsideDown ? Vector3.up : Vector3.down;
        rb.AddForce(gravityDirection * gravityMagnitude, ForceMode.Acceleration);
    }

    void ToggleGravityDirection()
    {
        isUpsideDown = !isUpsideDown;

        // Set target Z-axis rotation for flipping
        targetZRotation = isUpsideDown ? 180f : 0f;

        Debug.Log(isUpsideDown ? "Gravity inverted: Walking on ceiling." : "Gravity normal: Walking on floor.");
    }
}
