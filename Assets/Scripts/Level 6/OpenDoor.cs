using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenDoor : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public TextMeshPro doorMessage; // Reference to the TextMeshPro Text element
    public float openAngle = 70f; // Angle to open the door
    public float openSpeed = 2f; // Speed at which the door opens
    public RealitySwitch realitySwitch; // Reference to the RealitySwitch script

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private myControls inputActions;

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
    }

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -openAngle, 0));
    }

    private void Update()
    {
        // Check for interaction input (E key) only if the player is in range and the door is not already open
        if (isPlayerInRange && !isOpen && inputActions.Player.ActionKey.WasPerformedThisFrame())
        {
            TryOpenDoor();
        }

        // Smoothly rotate the door if it is open
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
    }

    private void TryOpenDoor()
    {
        // Check if the player is holding the Key item
        if (Inventory.Instance != null && Inventory.Instance.currentHandItem != null)
        {
            if (Inventory.Instance.currentHandItem.CompareTag("Key"))
            {
                // Open the door
                isOpen = true;
                Debug.Log("Door opened with the key.");
                doorMessage.text = ""; // Clear the message
                isPlayerInRange = false; // Make the trigger detection obsolete
            }
            else
            {
                Debug.Log("You need to hold the key to open the door.");
            }
        }
        else
        {
            Debug.Log("You need to hold the key to open the door.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpen)
        {
            isPlayerInRange = true;
            Debug.Log("Player entered the trigger area.");

            // Check if the player is holding the Key item
            if (Inventory.Instance != null && Inventory.Instance.currentHandItem != null &&
                Inventory.Instance.currentHandItem.CompareTag("Key"))
            {
                doorMessage.text = "Press E to open the door";

                // Disable the "Find the Key" text
                if (realitySwitch != null && realitySwitch.keyText != null)
                {
                    realitySwitch.keyText.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpen)
        {
            isPlayerInRange = false;
            Debug.Log("Player exited the trigger area.");
            doorMessage.text = ""; // Clear the message
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
