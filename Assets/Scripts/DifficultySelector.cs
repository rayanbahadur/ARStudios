using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public static string selectedDifficulty; // Store the selected difficulty

    public GameManager gameManager; // Reference to the GameManager
    public GameObject Crosshair; // Reference to the Crosshair GameObject

    private void Start()
    {
        // Ensure that difficulty selection happens at the start
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Disable the crosshair at the start
        if (Crosshair != null)
        {
            Crosshair.SetActive(false);
        }
    }

    // Method to set difficulty from UI buttons
    public void SetDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;

        // Notify GameManager that difficulty was selected
        gameManager.OnDifficultySelected();

        // Enable the crosshair after difficulty is selected
        if (Crosshair != null)
        {
            Crosshair.SetActive(true);
        }
    }
}
