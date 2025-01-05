using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

/// <summary>
/// Handles the interaction with a vent, including checking conditions (poster removed, screwdriver equipped),
/// playing animations, and displaying messages.
/// </summary>
public class OpenVent : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayableDirector unlockLockbox; // Playable Director for the vent animation
    [SerializeField] private GameObject poster; // The poster that must be deactivated to interact with the vent
    [SerializeField] private TMP_Text ventMessage; // Text message displayed after opening the vent
    [SerializeField] private Item screwdriver;

    [Header("Interaction Settings")]
    [SerializeField] private GameObject interactionPrompt; // Prompt shown when player interacts with the vent
    [SerializeField] private TextMeshProUGUI interactionText; // Text for the interaction prompt
    [SerializeField] private Outline outline; // Outline to highlight the vent when interactable

    private FirstPersonController firstPersonController; // Reference to the first-person controller
    private myControls inputActions; // Input actions for player interaction
    private Collider ventCollider; // Collider for the vent

    private bool isPosterDeactivated => !poster.activeInHierarchy; // Checks if the poster is removed
    private bool isScrewdriverInHand =>
        Inventory.Instance != null &&
        Inventory.Instance.currentHandItem != null &&
        Inventory.Instance.currentHandItem.CompareTag("Screwdriver"); // Checks if the player has a screwdriver equipped

    private bool inTrigger = false;

    private void Awake()
    {
        inputActions = new myControls(); // Initialise input actions
        inputActions.Player.Enable();
    }

    private void Start()
    {
        ventMessage.gameObject.SetActive(false); // Ensure vent message is hidden at the start
        outline.enabled = false; // Disable outline at the start
    }

    private void Update()
    {
        
        // Check conditions to open the vent and perform the action
        if (isPosterDeactivated && isScrewdriverInHand && inputActions.Player.ActionKey.triggered)
        {
            OpenVentAction();
        }
    }

    private void OpenVentAction()
    {
        // Play the unlock animation
        if (unlockLockbox != null)
        {
            unlockLockbox.Play();
        }

        // Display the vent message
        ventMessage.gameObject.SetActive(true);

        Inventory.Instance.Remove(screwdriver);
        Inventory.Instance.SelectSlot(1);

        interactionPrompt.SetActive(false);
        outline.enabled = false;

        // Disable this script to prevent further interactions
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPosterDeactivated)
            {
                outline.enabled = true; // Highlight the vent

                if (!isScrewdriverInHand)
                {
                    // Show interaction prompt if the screwdriver is not equipped
                    interactionText.text = "Screwed Shut, Need to Unscrew";
                    interactionPrompt.SetActive(true);
                }
                else if (isScrewdriverInHand)
                {
                    interactionText.text = "Press 'E' to Open Vent";
                    interactionPrompt.SetActive(true);
                }
            }
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide interaction prompt when the player exits the trigger zone
            interactionPrompt.SetActive(false);
            inTrigger = false;
        }
    }
}
