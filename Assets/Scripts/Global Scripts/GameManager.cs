using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.Events; // Needed for UnityEvent
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Over")]
    public GameObject gameOverUI; // Reference to the Game Over UI
    public UnityEvent gameOverEvent; // UnityEvent to invoke on game over
    [Header("Timer")]
    public TimerScript timerScript;
    [Header("Main Canvas")]
    public GameObject mainCanvas; // Reference to the main canvas
    [Header("Audio Sources")]
    public List<AudioSource> audioSources; // List of audio sources to disable

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
        UnityEngine.Debug.Log("HandleGameOver called."); // Debug log to verify method call

        // Deactivate all items in the main canvas
        if (mainCanvas != null)
        {
            foreach (Transform child in mainCanvas.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        gameOverUI.SetActive(true); // Activate the Game Over UI
        UnityEngine.Debug.Log("GameOverUI activated."); // Debug log to verify activation

        Time.timeScale = 0;

        // Disable all audio sources
        foreach (var audioSource in audioSources)
        {
            if (audioSource != null)
            {
                audioSource.enabled = false;
            }
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    }

    public void RestartGame()
    {
        UnityEngine.Debug.Log("RestartGame called."); // Add debug log to verify method call
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

}
