using UnityEngine;
using TMPro; // For TextMeshPro support

[RequireComponent(typeof(Rigidbody))]
public class GravityFlipper : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float gravityMagnitude = 9.81f; // Strength of gravity
    public float rotationSpeed = 5f;       // Speed of flipping

    [Header("UI Elements")]
    public TextMeshProUGUI interactionPromptChanges; // Displays remaining gravity changes
    public TextMeshProUGUI interactionPromptTimer;   // Displays time left or action prompt

    private Rigidbody rb;
    private bool isUpsideDown = false;
    private float targetZRotation; // Target Z-axis rotation for smooth flipping
    private int remainingChanges; // Counter for remaining gravity changes
    private float gravityChangeDuration; // Duration for gravity inversion
    private float gravityChangeStartTime; // Time when the current gravity change started
    private bool isGravityChanged = false; // Tracks if gravity is currently flipped

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable Unity's default gravity

        // Set difficulty parameters
        SetDifficultyParameters();

        // Ensure both interaction prompts are active and initialized
        if (interactionPromptChanges != null)
        {
            interactionPromptChanges.gameObject.SetActive(true);
            UpdateInteractionPromptChanges();
        }

        if (interactionPromptTimer != null)
        {
            interactionPromptTimer.gameObject.SetActive(true);
            UpdateInteractionPromptTimer();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && remainingChanges > 0)
        {
            if (!isGravityChanged)
            {
                ToggleGravityDirection();
                gravityChangeStartTime = Time.time; // Record the start time
                isGravityChanged = true; // Gravity is now flipped
                remainingChanges--;
            }
            else
            {
                // Allow reverting manually before the duration ends
                RevertGravity();
            }

            // Update both prompts
            UpdateInteractionPromptChanges();
            UpdateInteractionPromptTimer();
        }

        // Automatically revert gravity after the set duration
        if (isGravityChanged && Time.time >= gravityChangeStartTime + gravityChangeDuration)
        {
            RevertGravity();
            UpdateInteractionPromptTimer();
        }

        // Smoothly rotate the Z-axis while preserving X and Y axes
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, targetZRotation);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Continuously update timer if gravity is flipped
        if (isGravityChanged && interactionPromptTimer != null)
        {
            float timeLeft = gravityChangeStartTime + gravityChangeDuration - Time.time;
            interactionPromptTimer.text = $"Gravity Reverting In: {timeLeft:F1}s";
        }
    }

    void FixedUpdate()
    {
        // Apply custom gravity
        Vector3 gravityDirection = isUpsideDown ? Vector3.up : Vector3.down;
        rb.AddForce(gravityDirection * gravityMagnitude, ForceMode.Acceleration);
    }

    void ToggleGravityDirection()
    {
        isUpsideDown = !isUpsideDown;
        targetZRotation = isUpsideDown ? 180f : 0f;
    }

    void RevertGravity()
    {
        isUpsideDown = false; // Reset to normal gravity
        targetZRotation = 0f; // Reset rotation
        isGravityChanged = false; // Allow further changes
    }

    void SetDifficultyParameters()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy");
        switch (difficulty)
        {
            case "Easy":
                gravityChangeDuration = 10f;
                remainingChanges = 15;
                break;
            case "Medium":
                gravityChangeDuration = 7f;
                remainingChanges = 10;
                break;
            case "Hard":
                gravityChangeDuration = 5f;
                remainingChanges = 7;
                break;
            default:
                gravityChangeDuration = 10f; // Default to Easy
                remainingChanges = 15;
                break;
        }
    }

    void UpdateInteractionPromptChanges()
    {
        if (interactionPromptChanges != null)
        {
            interactionPromptChanges.text = $"Gravity changes Left: {remainingChanges}";
        }
    }

    void UpdateInteractionPromptTimer()
    {
        if (interactionPromptTimer != null)
        {
            interactionPromptTimer.text = isGravityChanged
                ? $"Gravity Reverting In: {(gravityChangeStartTime + gravityChangeDuration - Time.time):F1}s"
                : "Press G to change gravity";
        }
    }
}
