using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NumpadFinal : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Outline outline;
    [SerializeField] private Item keycard;

    [Header("Open Door")]
    [SerializeField] AudioObject clip;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private GameObject backLight;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip unlock;

    private myControls inputActions;

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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inputActions.Player.LMouseClick.triggered) {
                Debug.Log("Mouse Clicked");
                if (isKeycardInHand)
                {
                    audioSource.clip = unlock;
                    audioSource.Play();
                    backLight.SetActive(false);
                    doorAnimator.SetTrigger("Open");
                    Inventory.Instance.Remove(keycard);
                    Inventory.Instance.SelectSlot(1);
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
    }
}