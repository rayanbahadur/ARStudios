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

    [Header("Demon Light Settings")]
    public Transform demonHead; // Origin of the raycast (demon's head)
    public LayerMask obstructionMask; // Layers that can block the demon light
    public bool isDemonLight = false; // Toggle for objects that represent the demon light

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

            // Check if this is a demon light and validate with a raycast
            if (isDemonLight && demonHead != null)
            {
                if (IsPlayerVisible(other.transform))
                {
                    HandleGameOver();
                    Debug.Log("Player was visible to the demon. Game Over triggered.");
                }
                else
                {
                    Debug.Log("Player is obstructed from the demon light. No Game Over.");
                }
            }
            else
            {
                // Handle Game Over for non-demon light triggers
                HandleGameOver();
            }
        }
    }

    // Method to check if the player is visible to the demon
    private bool IsPlayerVisible(Transform playerTransform)
    {
        Vector3 directionToPlayer = playerTransform.position - demonHead.position;
        RaycastHit hit;

        Debug.DrawRay(demonHead.position, directionToPlayer, Color.red, 1.0f); // Draw the ray in the scene view for debugging

        // Perform raycast to check for obstructions
        if (Physics.Raycast(demonHead.position, directionToPlayer, out hit, Mathf.Infinity, obstructionMask))
        {
            Debug.Log("Raycast hit: " + hit.collider.name); // Debug log to see what the raycast hits

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Raycast hit the player directly. Player is visible to the demon.");
                return true; // Player is visible
            }
            else
            {
                Debug.Log("Raycast hit an obstruction: " + hit.collider.name);
                return false; // Player is obstructed
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }

        // Perform a separate raycast specifically for the player
        int playerLayer = LayerMask.NameToLayer("Player"); // Assuming the player is on the Default layer
        LayerMask playerLayerMask = 1 << playerLayer;

        if (Physics.Raycast(demonHead.position, directionToPlayer, out hit, Mathf.Infinity, playerLayerMask))
        {
            Debug.Log("Separate raycast for player hit: " + hit.collider.name); // Debug log to see what the separate raycast hits

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Separate raycast hit the player directly. Player is visible to the demon.");
                return true; // Player is visible
            }
        }

        return false; // Player is not visible
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
