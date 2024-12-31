using UnityEngine;
using UnityEngine.Playables;
using StarterAssets; // Namespace for the FirstPersonController and StarterAssetsInputs

public class StartCutscene : MonoBehaviour
{
    public PlayableDirector director; // Reference to the PlayableDirector
    public GameObject cutsceneCamera; // Reference to the cutscene camera
    public GameObject playerCamera; // Reference to the player's main camera
    public FirstPersonController firstPersonController; // Reference to the FirstPersonController script

    private void Start()
    {
        if (director != null)
        {
            // Subscribe to PlayableDirector events
            director.stopped += OnCutsceneStopped;
            Debug.Log("Subscribed to PlayableDirector stopped event.");

            // Start the cutscene
            director.Play();
            Debug.Log("Cutscene started.");

            // Disable player movement and switch to cutscene camera
            DisablePlayerMovement();
            SwitchToCutsceneCamera();
        }
        else
        {
            Debug.LogWarning("PlayableDirector reference is missing.");
        }
    }

    private void OnCutsceneStopped(PlayableDirector _)
    {
        Debug.Log("Cutscene stopped.");

        // Enable player movement and switch back to player camera
        EnablePlayerMovement();
        SwitchToPlayerCamera();
    }

    private void DisablePlayerMovement()
    {
        Debug.Log("DisablePlayerMovement called.");
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false; // Disable the FirstPersonController script
            Debug.Log("FirstPersonController disabled during cutscene.");
        }
        else
        {
            Debug.LogWarning("FirstPersonController reference is missing.");
        }
    }

    private void EnablePlayerMovement()
    {
        Debug.Log("EnablePlayerMovement called.");
        if (firstPersonController != null)
        {
            firstPersonController.enabled = true; // Re-enable the FirstPersonController script
            Debug.Log("FirstPersonController enabled after cutscene.");
        }
        else
        {
            Debug.LogWarning("FirstPersonController reference is missing.");
        }
    }

    private void SwitchToCutsceneCamera()
    {
        if (cutsceneCamera != null && playerCamera != null)
        {
            cutsceneCamera.SetActive(true);
            playerCamera.SetActive(false);
            Debug.Log("Switched to cutscene camera.");
        }
        else
        {
            Debug.LogWarning("Cutscene camera or player camera reference is missing.");
        }
    }

    private void SwitchToPlayerCamera()
    {
        if (cutsceneCamera != null && playerCamera != null)
        {
            cutsceneCamera.SetActive(false);
            playerCamera.SetActive(true);
            Debug.Log("Switched to player camera.");
        }
        else
        {
            Debug.LogWarning("Cutscene camera or player camera reference is missing.");
        }
    }
}
