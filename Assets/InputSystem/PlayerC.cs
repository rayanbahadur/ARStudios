using UnityEngine;
using UnityEngine.InputSystem;


// Use this script for crawling or crouching
public class PlayerC : MonoBehaviour
{
    public GameObject targetObject;

    public float defaultScaleY = 1; // The default Y scale when not crouched
    public float crouchScaleY = 0.5f; // The Y scale when crouched

    private bool isCrouching = false; // Tracks if the player is currently crouching
    private myControls inputActions; // Reference to Input Actions

    private void Awake()
    {
        // Initialize the input actions
        inputActions = new myControls();
    }

    private void OnEnable()
    {
        // Enable Input Action when the script is enabled
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

    private void OnCrouchPressed(InputAction.CallbackContext context)
    {
        // Toggle crouch state
        ToggleCrouch();
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
