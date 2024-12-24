using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HidePoster : MonoBehaviour
{
    [SerializeField] private Outline outline; // Outline for highlighting interactable objects
    [SerializeField] private GameObject poster;

    private myControls inputActions;

    private void Awake()
    {
        inputActions = new myControls();
    }

    private void Start()
    {
        outline.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = true; // Highlight the poster
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = false; // Stop highlighting
        }
    }

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        // Create a ray from the camera to the mouse position
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
