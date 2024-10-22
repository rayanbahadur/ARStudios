using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using UnityEngine.Events;

public class KeypadRiddle : MonoBehaviour
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
    public UnityEvent chickenDinner;
    private TimerScript timerScript;

    private FirstPersonController firstPersonController; // Reference to the First Person Controller
    private (string riddle, string[] answers, string correctAnswer) currentRiddle;
    private myControls inputActions;

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
    }

    private void Start()
    {
        timerScript = FindFirstObjectByType<TimerScript>();
        firstPersonController = FindFirstObjectByType<FirstPersonController>(); // Find the FirstPersonController in the scene
        riddleUI.SetActive(false); // Hide the riddle UI at the start

        // Set up a different riddle for the keypad
        SetupRiddle("What is the combination?", new string[] { "482", "284", "842", "248" }, "284"); // A new riddle
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !riddleUI.activeSelf)
        {
            interactionText.text = $"Press 'E' to interact with {gameObject.name}";
            interactionPrompt.SetActive(true); // Show the interaction prompt
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If 'E' is pressed
            if (inputActions.Player.ActionKey.WasPerformedThisFrame() && !riddleUI.activeSelf)
            {
                StartRiddle();
                interactionPrompt.SetActive(false); // Hide the prompt once interaction starts
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false); // Hide interaction prompt when player leaves
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
        firstPersonController.enabled = false; // Disable player movement
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }

    private void CheckAnswer(string answer)
    {
        if (answer == currentRiddle.correctAnswer)
        {
            Debug.Log("Correct answer!");
            riddleUI.SetActive(false); // Hide the riddle UI
            firstPersonController.enabled = true; // Re-enable player movement
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
            Cursor.visible = false; // Hide the cursor
            chickenDinner.Invoke();
        }
        else
        {
            Debug.Log("Wrong answer! Try again.");
            timerScript.ReduceTime(180f);
            riddleUI.SetActive(false); // Hide the riddle UI
            interactionPrompt.SetActive(true); // Show the prompt again
            firstPersonController.enabled = true; // Re-enable player movement
        }
    }

    private void ExitRiddle()
    {
        riddleUI.SetActive(false);
        firstPersonController.enabled = true; // Re-enable player movement
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
