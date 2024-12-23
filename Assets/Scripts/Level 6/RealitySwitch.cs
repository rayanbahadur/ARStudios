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
    public float transitionDuration = 2.0f; // Time for the camera animation
    public Image blackoutImage;            // UI Image for blackout effect

    public bool isFractured = false;     // Track current reality state
    private bool hasPressedZ = false;     // Track if the player has toggled fractured reality
    public Coroutine transitionCoroutine;

    private ColorAdjustments colorAdjustments;
    private myControls inputActions;

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
    }

    private void Start()
    {
        // Ensure the scene starts in Normal Reality
        fracturedPostProcess.weight = 0;    // Disable fractured post-processing
        normalReality.SetActive(true);      // Enable Normal Reality objects
        fracturedReality.SetActive(false); // Disable Fractured Reality objects

        // Show the initial prompt
        if (promptText != null)
        {
            promptText.text = "Press Z to switch between realities!";
            promptText.gameObject.SetActive(true);
        }

        if (keyText != null)
        {
            keyText.gameObject.SetActive(false); // Hide the key text initially
        }

        // Get the color adjustments effect from the post-processing volume
        if (fracturedPostProcess.profile.TryGet(out ColorAdjustments colorAdjustmentsEffect))
        {
            colorAdjustments = colorAdjustmentsEffect;
        }

        // Ensure the blackout image is fully transparent at the start
        if (blackoutImage != null)
        {
            blackoutImage.color = new Color(0, 0, 0, 0);
        }
    }

    private void Update()
    {
        if (inputActions.Player.ToggleReality.WasPerformedThisFrame())
        {
            if (!hasPressedZ) // First-time toggle
            {
                hasPressedZ = true;

                // Hide the initial prompt
                if (promptText != null)
                {
                    promptText.gameObject.SetActive(false);
                }

                // Show "Find the Key" message
                if (keyText != null)
                {
                    keyText.gameObject.SetActive(true);
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

    public IEnumerator SmoothTransition(bool toFractured)
    {
        float timeElapsed = 0f;
        float halfDuration = transitionDuration / 2;
        float startWeight = fracturedPostProcess.weight;
        float endWeight = toFractured ? 1f : 0f;

        // Phase 1: Fade to black
        while (timeElapsed < halfDuration)
        {
            float t = timeElapsed / halfDuration;
            blackoutImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the blackout image is fully opaque
        blackoutImage.color = new Color(0, 0, 0, 1);

        // Enable the target reality at the midpoint of the transition
        if (toFractured)
        {
            fracturedReality.SetActive(true);
            normalReality.SetActive(false);
        }
        else
        {
            normalReality.SetActive(true);
            fracturedReality.SetActive(false);
        }

        // Phase 2: Fade to clear and adjust post-processing
        timeElapsed = 0f;
        while (timeElapsed < halfDuration)
        {
            float t = timeElapsed / halfDuration;
            blackoutImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            fracturedPostProcess.weight = Mathf.Lerp(startWeight, endWeight, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the blackout image is fully transparent
        blackoutImage.color = new Color(0, 0, 0, 0);
        fracturedPostProcess.weight = endWeight;
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
