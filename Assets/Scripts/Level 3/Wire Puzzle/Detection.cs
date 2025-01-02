using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Detection : MonoBehaviour
{
    [Header("Detection Lights")]
    [SerializeField] Color searchingColor = Color.green; // Light color when searching
    [SerializeField] Color spottedColor = Color.red;    // Light color when spotting the player

    [Header("Fade UI Settings")]
    public Image fadeImage; // Reference to the Image component for fading
    public float fadeDuration = 1.0f; // Duration of the fade effect

    [Header("Audio Settings")]
    public AudioClip deathSound; // Sound clip to play when the player dies
    public AudioSource deathAudioSource; // Public AudioSource to assign from the scene

    private Light spotLight; // Reference to the Spotlight component
    private bool isRespawning = false;

    void Start()
    {
        // Get the Light component attached to this GameObject
        spotLight = GetComponent<Light>();
        if (spotLight == null || spotLight.type != LightType.Spot)
        {
            Debug.LogError("No Spot Light component found on this GameObject. Please attach this script to a spotlight.");
            return;
        }

        // Set the initial spotlight color to searchingColor
        spotLight.color = searchingColor;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            RaycastHit hit;

            Debug.Log($"Trigger detected: {other.gameObject.name}, Tag: {other.tag}");


            // Cast a ray from the spotlight toward the player
            if (Physics.Raycast(transform.position, direction.normalized, out hit, 1000))
            {
                Debug.Log(hit.collider.tag);

                if (hit.collider.CompareTag("PlayerCapsule"))
                {
                    // Change the spotlight color to red (spotted)
                    spotLight.color = spottedColor;
                    StartCoroutine(HandleRespawn(other.gameObject));
                }
                else
                {
                    // Change the spotlight color to green (searching)
                    spotLight.color = searchingColor;
                }
            }
        }
    }

    private IEnumerator HandleRespawn(GameObject player)
    {
        isRespawning = true; // Prevent re-triggering

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); // Activate the fade image

            // Start playing the sound
            if (deathSound != null && deathAudioSource != null)
            {
                float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.8f);
                deathAudioSource.volume = soundEffectVolume;

                deathAudioSource.clip = deathSound;
                deathAudioSource.Play();
            }

            // Start the fade-in effect while the sound plays
            yield return StartCoroutine(FadeWhileSoundPlays(0, 1, fadeDuration));

            // Respawn the player at the last checkpoint
            Vector3 checkpointPosition = CheckpointManager.Instance.GetLastCheckpointPosition();
            player.transform.position = checkpointPosition;
            Debug.Log("Player respawned at: " + checkpointPosition);

            // Start the fade-out effect
            yield return StartCoroutine(Fade(1, 0));

            fadeImage.gameObject.SetActive(false); // Deactivate the fade image
        }
        else
        {
            Debug.LogWarning("FadeImage reference is missing.");
        }

        spotLight.color = searchingColor; // Reset the spotlight color
        isRespawning = false; // Allow re-triggering after respawn is complete
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
    private IEnumerator FadeWhileSoundPlays(float startAlpha, float endAlpha, float fadeTime)
    {
        float elapsedTime = 0;
        float soundDuration = deathSound.length;

        // Use the shorter duration of fade or sound to sync the effect
        float duration = Mathf.Min(fadeTime, soundDuration);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            SetAlpha(alpha);
            yield return null;
        }

        // Ensure the final alpha is set correctly
        SetAlpha(endAlpha);
    }

}
