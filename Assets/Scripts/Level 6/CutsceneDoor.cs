using System.Collections;
using UnityEngine;

public class CutsceneDoor : MonoBehaviour
{
    public RealitySwitch realitySwitch; // Reference to the RealitySwitch script
    public Transform player;            // Reference to the player transform
    public GameObject fire;             // Reference to the fire GameObject
    public float doorOpenAngle = 90f;   // Angle to open the door
    public float doorOpenDuration = 0.5f; // Duration to open the door
    public StaircaseText staircaseText; // Reference to the StaircaseText script
    [SerializeField] private PlayerProgress playerProgress; // Reference to the PlayerProgress script

    private BoxCollider triggerCollider; // Reference to the door's box collider that serves as a trigger
    private bool doorOpened = false; // Flag to track if the door has been opened

    private void Start()
    {
        // Get the door's box collider component
        triggerCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Force the user into fractured reality if not already in it
            if (!realitySwitch.isFractured)
            {
                StartCoroutine(ForceFracturedReality());
            }
            else
            {
                // Open the door if already in fractured reality
                StartCoroutine(OpenDoor());
            }

            // Activate the fire object
            if (fire != null)
            {
                fire.SetActive(true);
                Debug.Log("Fire activated.");
            }
            else
            {
                Debug.LogWarning("Fire reference is missing.");
            }

            // Deactivate the staircase text
            if (staircaseText != null && staircaseText.staircaseMessage != null)
            {
                staircaseText.staircaseMessage.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ForceFracturedReality()
    {
        // Trigger the transition to fractured reality
        realitySwitch.isFractured = true;
        if (realitySwitch.transitionCoroutine != null)
        {
            StopCoroutine(realitySwitch.transitionCoroutine);
        }
        realitySwitch.transitionCoroutine = StartCoroutine(realitySwitch.SmoothTransition(true));

        // Open the door violently
        StartCoroutine(OpenDoor());

        yield return null;
    }

    private IEnumerator OpenDoor()
    {
        // Check if the door has already been opened
        if (doorOpened)
        {
            yield break;
        }

        float timeElapsed = 0f;
        Quaternion initialRotation = transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, doorOpenAngle, 0);

        while (timeElapsed < doorOpenDuration)
        {
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / doorOpenDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRotation;

        // Disable the door's trigger collider to prevent further triggering
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }

        // Set the doorOpened flag to true
        doorOpened = true;

        // Add 30% to the task progress
        if (playerProgress != null)
        {
            playerProgress.SetProgress(50);
            Debug.Log("Task progress set to 50%");
        }
    }
}
