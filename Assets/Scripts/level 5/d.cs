using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Door door; // Reference to the Door script
    public string requiredKeyTag = "Key"; // Tag assigned to the key in the Inspector

    private bool isPlayerInside = false; // Track if the player is in the trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player has entered the trigger zone.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is inside the trigger zone.");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed. Checking held item...");

                if (Inventory.Instance.currentHandItem != null)
                {
                    GameObject keyInHand = Inventory.Instance.currentHandItem;
                    Debug.Log(keyInHand.tag);

                    // Check if the held item has the correct tag
                    if (keyInHand.CompareTag(requiredKeyTag))
                    {
                        door.OpenDoor();
                        Debug.Log("Door opened!");
                    }
                    else
                    {
                        Debug.Log("You are not holding the correct key.");
                    }
                }
                else
                {
                    Debug.Log("Your hand is empty. Find the key first.");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player has exited the trigger zone.");
        }
    }
}
