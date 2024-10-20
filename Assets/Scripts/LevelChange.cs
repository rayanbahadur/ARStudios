using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class LevelChange : MonoBehaviour
{
    public Transform player;
    public GameObject endScreen;

    private void Start()
    {
        // Final Level will have an end screen, set it to inactive
        if (endScreen != null)
        {
            endScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Get the current scene index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Check if there is a next level
            if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                // Load the next level
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                // Show end screen if there are no more levels
                ShowEndScreen();
            }
        }
    }

    private void ShowEndScreen()
    {
        // Activate the end screen UI
        if (endScreen != null)
        {
            player.gameObject.SetActive(false);
            endScreen.SetActive(true);
        }
    }
}
