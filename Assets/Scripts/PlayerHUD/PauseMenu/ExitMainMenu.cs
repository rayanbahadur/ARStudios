using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitMainMenu : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ExitToMainMenu);
        }
    }

    public void ExitToMainMenu()
    {
        UnityEngine.Debug.Log("ExitToMainMenu called."); // Add debug log to verify method call
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
