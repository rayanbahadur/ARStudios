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

    private FirstPersonController firstPersonController; // Reference to the First Person Controller
    private myControls inputActions;

    private int[] correctNumbers;
    private Outline outline;
    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
        inputActions.Player.Enable();
    }

    void Start()
    {
        firstPersonController = FindFirstObjectByType<FirstPersonController>(); // Find the FirstPersonController in the scene
        correctNumbers = numberGenerator.GetGeneratedNumbers(); // Get the correct numbers from the generator
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !numpadUI.activeSelf && this.isActiveAndEnabled)
        {
            interactionText.text = $"Press 'E' to interact with {gameObject.name}";
            interactionPrompt.SetActive(true);
            outline.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(inputActions.Player.ActionKey.triggered && !numpadUI.activeSelf)
            {
                numpadUI.SetActive(true);
                interactionPrompt.SetActive(false);
                outline.enabled = false;
                EventSystem.current.SetSelectedGameObject(null); // Deselect any selected UI element
                firstPersonController.enabled = false; // Disable movement
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Show the cursor
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
}
