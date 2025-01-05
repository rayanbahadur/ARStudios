using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerC : MonoBehaviour
{
    public GameObject targetObject;
    public float defaultScaleY = 1;
    public float crouchScaleY = 0.5f;
    public LayerMask obstacleLayer; // Layer mask for objects to check collision with
    public float raycastOffset = 0.1f; // Small offset to prevent self-collision

    private bool isCrouching = false;
    private bool isInsideVent = false;
    private myControls inputActions;
    private FirstPersonController firstPersonController;
    private float defaultJumpValue;
    private CharacterController characterController;

    private void Awake()
    {
        inputActions = new myControls();
    }

    private void Start()
    {
        firstPersonController = targetObject.GetComponent<FirstPersonController>();
        characterController = targetObject.GetComponent<CharacterController>();
        defaultJumpValue = firstPersonController.JumpHeight;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Crouch.performed += OnCrouchPressed;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Crouch.performed -= OnCrouchPressed;
    }

    private bool CanStandUp()
    {
        // Calculate the height difference between standing and crouching
        float heightDifference = (defaultScaleY - crouchScaleY) * characterController.height;

        // Starting position for the raycast (current position plus a small offset)
        Vector3 rayStart = transform.position + Vector3.up * (characterController.height * crouchScaleY + raycastOffset);

        // Check if there's enough space above the player
        bool hitSomething = Physics.Raycast(rayStart, Vector3.up, heightDifference, obstacleLayer);

        // For debugging - shows the raycast in the scene view
        Debug.DrawRay(rayStart, Vector3.up * heightDifference, hitSomething ? Color.red : Color.green, 0.1f);

        return !hitSomething;
    }

    private void OnCrouchPressed(InputAction.CallbackContext context)
    {
        if (!isInsideVent)
        {
            if (isCrouching)
            {
                // Only allow standing up if there's enough space
                if (CanStandUp())
                {
                    ToggleCrouch();
                }
                else
                {
                    Debug.Log("Can't stand up - obstacle above!");
                }
            }
            else
            {
                // Can always crouch down
                ToggleCrouch();
            }
        }
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        Vector3 newScale = targetObject.transform.localScale;
        newScale.y = isCrouching ? crouchScaleY : defaultScaleY;
        targetObject.transform.localScale = newScale;

        firstPersonController.JumpHeight = isCrouching ? 0 : defaultJumpValue;
    }

    // OnTriggerEnter and OnTriggerExit remain the same
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vent"))
        {
            isInsideVent = true;
            firstPersonController.JumpHeight = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vent"))
        {
            isInsideVent = false;
            firstPersonController.JumpHeight = defaultJumpValue;
        }
    }
}