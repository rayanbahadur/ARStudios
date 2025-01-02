using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.Events; // Needed for UnityEvent
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Over")]
    public GameObject gameOverUI; // Reference to the Game Over UI
    public UnityEvent gameOverEvent; // UnityEvent to invoke on game over
    [Header("Interaction")]
    public GameObject interactionPrompt;
    public GameObject Crosshair; // Reference to the Crosshair GameObject
    [Header("Timer")]
    public TimerScript timerScript;
    //[Header("Restart Game")]
    //public CheckpointLoader checkpointLoader;

    private void Start()
    {
        Time.timeScale = 1; // Resume the game
        // Use FindFirstObjectByType to find the TimerScript instance
        timerScript = FindFirstObjectByType<TimerScript>(); // Find the TimerScript in the scene
        gameOverUI.SetActive(false); // Ensure the Game Over UI is hidden at the start

        // Subscribe to the gameOverEvent from TimerScript
        if (timerScript != null)
        {
            timerScript.gameOverEvent.AddListener(HandleGameOver);
        }
    }

    // Method to handle game over logic
    public void HandleGameOver()
    {
        gameOverUI.SetActive(true); // Activate the Game Over UI
        interactionPrompt.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor

        // Disable the crosshair
        if (Crosshair != null)
        {
            Crosshair.SetActive(false);
        }
    }

    public void RestartGame()
    {
        UnityEngine.Debug.Log("RestartGame called."); // Add debug log to verify method call
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void RestartFromCheckpoint()
    //{
    //    if (checkpointLoader != null)
    //    {
    //        checkpointLoader.LoadCheckpoint();
    //    }
    //}
}
