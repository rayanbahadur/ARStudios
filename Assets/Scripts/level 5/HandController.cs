using System.Collections;
using UnityEngine;
using TMPro;

public class HandController : MonoBehaviour
{
    public Transform handTransform; // Reference to the hand's transform
    public TextMeshProUGUI interactionPrompt; // Reference to the Interaction Prompt UI element
    private Transform currentItemTransform; // Transform of the current item in hand
    private bool isAnimating = false; // Prevent overlapping animations
    public float swordDamage = 25f; // Damage dealt by the sword
    public float raycastRange = 2f; // Range of the sword attack
    public string keyTag = "Key"; // Tag assigned to the key
    private PlayerProgress playerProgress; // Reference to the PlayerProgress script

    void Start()
    {
        // Find the PlayerProgress component in the scene
        playerProgress = FindObjectOfType<PlayerProgress>();
        if (playerProgress == null)
        {
            Debug.LogError("PlayerProgress component not found in the scene!");
        }

        // Ensure the prompt is hidden initially
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Interaction Prompt not assigned!");
        }
    }

    void Update()
    {
        // Check if there is a child item attached to the hand
        if (handTransform.childCount > 0)
        {
            currentItemTransform = handTransform.GetChild(0); // Get the first child (current hand item)

            // Check if the item's name matches "Sword(Clone)"
            if (currentItemTransform.name == "Sword(Clone)")
            {
                // Display interaction prompt for the sword
                if (interactionPrompt != null)
                {
                    interactionPrompt.text = "Press Q to attack";
                    interactionPrompt.gameObject.SetActive(true);
                }

                // Trigger animation on Q press
                if (Input.GetKeyDown(KeyCode.Q) && !isAnimating)
                {
                    StartCoroutine(AnimateSword());
                }
            }
            else
            {
                // Hide the prompt if the item is not a sword
                if (interactionPrompt != null)
                {
                    interactionPrompt.gameObject.SetActive(false);
                }

                ResetItemPosition();
            }

            // Check if the held item is a key and set progress to 25% only if it's less than 75%
            if (currentItemTransform.CompareTag(keyTag))
            {
                if (playerProgress != null && playerProgress.currentProgress < 25)
                {
                    playerProgress.SetProgress(25);
                    playerProgress.SetTaskText("Key Claimed. Next: Unlock Door");
                    Debug.Log("Progress set to 25% as the key is in hand and progress is below 75%.");
                }
            }
        }
        else
        {
            currentItemTransform = null;

            // Hide the prompt if no item is in hand
            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Performs the animation for the sword and applies damage once during the swing.
    /// </summary>
    private IEnumerator AnimateSword()
    {
        isAnimating = true;

        Quaternion initialRotation = currentItemTransform.localRotation;
        Quaternion intermediateRotation = Quaternion.Euler(0f, 0f, 0f); // Step 1: Rotate to (0, 0, 0)
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 80f);      // Step 2: Rotate to (0, 0, 80)

        float time = 0f;
        float animationSpeed = 5f;

        // Step 1: Rotate to (0, 0, 0)
        while (time < 1f)
        {
            time += Time.deltaTime * animationSpeed;
            currentItemTransform.localRotation = Quaternion.Lerp(initialRotation, intermediateRotation, time);
            yield return null;
        }

        // Perform raycast once here (at the midpoint of the swing)
        PerformRaycast();

        time = 0f;

        // Step 2: Rotate to (0, 0, 80)
        while (time < 1f)
        {
            time += Time.deltaTime * animationSpeed;
            currentItemTransform.localRotation = Quaternion.Lerp(intermediateRotation, targetRotation, time);
            yield return null;
        }

        time = 0f;

        // Step 3: Return to the initial position
        while (time < 1f)
        {
            time += Time.deltaTime * animationSpeed;
            currentItemTransform.localRotation = Quaternion.Lerp(targetRotation, initialRotation, time);
            yield return null;
        }

        isAnimating = false;
    }

    /// <summary>
    /// Casts a ray from the crosshair to detect enemies and applies damage once per swing.
    /// </summary>
    private void PerformRaycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Cast from the center of the screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            ZombieHealth zombieHealth = hit.collider.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(Mathf.RoundToInt(swordDamage)); // Convert float to int
                Debug.Log($"Zombie hit! Dealt {Mathf.RoundToInt(swordDamage)} damage.");
            }
        }
    }

    /// <summary>
    /// Resets the item position for non-sword items.
    /// </summary>
    private void ResetItemPosition()
    {
        if (currentItemTransform != null)
        {
            currentItemTransform.localPosition = Vector3.zero;
            currentItemTransform.localRotation = Quaternion.identity;
        }
    }
}
