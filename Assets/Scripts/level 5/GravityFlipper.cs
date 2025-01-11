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
    public TextMeshProUGUI interactionPromptPotion;  // Displays potion effect details

    private Rigidbody rb;
    private bool isUpsideDown = false;
    private float targetZRotation; // Target Z-axis rotation for smooth flipping
    private int remainingChanges; // Counter for remaining gravity changes
    private float gravityChangeDuration; // Duration for gravity inversion
    private float gravityChangeStartTime; // Time when the current gravity change started
    private bool isGravityChanged = false; // Tracks if gravity is currently flipped
    private bool hasPotionEffect = false;  // Tracks if the potion effect is active

    [SerializeField] private PlayerHealth playerHealth;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable Unity's default gravity

        // Set difficulty and potion parameters
        SetDifficultyParameters();

        // Initialize and display prompts
        InitializePrompts();
    }
    void Update()
    {
        // Handle gravity flipping
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

            // Update UI prompts
            UpdateInteractionPromptChanges();
        }

        // Continuously update timer if gravity is flipped
        if (isGravityChanged)
        {
            float timeLeft = Mathf.Max(0, gravityChangeStartTime + gravityChangeDuration - Time.time);

            // Automatically revert gravity after the set duration
            if (timeLeft <= 0)
            {
                RevertGravity();
            }

            // Update timer text
            UpdateInteractionPromptTimer(timeLeft);
        }

        // Smoothly rotate the Z-axis while preserving X and Y axes
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, targetZRotation);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
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
        UpdateInteractionPromptTimer(0); // Reset timer text
        if (remainingChanges <= 0)
        {
            // Disable gravity change when no changes are left
            interactionPromptTimer.text = "No gravity changes left - Game over!";
            playerHealth.TakeDamage(100); // Kill the player

        }
    }

    void SetDifficultyParameters()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy");
        string gravityPotion = PlayerPrefs.GetString("GravityPotion", "Not Drank");

        // Handle potion effect
        if (gravityPotion == "Drank")
        {
            gravityChangeDuration = 13f;
            remainingChanges = 16;
            hasPotionEffect = true;

            if (interactionPromptPotion != null)
            {
                interactionPromptPotion.text = "Gravity potion drank: Extra 3s and 16 changes!";
                interactionPromptPotion.gameObject.SetActive(true);
            }
        }
        else
        {
            hasPotionEffect = false;
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

            if (interactionPromptPotion != null)
            {
                interactionPromptPotion.text = ""; // Clear the potion prompt if no potion effect
                interactionPromptPotion.gameObject.SetActive(false);
            }
        }
    }

    void InitializePrompts()
    {
        if (interactionPromptChanges != null)
        {
            interactionPromptChanges.gameObject.SetActive(true);
            UpdateInteractionPromptChanges();
        }

        if (interactionPromptTimer != null)
        {
            interactionPromptTimer.gameObject.SetActive(true);
            UpdateInteractionPromptTimer(0); // Start with "Press G to change gravity"
        }

        if (interactionPromptPotion != null)
        {
            interactionPromptPotion.gameObject.SetActive(true);
            interactionPromptPotion.text = ""; // Initialize with no text
        }
    }

    void UpdateInteractionPromptChanges()
    {
        if (interactionPromptChanges != null)
        {
            interactionPromptChanges.text = $"Gravity changes Left: {remainingChanges}";
        }
    }

    void UpdateInteractionPromptTimer(float timeLeft)
    {
        if (interactionPromptTimer != null)
        {
            interactionPromptTimer.text = isGravityChanged && timeLeft > 0
                ? $"Gravity Reverting In: {timeLeft:F1}s"
                : "Press G to change gravity";
        }
    }
}