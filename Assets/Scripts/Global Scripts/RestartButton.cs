using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(RestartGame);
        }
    }

    void RestartGame()
    {
        UnityEngine.Debug.Log("RestartGame called."); // Add debug log to verify method call
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}