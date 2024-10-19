using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.Events; // Needed for UnityEvent
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI
    public UnityEvent gameOverEvent; // UnityEvent to invoke on game over

    private TimerScript timerScript;

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

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    }

    public void RestartGame()
    {
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
