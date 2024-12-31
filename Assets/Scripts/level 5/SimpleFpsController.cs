using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFpsController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Look Settings")]
    public float mouseSensitivity = 200f;
    public Transform cameraTransform; // Assign your MainCamera here in Inspector

    [Header("Jump Settings")]
    public float jumpForce = 5f;           // Force magnitude along transform.up
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;          // Floor and ceiling colliders go on these layers

    // Internals
    private Rigidbody rb;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock & hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        LookAround();
        Move();
        Jump();
        // Optionally: FlipOrientation(); // see example below
    }

    /// <summary>
    /// Basic mouse look:
    ///   - Rotate camera up/down (local X rotation)
    ///   - Rotate entire player left/right (y rotation)
    /// </summary>
    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical look (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal turn (player body)
        transform.Rotate(Vector3.up, mouseX);
    }


    /// <summary>
    /// WASD movement by directly modifying transform.position (horizontal only).
    /// Rigidbody still handles vertical motion (gravity, jump).
    /// </summary>
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");   // W/S

        // Move relative to where the player is facing
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        direction.Normalize();

        // Move the player horizontally
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Checks for "ground" (floor or ceiling) in local-down direction, then jumps along local up.
    /// </summary>
    private void Jump()
    {
        // Instead of always raycasting downward in world coords,
        // we raycast "behind" our local up => -transform.up.
        bool isGrounded = Physics.Raycast(
            transform.position,
            -transform.up,
            groundCheckDistance,
            groundLayer
        );

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Reset any vertical velocity
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;

            // Add an impulse in the direction of transform.up
            // If you're flipped, transform.up is "down" in world coords, so you'll jump off the ceiling.
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Example method for flipping orientation manually (press G) so transform.up inverts.
    /// (Optional) Uncomment in Update() if you want key-based flipping.
    /// </summary>
    private void FlipOrientation()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Rotate 180 degrees around some axis. 
            // For example, flipping around the Z axis so up is reversed:
            transform.Rotate(0f, 0f, 180f);

            Debug.Log("Flipped orientation! Now up is reversed for local transform.");
        }
    }
}
