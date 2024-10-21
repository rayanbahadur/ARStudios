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
    public JournalSystem journalSystem;

    private FirstPersonController firstPersonController; // Reference to the First Person Controller
    private (string riddle, string[] answers, string correctAnswer) currentRiddle;
    private RingingTextAnimation ringingTextAnimation;
    private myControls inputActions; 
    private TimerScript timerScript; 


    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
    }

    private void Start()
    {
        firstPersonController = FindFirstObjectByType<FirstPersonController>(); // Find the FirstPersonController in the scene
        ringingTextAnimation = FindFirstObjectByType<RingingTextAnimation>(); // Find the RingingTextAnimation script in the scene
        timerScript = FindFirstObjectByType<TimerScript>();
        riddleUI.SetActive(false); // Hide the riddle UI at the start
        SetupRiddle("I stand between first and last, Divide the even, hold me fast. When pressure builds and pipes align, You'll find the next, just in time. What am I?", 
        new string[] { "1", "5", "9", "4" }, "4");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !riddleUI.activeSelf && this.isActiveAndEnabled) // Ensure the script is active
        {
            interactionText.text = $"Press 'E' to interact with {gameObject.name}";
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check for action key press using the new input system (same as ATM)
            if (inputActions.Player.ActionKey.WasPerformedThisFrame() && !riddleUI.activeSelf)
            {
                Debug.Log("Action key pressed PHONE.");
                StartRiddle(); 
                interactionPrompt.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            journalSystem.AddClue("PHONE CALL: 4");
        }
        else
        {
            Debug.Log("Wrong answer! Try again.");
            timerScript.ReduceTime(30f);
            riddleUI.SetActive(false); // Hide the riddle UI
            interactionPrompt.SetActive(true);
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

    public void OnEnable()
    {
        inputActions.Player.Enable();
    }

    public void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
