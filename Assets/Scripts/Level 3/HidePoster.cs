using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePoster : MonoBehaviour
{
    [SerializeField] private Outline outline; // Outline for highlighting interactable objects
    [SerializeField] private GameObject poster;

    private myControls inputActions;
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
        if (other.CompareTag("Player")){
            outline.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && inputActions.Player.LMouseClick.triggered)
        {
            poster.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = false;
        }
    }
}
