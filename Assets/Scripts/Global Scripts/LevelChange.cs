using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class LevelChange : MonoBehaviour
{
    public Transform player;
    public static LevelChange Instance;

    void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {   
            loadNextLevel();
        }
    }

    public void loadNextLevel()
    {
        // Get the current scene index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            // Check if there is a next level
            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                PlayerPrefs.SetString("SavedLevel", currentSceneIndex.ToString());
                // Load the next level
                SceneManager.LoadScene(currentSceneIndex);
            }
    }
}
