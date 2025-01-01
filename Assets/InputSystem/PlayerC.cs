using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

// Use this script for crawling or crouching
public class PlayerC : MonoBehaviour
{
    public GameObject targetObject;

    public float defaultScaleY = 1; // The default Y scale when not crouched
    public float crouchScaleY = 0.5f; // The Y scale when crouched

    private bool isCrouching = false; // Tracks if the player is currently crouching
    private bool isInsideVent = false; // Tracks if the player is inside a vent
    private myControls inputActions; // Reference to Input Actions

    private FirstPersonController firstPersonController; // Reference to the FirstPersonController script
    private float defaultJumpValue;

    private void Awake()
    {
        inputActions = new myControls();
    }

    private void Start()
    {
        firstPersonController = targetObject.GetComponent<FirstPersonController>();
        defaultJumpValue = firstPersonController.JumpHeight;
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();

        // Subscribe to the Crouch action
        inputActions.Player.Crouch.performed += OnCrouchPressed;
    }

    private void OnDisable()
    {
        // Disable Input Action and unsubscribe when the script is disabled
        inputActions.Player.Disable();
        inputActions.Player.Crouch.performed -= OnCrouchPressed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters a vent
        if (other.CompareTag("Vent"))
        {
            isInsideVent = true;
            firstPersonController.JumpHeight = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits a vent
        if (other.CompareTag("Vent"))
        {
            isInsideVent = false;
            firstPersonController.JumpHeight = defaultJumpValue;
        }
    }

    private void OnCrouchPressed(InputAction.CallbackContext context)
    {
        if (!isInsideVent)
        {
            // Allow free crouch/uncrouch outside the vent
            ToggleCrouch();
        }
    }

    private void ToggleCrouch()
    {
        // Toggle crouch status
        isCrouching = !isCrouching;

        // Adjust player scale based on crouch state
        Vector3 newScale = targetObject.transform.localScale;
        newScale.y = isCrouching ? crouchScaleY : defaultScaleY;

        targetObject.transform.localScale = newScale;
    }
}
