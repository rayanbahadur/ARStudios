using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using StarterAssets;

public class PhoneRiddle : MonoBehaviour
{
    public GameObject riddleUI; 
    public TextMeshProUGUI riddleText; 
    public Button riddleButton1; 
    public Button riddleButton2; 
    public Button riddleButton3;
    public Button riddleButton4;  
    public Button riddleExitButton; 
    public GameObject interactionPrompt; 
    public TextMeshProUGUI interactionText;
    public GameObject ringingText;

    private FirstPersonController firstPersonController; // Reference to the First Person Controller
    private (string riddle, string[] answers, string correctAnswer) currentRiddle;
    private RingingTextAnimation ringingTextAnimation;
    private bool playerInRange; // Track if the player is in range

    private void Start()
    {
        firstPersonController = FindFirstObjectByType<FirstPersonController>(); // Find the FirstPersonController in the scene
        ringingTextAnimation = FindFirstObjectByType<RingingTextAnimation>(); // Find the RingingTextAnimation script in the scene

        riddleUI.SetActive(false); // Hide the riddle UI at the start
        SetupRiddle("I stand between first and last, Divide the even, hold me fast. When pressure builds and pipes align, You'll find the next, just in time. What am I?", 
        new string[] { "1", "5", "9", "4" }, "4");
    }

    private void Update()
    {
        // Check for player input to start the riddle
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !riddleUI.activeSelf) // Check if riddle UI is not active and player is in range
        {
            Debug.Log("Action key pressed PHONE.");
            StartRiddle(); 
            interactionPrompt.SetActive(false);
        }

        // Check if 'ESC' is pressed to exit the riddle UI
        if (Input.GetKeyDown(KeyCode.Escape) && riddleUI.activeSelf)
        {
            ExitRiddle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !riddleUI.activeSelf && this.isActiveAndEnabled) // Ensure the script is active
        {
            playerInRange = true; // Player is in range
            interactionText.text = $"Press 'E' to interact with {gameObject.name}";
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Player is out of range
            interactionPrompt.SetActive(false);
        }
    }

    private void SetupRiddle(string riddle, string[] answers, string correctAnswer)
    {
        currentRiddle = (riddle, answers, correctAnswer);
        riddleText.text = currentRiddle.riddle; // Set the riddle text

        // Update button labels
        riddleButton1.GetComponentInChildren<TextMeshProUGUI>().text = answers[0]; 
        riddleButton2.GetComponentInChildren<TextMeshProUGUI>().text = answers[1];
        riddleButton3.GetComponentInChildren<TextMeshProUGUI>().text = answers[2];
        riddleButton4.GetComponentInChildren<TextMeshProUGUI>().text = answers[3];

        // Set up listeners for each button
        riddleButton1.onClick.AddListener(() => CheckAnswer(answers[0]));
        riddleButton2.onClick.AddListener(() => CheckAnswer(answers[1]));
        riddleButton3.onClick.AddListener(() => CheckAnswer(answers[2]));
        riddleButton4.onClick.AddListener(() => CheckAnswer(answers[3]));
        riddleExitButton.onClick.AddListener(ExitRiddle); // Exit button listener
    }

    public void StartRiddle()
    {
        riddleUI.SetActive(true); 
        ringingText.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null); // Deselect any selected UI element
        firstPersonController.enabled = false; // Disable movement
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }

    private void CheckAnswer(string answer)
    {
        if (answer == currentRiddle.correctAnswer)
        {
            Debug.Log("Correct answer!");
            riddleUI.SetActive(false); // Hide the riddle UI
            ringingText.SetActive(false); // Ensure the ringing text is hidden now that it's solved
            firstPersonController.enabled = true; // Re-enable the FirstPersonController
            this.enabled = false; // Deactivate this script
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
            Cursor.visible = false; // Hide the cursor
        }
        else
        {
            Debug.Log("Wrong answer! Try again.");
            riddleUI.SetActive(false); // Hide the riddle UI
            firstPersonController.enabled = true; // Re-enable the FirstPersonController
            ringingText.SetActive(true);
            ringingTextAnimation.StartCoroutine("TypeText");
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
            Cursor.visible = false; // Hide the cursor
        }
    }

    private void ExitRiddle()
    {
        riddleUI.SetActive(false); 
        Debug.Log("Exited riddle.");
        firstPersonController.enabled = true; // Re-enable the FirstPersonController
        ringingText.SetActive(true);
        ringingTextAnimation.StartCoroutine("TypeText");
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
        Cursor.visible = false; // Hide the cursor
    }
}
