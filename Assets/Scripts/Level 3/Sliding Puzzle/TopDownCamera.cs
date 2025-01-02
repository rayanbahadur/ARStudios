using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] GameObject player;          // The player GameObject
    [SerializeField] GameObject topdownCamera;   // The top-down camera GameObject

    [Header("Interaction Settings")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI interactionText; // Text for the interaction prompt

    private myControls inputActions;
    private bool isPlayerInTrigger = false;      // Tracks if the player is in the trigger

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
        inputActions.Player.Enable();
    }

    private void Start()
    {
        player.SetActive(true);
        topdownCamera.SetActive(false);
    }

    private void Update()
    {
        // Check if the player is in the trigger and the action key is pressed
        if (isPlayerInTrigger && inputActions.Player.ActionKey.triggered)
        {
            ToggleCameras();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            interactionText.text = "Press 'E' to Toggle Top-Down Camera";
            interactionPrompt.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            interactionPrompt.SetActive(false);
        }
    }

    private void ToggleCameras()
    {
        // Toggle player and top-down camera
        bool isPlayerActive = player.activeSelf;

        player.SetActive(!isPlayerActive);
        topdownCamera.SetActive(isPlayerActive);
    }
}
