using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages interactions with a poster in the game, including highlighting and hiding the poster when clicked.
/// </summary>
public class HidePoster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Outline outline; // Component to highlight the interactable poster
    [SerializeField] private GameObject poster; // The poster GameObject to be hidden

    // Input action map for player controls
    private myControls inputActions;

    private void Awake()
    {
        // Initialise input actions
        inputActions = new myControls();
    }

    private void Start()
    {
        // Disable the outline at the start
        outline.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            outline.enabled = true; // Enable the highlight
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger zone
        if (other.CompareTag("Player"))
        {
            outline.enabled = false; // Disable the highlight
        }
    }

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the ray hit the poster object
            if (hit.collider.gameObject == poster)
            {
                poster.SetActive(false); // Hide the poster
            }
        }
    }

    private void OnEnable()
    {
        // Enable the input actions and subscribe to the mouse click event
        inputActions.Player.Enable();
        inputActions.Player.LMouseClick.performed += OnMouseClick;
    }

    private void OnDisable()
    {
        // Disable the input actions and unsubscribe from the mouse click event
        inputActions.Player.LMouseClick.performed -= OnMouseClick;
        inputActions.Player.Disable();
    }
}
