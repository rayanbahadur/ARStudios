using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

public class FinalPuzzleTrigger : MonoBehaviour
{
    public GameObject portcullis; // Reference to the portcullis game object
    public GameObject demon;      // Reference to the demon game object
    public float loweringDistance = 5f; // Distance to lower the portcullis
    public float loweringSpeed = 2f;    // Speed at which the portcullis lowers
    public Camera cutsceneCamera; // Reference to the cutscene camera
    public Camera playerCamera;   // Reference to the player's camera
    public GameObject playerCapsule; // Reference to the player capsule object
    public TextMeshProUGUI promptText; // Reference to the UI Text element for the prompt
    [SerializeField] private PlayerProgress playerProgress; // Reference to the PlayerProgress script

    private bool isLowering = false;
    private bool hasBeenTriggered = false; // Flag to track if the trigger has been activated
    private Vector3 targetPosition;
    private PlayableDirector cutsceneDirector;
    private FirstPersonController firstPersonController;

    private void Start()
    {
        if (demon != null)
        {
            demon.SetActive(false); // Ensure the demon is inactive at the start
        }

        if (cutsceneCamera != null)
        {
            cutsceneCamera.enabled = false; // Ensure the cutscene camera is disabled at the start
            cutsceneDirector = cutsceneCamera.GetComponent<PlayableDirector>(); // Get the PlayableDirector from the cutscene camera
        }

        firstPersonController = FindFirstObjectByType<FirstPersonController>(); // Find the FirstPersonController in the scene

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false); // Ensure the prompt text is inactive at the start
        }
    }

    private void Update()
    {
        if (isLowering && portcullis != null)
        {
            portcullis.transform.position = Vector3.MoveTowards(portcullis.transform.position, targetPosition, loweringSpeed * Time.deltaTime);
            if (portcullis.transform.position == targetPosition)
            {
                isLowering = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true; // Set the flag to prevent re-triggering

            if (portcullis != null)
            {
                // Calculate the target position for the portcullis
                targetPosition = portcullis.transform.position - new Vector3(0, loweringDistance, 0);
                isLowering = true;
            }

            if (demon != null)
            {
                demon.SetActive(true); // Activate the demon
            }

            if (cutsceneCamera != null && playerCamera != null)
            {
                playerCamera.enabled = false; // Disable the player's camera
                cutsceneCamera.enabled = true; // Enable the cutscene camera
            }

            if (cutsceneDirector != null)
            {
                cutsceneDirector.Play(); // Play the cutscene
                cutsceneDirector.stopped += OnCutsceneStopped; // Subscribe to the stopped event
            }

            if (firstPersonController != null)
            {
                firstPersonController.enabled = false; // Disable player movement
            }

            if (playerCapsule != null)
            {
                playerCapsule.SetActive(false); // Disable the player capsule mesh
            }

            if (promptText != null)
            {
                StartCoroutine(DisplayPromptText()); // Display the prompt text
            }

            // Add 10% to the task progress
            if (playerProgress != null)
            {
                playerProgress.SetProgress(60);
            }
        }
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        if (cutsceneCamera != null && playerCamera != null)
        {
            cutsceneCamera.enabled = false; // Disable the cutscene camera
            playerCamera.enabled = true; // Re-enable the player's camera
        }

        if (cutsceneDirector != null)
        {
            cutsceneDirector.stopped -= OnCutsceneStopped; // Unsubscribe from the stopped event
        }

        if (firstPersonController != null)
        {
            firstPersonController.enabled = true; // Re-enable player movement
        }

        if (playerCapsule != null)
        {
            playerCapsule.SetActive(true); // Re-enable the player capsule mesh
        }
    }

    private IEnumerator DisplayPromptText()
    {
        promptText.gameObject.SetActive(true); // Show the prompt text
        yield return new WaitForSeconds(20); // Wait for 20 seconds
        promptText.gameObject.SetActive(false); // Hide the prompt text
    }
}
