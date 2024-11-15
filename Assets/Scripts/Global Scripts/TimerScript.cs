using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerScript : MonoBehaviour
{
    public TMP_Text displayTimeText;

    // Constants for timer settings
    private const float START_TIME = 300f; // 5 minutes in seconds
    private const float WARNING_TIME = 150f; // 2.5 minutes in seconds
    private const float DANGER_TIME = 60f; // 1 minute in seconds

    // Constants for colors
    private static readonly Color SAFE_COLOR = Color.green; // Green
    private static readonly Color WARNING_COLOR = new Color(1f, 0.55f, 0f); // Dark Orange
    private static readonly Color DANGER_COLOR = new Color(0.86f, 0.08f, 0.24f); // Crimson
    private static readonly Color FLASH_COLOR = Color.red; // Flashing color

    private float currentTime;
    public UnityEvent gameOverEvent;

    void Start()
    {
        // Start timer as paused since the difficulty hasn't been selected yet
        currentTime = AdjustDifficultySettings();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        // Calculate minutes, seconds, and milliseconds
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 100) % 100);

        // Display the timer text in the format MM:SS:MS
        displayTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

        // Color transitions: SAFE_COLOR -> WARNING_COLOR -> DANGER_COLOR
        if (currentTime > WARNING_TIME) // More than 2.5 minutes
        {
            displayTimeText.color = SAFE_COLOR;
        }
        else if (currentTime > DANGER_TIME) // Between 2.5 minutes and 1 minute
        {
            displayTimeText.color = Color.Lerp(SAFE_COLOR, WARNING_COLOR, (START_TIME - currentTime) / (WARNING_TIME - DANGER_TIME));
        }
        else // Final minute
        {
            // Flash red for urgency
            displayTimeText.color = Color.Lerp(WARNING_COLOR, DANGER_COLOR, (WARNING_TIME - currentTime) / (WARNING_TIME - DANGER_TIME));
            displayTimeText.color = Color.Lerp(FLASH_COLOR, Color.white, Mathf.PingPong(Time.time * 2, 1f));
        }

        // When the time runs out
        if (currentTime <= 0)
        {
            currentTime = 0; // Set to zero to avoid negative values
            gameOverEvent.Invoke(); // Invoke the game over event
        }
    }
    public void StartTimer()
    {
        currentTime = AdjustDifficultySettings();
        Time.timeScale = 1; // Start the game by resuming time
    }

    public void ReduceTime(float amount)
    {
        currentTime = Mathf.Max(currentTime - amount, 0); // Clamp the time to a minimum of 0
    }

    public float AdjustDifficultySettings()
    {
        // Start the timer based on difficulty, but it will remain paused until difficulty is chosen
        string difficulty = PlayerPrefs.GetString("Difficulty");
        float adjustedTimer = START_TIME;

        switch (difficulty)
        {
            case "Easy":
                adjustedTimer *= 1.2f;
                break;
            case "Medium":
                break;
            case "Hard":
                adjustedTimer *= 0.8f;
                break;
        }
        return adjustedTimer;
    }
}
