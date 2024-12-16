using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI; // For handling UI
using TMPro;

public class RealitySwitch : MonoBehaviour
{
    public GameObject normalReality;       // Parent object for Normal Reality
    public GameObject fracturedReality;   // Parent object for Fractured Reality
    public Volume fracturedPostProcess;   // Post-processing volume for fractured reality
    public Camera playerCamera;           // Player's camera
    public TextMeshProUGUI promptText;               // UI text for the initial prompt
    public TextMeshProUGUI keyText;                  // UI text for the key objective
    public float keyTextDuration = 10f;    // Duration to show the "Find the Key" text
    public float transitionDuration = 1.0f; // Time for the camera animation

    public bool isFractured = false;     // Track current reality state
    private bool hasPressedZ = false;     // Track if the player has toggled fractured reality
    public Coroutine cameraAnimationCoroutine;
    public Coroutine transitionCoroutine;

    private void Start()
    {
        // Ensure the scene starts in Normal Reality
        fracturedPostProcess.weight = 0;    // Disable fractured post-processing
        normalReality.SetActive(true);      // Enable Normal Reality objects
        fracturedReality.SetActive(false); // Disable Fractured Reality objects

        // Show the initial prompt
        if (promptText != null)
        {
            promptText.text = "Press Z to enter the fractured reality!";
            promptText.gameObject.SetActive(true);
        }

        if (keyText != null)
        {
            keyText.gameObject.SetActive(false); // Hide the key text initially
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!hasPressedZ) // First-time toggle
            {
                hasPressedZ = true;

                // Hide the initial prompt
                if (promptText != null)
                {
                    promptText.gameObject.SetActive(false);
                }

                // Show "Find the Key" message for a few seconds
                if (keyText != null)
                {
                    StartCoroutine(ShowKeyText());
                }
            }

            // Toggle the fractured reality
            isFractured = !isFractured;
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
            }
            transitionCoroutine = StartCoroutine(SmoothTransition(isFractured));
        }
    }

    private IEnumerator ShowKeyText()
    {
        if (keyText != null)
        {
            keyText.gameObject.SetActive(true); // Show the "Find the Key" message
            yield return new WaitForSeconds(keyTextDuration);
            keyText.gameObject.SetActive(false); // Hide it after the duration
        }
    }

    public IEnumerator SmoothTransition(bool toFractured)
    {
        float timeElapsed = 0f;
        float startWeight = fracturedPostProcess.weight;
        float endWeight = toFractured ? 1f : 0f;

        // Enable the target reality at the start of the transition
        if (toFractured)
        {
            fracturedReality.SetActive(true);
        }
        else
        {
            normalReality.SetActive(true);
        }

        while (timeElapsed < transitionDuration)
        {
            float t = timeElapsed / transitionDuration;
            fracturedPostProcess.weight = Mathf.Lerp(startWeight, endWeight, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fracturedPostProcess.weight = endWeight;

        // Disable the other reality at the end of the transition
        if (toFractured)
        {
            normalReality.SetActive(false);
        }
        else
        {
            fracturedReality.SetActive(false);
        }

        // Start or stop the camera animation based on the current reality
        if (toFractured)
        {
            cameraAnimationCoroutine = StartCoroutine(AnimateCamera());
        }
        else if (cameraAnimationCoroutine != null)
        {
            StopCoroutine(cameraAnimationCoroutine);
            ResetCamera();
        }
    }

    private IEnumerator AnimateCamera()
    {
        while (true)
        {
            // Simulate a swaying motion by adjusting position and rotation
            float swayX = Mathf.Sin(Time.time * 5f) * 0.5f;  // Horizontal sway
            float swayY = Mathf.Cos(Time.time * 5f) * 0.5f;  // Vertical sway
            float tiltZ = Mathf.Sin(Time.time * 3f) * 10f;   // Z-axis tilt for disorientation

            // Apply sway to camera position
            playerCamera.transform.localPosition = new Vector3(swayX, swayY, playerCamera.transform.localPosition.z);

            // Apply tilt to camera rotation
            playerCamera.transform.localRotation = Quaternion.Euler(0, 0, tiltZ);

            yield return null;
        }
    }

    private void ResetCamera()
    {
        // Reset camera to its original state
        playerCamera.transform.localPosition = Vector3.zero;
        playerCamera.transform.localRotation = Quaternion.identity;
    }
}
