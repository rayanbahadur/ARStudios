using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RealitySwitch : MonoBehaviour
{
    public GameObject normalReality;       // Parent object for Normal Reality
    public GameObject fracturedReality;   // Parent object for Fractured Reality
    public Volume fracturedPostProcess;   // Post-processing volume for fractured reality
    public Camera playerCamera;           // Player's camera

    public float transitionDuration = 1.0f; // Time for the camera animation

    private bool isFractured = false;     // Track current reality state
    private float blurIntensity = 0.5f;   // Adjust blur intensity

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // Press 'Z' to toggle
        {
            isFractured = !isFractured;
            SwitchReality();
        }
    }

    private void SwitchReality()
    {
        // Enable/Disable the respective realities
        normalReality.SetActive(!isFractured);
        fracturedReality.SetActive(isFractured);

        // Enable/Disable Post-Processing
        fracturedPostProcess.weight = isFractured ? 1 : 0;

        // Animate the camera only when entering the Fractured Reality
        if (isFractured)
        {
            StartCoroutine(AnimateCamera());
        }
    }

    private IEnumerator AnimateCamera()
    {
        float timeElapsed = 0f;
        Vector3 originalPosition = playerCamera.transform.localPosition;
        Quaternion originalRotation = playerCamera.transform.localRotation;

        while (timeElapsed < transitionDuration)
        {
            // Simulate a swaying motion by adjusting position and rotation
            float swayX = Mathf.Sin(Time.time * 5f) * 10f;  // Horizontal sway
            float swayY = Mathf.Cos(Time.time * 5f) * 10f;  // Vertical sway
            float tiltZ = Mathf.Sin(Time.time * 3f) * 20f;    // Z-axis tilt for disorientation

            // Apply sway to camera position
            playerCamera.transform.localPosition = originalPosition + new Vector3(swayX, swayY, 0f);

            // Apply tilt to camera rotation
            playerCamera.transform.localRotation = originalRotation * Quaternion.Euler(0, 0, tiltZ);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Reset camera to its original state after the animation
        playerCamera.transform.localPosition = originalPosition;
        playerCamera.transform.localRotation = originalRotation;
    }
}


