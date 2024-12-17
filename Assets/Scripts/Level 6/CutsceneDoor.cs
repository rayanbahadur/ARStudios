using System.Collections;
using UnityEngine;

public class CutsceneDoor : MonoBehaviour
{
    public RealitySwitch realitySwitch; // Reference to the RealitySwitch script
    public Transform player;            // Reference to the player transform
    public GameObject fire;             // Reference to the fire GameObject
    public float doorOpenAngle = 90f;   // Angle to open the door
    public float doorOpenDuration = 0.5f; // Duration to open the door

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Force the user into fractured reality
            if (!realitySwitch.isFractured)
            {
                StartCoroutine(ForceFracturedReality());
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
    }
}
