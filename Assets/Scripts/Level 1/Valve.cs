using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro; 

public class Valve : MonoBehaviour
{
    public GameObject atmScreen;
    public GameObject interactionPrompt; 
    public TextMeshProUGUI interactionText;
    myControls inputActions;

    public UnityEvent myAction;

    private void Awake()
    {
        inputActions = new myControls();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionText.text = $"Press 'E' to interact with {gameObject.name}"; 
            interactionPrompt.SetActive(true); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false); 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (inputActions.Player.ActionKey.WasPerformedThisFrame())
            {
                Debug.Log("Action key pressed!");
                myAction.Invoke();
            }
        }
    }

    public void OnEnable()
    {
        inputActions.Player.Enable();
    }

    public void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
