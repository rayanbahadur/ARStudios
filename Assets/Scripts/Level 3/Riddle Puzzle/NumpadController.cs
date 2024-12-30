using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using StarterAssets;
using UnityEngine.UI;
using UnityEngine.Playables;

/// <summary>
/// Handles interactions with a numpad UI for unlocking a lockbox in the game. 
/// Includes input validation, UI management, and unlocking mechanics.
/// </summary>
public class NumpadController : MonoBehaviour
{
    [Header("Numpad UI")]
    [SerializeField] private GameObject numpadUI; // The UI panel for the numpad
    [SerializeField] private TextMeshProUGUI inputField; // Text field for displaying user input

    [Header("Interaction Settings")]
    [SerializeField] private GameObject interactionPrompt; // Prompt displayed to the player for interaction
    [SerializeField] private TextMeshProUGUI interactionText; // Text of the interaction prompt
    [SerializeField] private Outline outline; // Outline effect for highlighting interactable objects

    [Header("Success")]
    [SerializeField] private Item item; // Item awarded upon success
    [SerializeField] private PlayableDirector unlockLockbox; // Timeline director for the unlock cutscene

    [Header("Dependencies")]
    [SerializeField] private RandomNumberGenerator numberGenerator; // Script to generate the correct number sequence

    private FirstPersonController firstPersonController; // Reference to the First Person Controller
    private myControls inputActions; // Input action map
    private Collider lockboxCollider; // Collider for the lockbox
    private AudioSource audioSource; // Audio source for sound effects

    private int[] correctNumbers; // Array holding the correct code sequence

    private void Awake()
    {
        inputActions = new myControls(); // Initialise input actions
        inputActions.Player.Enable();
    }

    private void Start()
    {
        firstPersonController = FindFirstObjectByType<FirstPersonController>(); // Find the FirstPersonController in the scene
        correctNumbers = numberGenerator.GetGeneratedNumbers(); // Get the correct numbers from the generator
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Display interaction prompt when the player enters the trigger zone
        if (other.CompareTag("Player") && !numpadUI.activeSelf && this.isActiveAndEnabled)
        {
            interactionText.text = "Press 'E' to open lockbox";
            interactionPrompt.SetActive(true);
            outline.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Toggle the numpad UI when the player presses the action key
        if (other.CompareTag("Player"))
        {
            if (inputActions.Player.ActionKey.triggered)
            {
                ToggleNumpadUI(!numpadUI.activeSelf);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide interaction prompt when the player exits the trigger zone
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false);
            outline.enabled = false;
        }
    }

    public void OnNumpadButtonPressed(string number)
    {
        // Append the pressed number to the input field
        inputField.text += number;

        // Check the code once input length matches the correct code length
        if (inputField.text.Length == correctNumbers.Length)
        {
            CheckCode();
        }
    }

    public void ClearInput()
    {
        // Clear the input field
        inputField.text = "";
    }

    private void CheckCode()
    {
        // Verify if the input matches the correct code
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (inputField.text[i].ToString() != correctNumbers[i].ToString())
            {
                Debug.Log("Incorrect code!");
                ClearInput(); // Clear the input if the code is wrong
                return;
            }
        }

        // Unlock the box if the code is correct
        UnlockBox();
    }

    private void UnlockBox()
    {
        ToggleNumpadUI(false);

        float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 1.0f);

        // Play the unlock sound
        if (audioSource != null)
        {
            audioSource.volume = soundEffectVolume;
            audioSource.Play();
        }

        numpadUI.SetActive(false);

        if (unlockLockbox != null)
        {
            unlockLockbox.Play();
            unlockLockbox.stopped += director => PostTimelineActions(); // Actions after the Timeline ends
        }
        else
        {
            PostTimelineActions(); // Immediate actions if no Timeline is assigned
        }
    }

    private void PostTimelineActions()
    {
        // Disable the lockbox collider and hide interaction prompts
        lockboxCollider = GetComponent<BoxCollider>();
        if (lockboxCollider != null) lockboxCollider.enabled = false;

        interactionPrompt.SetActive(false);
        outline.enabled = false;
    }

    private void ToggleNumpadUI(bool state)
    {
        // Toggle the visibility of the numpad UI and adjust player controls
        numpadUI.SetActive(state);
        firstPersonController.enabled = !state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;

        interactionPrompt.SetActive(!state);
        outline.enabled = !state;

        if (!state)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
