using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hatch : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public TextMeshProUGUI hatchMessage; // Reference to the TextMeshPro Text element
    public JournalSystem journalSystem;

    private void Update()
    {
        // Check for interaction input (E key) only if the player is in range
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryBreakHatch();
        }
    }

    private void TryBreakHatch()
    {
        // Check if the player is holding the Hammer item
        if (Inventory.Instance != null && Inventory.Instance.currentHandItem != null)
        {
            if (Inventory.Instance.currentHandItem.CompareTag("Hammer"))
            {
                // Destroy the hidden hatch
                Destroy(gameObject);
                journalSystem.AddClue("HATCH: 8");
                Debug.Log("Hidden hatch broken with the hammer.");
                hatchMessage.text = ""; // Clear the message

            }
            else
            {
                Debug.Log("You need to hold the hammer to break the hatch.");
            }
        }
        else
        {
            Debug.Log("You need to hold the hammer to break the hatch.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered the trigger area.");

            // Check if the player is holding the Hammer item
            if (Inventory.Instance != null && Inventory.Instance.currentHandItem != null &&
                Inventory.Instance.currentHandItem.CompareTag("Hammer"))
            {
                hatchMessage.text = "Press E to break the hatch";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player exited the trigger area.");
            hatchMessage.text = ""; // Clear the message
        }
    }
}

