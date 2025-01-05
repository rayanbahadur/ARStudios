using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Maximum health of the zombie
    public int currentHealth; // Current health of the zombie
    public HealthBar healthBar; // Reference to the HealthBar component

    [Header("Progress Settings")]
    private PlayerProgress playerProgress; // Reference to the PlayerProgress script

    [Header("Audio Settings")]
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip zombieDeathSound; // Sound played when the zombie dies
    public AudioClip proximitySound; // Sound played when the player is near the zombie
    public float proximityRadius = 10f; // Distance within which the proximity sound plays
    private bool isPlayerNearby = false; // Tracks if the player is within range

    private Transform playerTransform; // Reference to the player's transform

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        healthBar.SetMaxHealth(maxHealth); // Initialize the health bar

        // Find the PlayerProgress component in the scene
        playerProgress = FindObjectOfType<PlayerProgress>();
        if (playerProgress == null)
        {
            Debug.LogError("PlayerProgress component not found in the scene!");
        }

        // Find the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found in the scene!");
        }
    }

    void Update()
    {
        // Check if the player is nearby and play the proximity sound if not already playing
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= proximityRadius && !isPlayerNearby)
            {
                isPlayerNearby = true;
                PlayProximitySound();
            }
            else if (distanceToPlayer > proximityRadius && isPlayerNearby)
            {
                isPlayerNearby = false; // Reset the flag if the player moves out of range
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.SetHealth(currentHealth); // Update the health bar
        Debug.Log($"Zombie took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Zombie has died.");

        // Set player progress to 75 if the PlayerProgress component is found
        if (playerProgress != null && playerProgress.currentProgress < 75)
        {
            playerProgress.SetProgress(75);
            playerProgress.SetTaskText("Zombie defeated! Next: Unlock the Door");
        }

        // Play the zombie death sound
        audioSource.volume = PlayerPrefs.GetFloat("SoundEffectVolume", 1.0f); // Set the volume of the audio source
        audioSource.PlayOneShot(zombieDeathSound);

        Destroy(gameObject); // Destroy the zombie GameObject
    }

    private void PlayProximitySound()
    {
        if (proximitySound != null && audioSource != null)
        {
            audioSource.volume = PlayerPrefs.GetFloat("SoundEffectVolume", 1.0f); // Set the volume of the audio source
            audioSource.PlayOneShot(proximitySound); // Play the proximity sound
        }
    }
}
