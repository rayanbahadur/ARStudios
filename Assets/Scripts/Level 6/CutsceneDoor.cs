using System.Collections;
using UnityEngine;

public class CutsceneDoor : MonoBehaviour
{
    public RealitySwitch realitySwitch; // Reference to the RealitySwitch script
    public Transform player;            // Reference to the player transform
    public float painDuration = 20.0f;   // Duration of the pain effect
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

        // Move the player's head violently to simulate pain
        float timeElapsed = 0f;
        while (timeElapsed < painDuration)
        {
            float swayX = Mathf.Sin(Time.time * 20f) * 0.5f;  // Rapid horizontal sway
            float swayY = Mathf.Cos(Time.time * 20f) * 0.5f;  // Rapid vertical sway
            float tiltZ = Mathf.Sin(Time.time * 15f) * 10f;   // Rapid Z-axis tilt

            // Apply sway to player position
            player.localPosition = new Vector3(swayX, swayY, player.localPosition.z);

            // Apply tilt to player rotation
            player.localRotation = Quaternion.Euler(0, 0, tiltZ);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Reset player position and rotation after the pain effect
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;
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
