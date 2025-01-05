using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityFlipper : MonoBehaviour
{
    public float gravityMagnitude = 9.81f; // Strength of gravity
    public float rotationSpeed = 5f;       // Speed of flipping
    public int maxGravityChanges = 10;     // Maximum number of allowed gravity changes
    public float gravityChangeDuration = 5f; // Duration for gravity inversion in seconds

    private Rigidbody rb;
    private bool isUpsideDown = false;
    private float targetZRotation; // Target Z-axis rotation for smooth flipping
    private int remainingChanges; // Counter for remaining gravity changes
    private float gravityChangeStartTime; // Time when the current gravity change started
    private bool isGravityChanged = false; // Tracks if gravity is currently flipped

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable Unity's default gravity
        remainingChanges = maxGravityChanges; // Initialize remaining changes
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && remainingChanges > 0)
        {
            // Toggle gravity direction and start the duration timer
            if (!isGravityChanged)
            {
                ToggleGravityDirection();
                gravityChangeStartTime = Time.time; // Record the start time
                isGravityChanged = true; // Gravity is now flipped
                remainingChanges--;
            }
            else
            {
                // Allow reverting manually before the duration ends
                RevertGravity();
            }
        }

        // Automatically revert gravity after 10 seconds
        if (isGravityChanged && Time.time >= gravityChangeStartTime + gravityChangeDuration)
        {
            RevertGravity();
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

        Debug.Log(isUpsideDown
            ? $"Gravity inverted: Walking on ceiling. Remaining changes: {remainingChanges}"
            : $"Gravity normal: Walking on floor. Remaining changes: {remainingChanges}");
    }

    void RevertGravity()
    {
        isUpsideDown = false; // Reset to normal gravity
        targetZRotation = 0f; // Reset rotation
        isGravityChanged = false; // Allow further changes
        Debug.Log("Gravity reverted to normal.");
    }
}
