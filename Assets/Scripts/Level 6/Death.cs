using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    [Header("Demon Light Settings")]
    public Transform demonHead; // Origin of the raycast (demon's head)
    public LayerMask obstructionMask; // Layers that can block the demon light
    public bool isDemonLight = false; // Toggle for objects that represent the demon light

    [Header("Fade UI Settings")]
    public Image fadeImage; // Reference to the Image component for fading
    public float fadeDuration = 1.0f; // Duration of the fade effect

    [Header("Audio Settings")]
    public AudioClip deathSound; // Sound clip to play when the player dies
    public AudioSource deathAudioSource; // Public AudioSource to assign from the scene

    [Header("Player Settings")]
    public PlayerHealth playerHealth; // Reference to the PlayerHealth component

    private bool diedToDemonLight = false; // Flag to track if the player died to the demon light

    private void Start()
    {
        if (deathAudioSource == null)
        {
            Debug.LogWarning("Death AudioSource is not assigned. Please assign an AudioSource in the Inspector.");
        }
        else
        {
            // Retrieve and set the volume for the audio source
            float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.8f);
            deathAudioSource.volume = soundEffectVolume;
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth is not assigned. Please assign a PlayerHealth component in the Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.tag); // Debug log to check trigger entry

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger."); // Debug log to confirm player entry

            // Check if this is a demon light and validate with a raycast
            if (isDemonLight && demonHead != null)
            {
                if (IsPlayerVisible(other.transform))
                {
                    diedToDemonLight = true; // Set the flag to true if the player dies to the demon light
                    HandlePlayerDeath();
                    StartCoroutine(HandleRespawn(other.gameObject));
                    Debug.Log("Player was visible to the demon. Respawn triggered.");
                }
                else
                {
                    Debug.Log("Player is obstructed from the demon light. No respawn.");
                }
            }
            else
            {
                diedToDemonLight = false; // Set the flag to false if the player dies to other causes
                HandlePlayerDeath();
                StartCoroutine(HandleRespawn(other.gameObject));
            }
        }
    }

    // Method to check if the player is visible to the demon
    private bool IsPlayerVisible(Transform playerTransform)
    {
        Vector3 directionToPlayer = playerTransform.position - demonHead.position;
        RaycastHit hit;
        float sphereRadius = 0.1f; // Adjust the radius as needed

        Debug.DrawRay(demonHead.position, directionToPlayer, Color.red, 1.0f); // Draw the ray in the scene view for debugging

        // Perform spherecast to check for obstructions
        if (Physics.SphereCast(demonHead.position, sphereRadius, directionToPlayer, out hit, Mathf.Infinity, obstructionMask))
        {
            Debug.Log("SphereCast hit: " + hit.collider.name); // Debug log to see what the spherecast hits

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("SphereCast hit the player directly. Player is visible to the demon.");
                return true; // Player is visible
            }
            else
            {
                Debug.Log("SphereCast hit an obstruction: " + hit.collider.name);
                return false; // Player is obstructed
            }
        }
        else
        {
            Debug.Log("SphereCast did not hit anything.");
        }

        // Perform a separate spherecast specifically for the player
        int playerLayer = LayerMask.NameToLayer("Player"); // Assuming the player is on the Default layer
        LayerMask playerLayerMask = 1 << playerLayer;

        if (Physics.SphereCast(demonHead.position, sphereRadius, directionToPlayer, out hit, Mathf.Infinity, playerLayerMask))
        {
            Debug.Log("Separate SphereCast for player hit: " + hit.collider.name); // Debug log to see what the separate spherecast hits

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Separate SphereCast hit the player directly. Player is visible to the demon.");
                return true; // Player is visible
            }
        }

        return false; // Player is not visible
    }

    // Method to handle player death
    private void HandlePlayerDeath()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(100); // Inflict 100 damage to the player
            Debug.Log("Player health set to 0 due to death.");
        }
    }

    // Method to handle respawn logic with fade effect
    private IEnumerator HandleRespawn(GameObject player)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); // Activate the fade image

            // Play the death sound
            if (deathSound != null && deathAudioSource != null)
            {
                deathAudioSource.PlayOneShot(deathSound);
            }

            // Fade in
            yield return StartCoroutine(Fade(0, 1));

            // Respawn the player at the last checkpoint
            Vector3 checkpointPosition = CheckpointManager.Instance.GetLastCheckpointPosition();
            player.transform.position = checkpointPosition;
            Debug.Log("Player respawned at: " + checkpointPosition);

            // Reset all levers only if the player died to the demon light
            if (diedToDemonLight)
            {
                LeverManager.Instance.ResetLevers();
                Debug.Log("Levers reset due to death by demon light.");
            }

            // Set player's health to max
            if (playerHealth != null)
            {
                playerHealth.currentHealth = playerHealth.maxHealth;
                playerHealth.healthBar.SetHealth(playerHealth.currentHealth); // Update the health bar
                Debug.Log("Player health reset to max health: " + playerHealth.maxHealth);
            }

            // Fade out
            yield return StartCoroutine(Fade(1, 0));

            fadeImage.gameObject.SetActive(false); // Deactivate the fade image
        }
        else
        {
            Debug.LogWarning("FadeImage reference is missing.");
        }
    }

    // Coroutine to handle the fade effect
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(endAlpha);
    }

    // Method to set the alpha value of the fade image
    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }
}
