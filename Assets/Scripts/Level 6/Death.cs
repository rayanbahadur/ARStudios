using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [Header("Game Over")]
    public GameObject gameOverUI; // Reference to the Game Over UI
    public GameObject interactionPrompt;
    public GameObject Crosshair; // Reference to the Crosshair GameObject
    public GameObject blackoutScreen; // Reference to the Blackout Screen GameObject
    public GameObject inventory; // Reference to the Inventory GameObject

    private void Start()
    {
        // Ensure the Game Over UI is hidden at the start
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.tag); // Debug log to check trigger entry

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger."); // Debug log to confirm player entry
            HandleGameOver();
            Debug.Log("HandleGameOver called from Death script.");
        }
    }

    // Method to handle game over logic
    private void HandleGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Activate the Game Over UI
            Debug.Log("Game Over UI activated.");
        }
        else
        {
            Debug.LogError("Game Over UI is not assigned.");
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
            Debug.Log("Interaction prompt deactivated.");
        }
        else
        {
            Debug.LogError("Interaction prompt is not assigned.");
        }

        if (blackoutScreen != null)
        {
            blackoutScreen.SetActive(false);
            Debug.Log("Blackout screen deactivated.");
        }
        else
        {
            Debug.LogError("Blackout screen is not assigned.");
        }

        if (inventory != null)
        {
            inventory.SetActive(false);
            Debug.Log("Inventory deactivated.");
        }
        else
        {
            Debug.LogError("Inventory is not assigned.");
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Debug.Log("Cursor unlocked and made visible.");

        // Disable the crosshair
        if (Crosshair != null)
        {
            Crosshair.SetActive(false);
            Debug.Log("Crosshair deactivated.");
        }
        else
        {
            Debug.LogError("Crosshair is not assigned.");
        }
    }
}
