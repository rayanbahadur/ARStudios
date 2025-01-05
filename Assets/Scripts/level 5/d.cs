using UnityEngine;
using TMPro;

public class DoorInteraction : MonoBehaviour
{
    public Door door; // Reference to the Door script
    public TextMeshProUGUI interactionPrompt; // Reference to the Interaction Prompt UI element
    public string requiredKeyTag = "Key"; // Tag assigned to the key in the Inspector
    public string interactionMessage = "Press 'E' to unlock the door"; // Default message
    public string findKeyMessage = "Find a key to unlock the door"; // Message if no key is in hand

    private bool isPlayerInside = false; // Track if the player is in the trigger
    private bool doorOpened = false; // Track if the door has been opened
    private PlayerProgress playerProgress; // Reference to the PlayerProgress script

    private void Start()
    {
        // Find the PlayerProgress component in the scene
        playerProgress = FindObjectOfType<PlayerProgress>();
        if (playerProgress == null)
        {
            Debug.LogError("PlayerProgress component not found in the scene!");
        }

        // Ensure the prompt is hidden initially
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Interaction Prompt not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !doorOpened)
        {
            isPlayerInside = true;
            //Debug.Log("Player has entered the trigger zone.");

            // Show the interaction prompt
            if (interactionPrompt != null)
            {
                //Debug.Log("Displaying interaction prompt.");
                interactionPrompt.text = interactionMessage;
                interactionPrompt.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !doorOpened)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed. Checking held item...");

                if (Inventory.Instance.currentHandItem != null)
                {
                    GameObject keyInHand = Inventory.Instance.currentHandItem;

                    // Check if the held item has the correct tag
                    if (keyInHand.CompareTag(requiredKeyTag))
                    {
                        door.OpenDoor();
                        Debug.Log("Door opened!");

                        // Set progress to 50 if the PlayerProgress component is found
                        if (playerProgress != null && playerProgress.currentProgress < 50)
                        {
                            playerProgress.SetProgress(50);
                            playerProgress.SetTaskText("Door Unlocked. Next: Defeat the zombie!");
                            Debug.Log("Progress set to 50.");
                        }

                        // Hide the prompt after opening the door
                        if (interactionPrompt != null)
                        {
                            interactionPrompt.gameObject.SetActive(false);
                        }

                        doorOpened = true; // Mark the door as opened
                    }
                    else
                    {
                        Debug.Log("You are not holding the correct key.");
                        if (interactionPrompt != null)
                        {
                            interactionPrompt.text = "You need the correct key.";
                        }
                    }
                }
                else
                {
                    Debug.Log("Your hand is empty. Find the key first.");
                    if (interactionPrompt != null)
                    {
                        interactionPrompt.text = findKeyMessage;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player has exited the trigger zone.");

            // Hide the interaction prompt unless the door is already opened
            if (interactionPrompt != null && !doorOpened)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }
}
