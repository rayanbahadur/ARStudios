using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using StarterAssets;
using UnityEngine.UI;

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

    private FirstPersonController firstPersonController; // Reference to the First Person Controller
    private myControls inputActions;

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

    void UnlockBox()
    {
        Debug.Log("Lockbox opened!");
        numpadUI.SetActive(false);
        firstPersonController.enabled = true; // Enable movement
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor

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