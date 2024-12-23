using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using StarterAssets;
using UnityEngine.UI;
using UnityEngine.Playables;

public class NumpadController : MonoBehaviour
{
    public RandomNumberGenerator numberGenerator;

    [Header("Numpad UI")]
    public GameObject numpadUI;
    public TextMeshProUGUI inputField;

    [Header("Interaction Settings")]
    public GameObject interactionPrompt;
    public TextMeshProUGUI interactionText;
    public Outline outline;

    [Header("Success")]
    [SerializeField] public Item Item;
    [SerializeField] public PlayableDirector UnlockLockbox;

    private FirstPersonController firstPersonController; // Reference to the First Person Controller
    private myControls inputActions;
    private Collider lockboxCollider;
    private AudioSource audioSource;

    private int[] correctNumbers;
    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
        inputActions.Player.Enable();
    }

    void Start()
    {
        firstPersonController = FindFirstObjectByType<FirstPersonController>(); // Find the FirstPersonController in the scene
        correctNumbers = numberGenerator.GetGeneratedNumbers(); // Get the correct numbers from the generator
        audioSource = GetComponent<AudioSource>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !numpadUI.activeSelf && this.isActiveAndEnabled)
        {
            interactionText.text = $"Press 'E' to open lockbox";
            interactionPrompt.SetActive(true);
            outline.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inputActions.Player.ActionKey.triggered)
            {
                if (!numpadUI.activeSelf)
                {
                    ToggleNumpadUI(true);
                }
                else {
                    ToggleNumpadUI(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false);
            outline.enabled = false;
        }
    }

    public void OnNumpadButtonPressed(string number)
    {
        // Append the number to the input field
        inputField.text += number;

        // Check the code after each input
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

    void CheckCode()
    {
        // Check if the input matches the correct numbers
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (inputField.text[i].ToString() != correctNumbers[i].ToString())
            {
                Debug.Log("Incorrect code!");
                ClearInput(); // Clear the input if the code is wrong
                return;
            }
        }

        // If all numbers match, unlock the box
        UnlockBox();
    }

    public void UnlockBox()
    {
        ToggleNumpadUI(false);

        float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 1.0f);

        // Play the unlock sound with the specified volume
        if (audioSource != null)
        {
            audioSource.volume = soundEffectVolume; // Set volume from PlayerPrefs
            audioSource.Play();
        }

        // Disable the numpad UI
        numpadUI.SetActive(false);

        // Show Timeline Cutscene Animation
        if (UnlockLockbox != null)
        {
            UnlockLockbox.Play();

            // Subscribe to the stopped event
            UnlockLockbox.stopped += director =>
            {
                // Actions to perform after the Timeline finishes
                PostTimelineActions();
            };
        }
        else
        {
            // Perform actions immediately if no Timeline is assigned
            PostTimelineActions();
        }
    }

    private void PostTimelineActions()
    {

        // Turn off Lockbox Collider
        lockboxCollider = GetComponent<BoxCollider>();
        if (lockboxCollider != null) { lockboxCollider.enabled = false; }
    }




    private void ToggleNumpadUI(bool state) {
        numpadUI.SetActive(state); // Hide the numpad UI
        firstPersonController.enabled = !state; // Re-enable player movement
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = state; // Hide the cursor
        interactionPrompt.SetActive(!state);
        outline.enabled = !state;

        if (!state)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}