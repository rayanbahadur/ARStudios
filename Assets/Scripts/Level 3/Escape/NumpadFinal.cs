using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class NumpadFinal : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Outline outline;
    [SerializeField] private Item keycard;
    [SerializeField] private GameObject interactionPrompt; // Prompt displayed to the player for interaction
    [SerializeField] private TextMeshProUGUI interactionText; // Text of the interaction prompt
    [SerializeField] private PlayerProgress playerProgress;

    [Header("Open Door")]
    [SerializeField] AudioObject clip;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private GameObject backLight;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip unlock;

    private myControls inputActions;
    private bool inRange = false;
    private bool hasProgressBeenAdded = false;

    private bool isKeycardInHand =>
        Inventory.Instance != null &&
        Inventory.Instance.currentHandItem != null &&
        Inventory.Instance.currentHandItem.CompareTag("Keycard"); // Checks if the player has a keycard equipped

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
        inputActions.Player.Enable();
    }
    
    private void Start()
    {
        outline.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        outline.enabled = true;
        inRange = true;
        interactionText.text = "Press 'E' to Interact with Number Pad";
        interactionPrompt.SetActive(true);
    }

    private void Update()
    {
        if (inRange)
        {
            if (inputActions.Player.ActionKey.triggered) {
                if (!hasProgressBeenAdded)
                {
                    playerProgress.AddProgress(60);
                }
                if (isKeycardInHand)
                {
                    audioSource.clip = unlock;
                    audioSource.Play();
                    backLight.SetActive(false);
                    doorAnimator.SetTrigger("Open");
                    Inventory.Instance.Remove(keycard);
                    Inventory.Instance.SelectSlot(1);
                    interactionPrompt.SetActive(false);
                    if (!hasProgressBeenAdded)
                    {
                        playerProgress.AddProgress(40);
                    }
                    this.enabled = false;
                }
                else
                {
                    Vocals.instance.Speak(clip);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        outline.enabled = false;
        inRange = false;
        interactionPrompt.SetActive(false);
    }
}