using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintSystem : MonoBehaviour
{
    [Header("Hint System")]
    public TMP_Text hint;  // Reference to the TextMeshPro UI component
    public string[] hints = new string[3];  // Array of hints
    public float hintInterval = 30f;  // Time in seconds before showing "Press H for a hint"
    public float hintDisplayTime = 5f;  // Time in seconds to display the hint message

    [Header("Max Hints")]
    public int hardHintVal = 1;
    public int mediumHintVal = 3;


    public int currentHintIndex = 0;  // Tracks which hint is currently being displayed
    private float hintTimer;  // Timer to control when the hint message shows
    private bool hintMessageVisible = false;  // Flag to check if the "Press H" message is showing
    private int maxHintsToShow; // Max hints based on difficulty

    private myControls inputActions;

    void Awake()
    {
        inputActions = new myControls();
    }

    void Start()
    {
        inputActions.Enable();

        // Initially hide the hint message
        hint.text = "";

        // Set the number of hints based on the difficulty level
        SetHintsBasedOnDifficulty();

        // Start the timer at the interval duration
        hintTimer = 0f; // Start at 0 to ensure it kicks in after the first frame.
    }

    void Update()
    {
        // Countdown the timer
        hintTimer += Time.deltaTime;

        // Check if it's time to show the "Press H for a hint" message
        if (hintTimer >= hintInterval && !hintMessageVisible)
        {
            ShowHintMessage();
        }

        // If the player presses the 'H' key, show the next hint
        if (inputActions.Player.HintKey.WasPerformedThisFrame())
        {
            ShowHint();
        }
    }

    void SetHintsBasedOnDifficulty()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy"); // Default to "Easy" if not set

        switch (difficulty)
        {
            case "Hard":
                maxHintsToShow = hardHintVal;
                break;
            case "Medium":
                maxHintsToShow = mediumHintVal;
                break;
            case "Easy":
                maxHintsToShow = hints.Length;  // Show all hints
                break;
            default:
                maxHintsToShow = hints.Length;  // Default to showing all hints
                break;
        }
    }


    // Function to show the "Press H for a hint" message
    void ShowHintMessage()
    {
        hint.text = "Press H for a hint";
        hintMessageVisible = true;
        hintTimer = 0f; // Reset the timer to zero to trigger the next 30-second cycle
        StartCoroutine(HideHintMessageAfterTime());  // Hide after a few seconds
    }

    // Coroutine to hide the hint message after a few seconds
    IEnumerator HideHintMessageAfterTime()
    {
        yield return new WaitForSeconds(hintDisplayTime);
        hint.text = "";
        hintMessageVisible = false;
    }

    // Show the next hint in the array
    void ShowHint()
    {
        if (currentHintIndex < hints.Length-1 && currentHintIndex < maxHintsToShow)
        {
            hint.text = hints[currentHintIndex];
            currentHintIndex++;
        }
        else
        {
            hint.text = "No more hints available!";
        }

        // Hide the hint again after the specified display time
        StartCoroutine(HideHintMessageAfterTime());
    }
}