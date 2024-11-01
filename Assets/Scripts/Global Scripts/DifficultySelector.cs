using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{

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

}
