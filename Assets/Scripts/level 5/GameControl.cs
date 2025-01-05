using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject gameControlUI;

    private myControls inputActions;
    private SimpleFpsController fpsController; // Updated to SimpleFpsController
    private bool wasControllerEnabled;

    private void Awake()
    {
        inputActions = new myControls(); // Initialise input actions
        inputActions.Player.Enable();
    }

    private void Start()
    {
        // Find SimpleFpsController in the scene
        fpsController = FindObjectOfType<SimpleFpsController>();
        if (fpsController == null)
        {
            Debug.LogError("SimpleFpsController not found in the scene! Make sure it's attached to the Player GameObject.");
        }

        // Check if UI objects are assigned in the Inspector
        if (inventory == null || crosshair == null || gameControlUI == null)
        {
            Debug.LogError("One or more UI objects (inventory, crosshair, gameControlUI) are not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        if (inputActions.Player.Escape.triggered)
        {
            // Ensure gameControlUI is assigned before toggling
            if (gameControlUI != null)
            {
                ToggleShowUI(!gameControlUI.activeSelf);
            }
            else
            {
                Debug.LogError("gameControlUI is not assigned in the Inspector.");
            }
        }
    }

    private void ToggleShowUI(bool activeState)
    {
        // Ensure fpsController and UI objects are valid
        if (fpsController == null)
        {
            Debug.LogError("Cannot toggle UI because fpsController is not assigned.");
            return;
        }

        if (inventory == null || crosshair == null || gameControlUI == null)
        {
            Debug.LogError("Cannot toggle UI because one or more UI objects are not assigned.");
            return;
        }

        if (activeState) // When showing the UI
        {
            // Store the current state of the SimpleFpsController
            wasControllerEnabled = fpsController.enabled;
        }

        // Toggle UI and FPS controller state
        gameControlUI.SetActive(activeState);
        inventory.SetActive(!activeState);
        fpsController.enabled = !activeState;

        // Show or hide the cursor
        Cursor.visible = activeState;
        Cursor.lockState = activeState ? CursorLockMode.None : CursorLockMode.Locked;

        // Disable the crosshair when UI is active
        crosshair?.SetActive(!activeState);
    }
}
