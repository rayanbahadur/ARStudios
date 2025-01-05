using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar; // Reference to the HealthBar component

    [Header("Game Over Settings")]
    public  GameObject gameOverUI;
    public GameObject mainCanvas;

    [Header("Player Audio Settings")]
    public AudioSource audioSource; 
    public AudioClip dyingSound;

    [Header("Audio Sources")]
    public List<AudioSource> audioSources; // List of audio sources to disable

    private bool isDying = false; // Flag to check if the DieCoroutine is running

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth); // Initialize the health bar
    }

    void Update()
    {
        // Test taking damage
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(95);
        }

        // Test poison effect
        if (Input.GetKeyDown(KeyCode.P))
        {
            healthBar.TogglePoisonEffect();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        healthBar.SetHealth(currentHealth); // Update the health bar
    }

    public void Die()
    {
        if (!isDying)
        {
            StartCoroutine(DieCoroutine());
        }
    }

    private IEnumerator DieCoroutine()
    {
        isDying = true; // Set the flag to true
        Debug.Log("Player died.");
        float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.8f);
        if (audioSource != null && dyingSound != null)
        {
            audioSource.volume = soundEffectVolume;
            audioSource.clip = dyingSound;
            audioSource.loop = false;
            audioSource.Play(); 
            yield return new WaitForSeconds(dyingSound.length);
        }

        if (currentHealth <= 0 && 
            CheckpointManager.Instance != null && 
            CheckpointManager.Instance.GetLastCheckpointPosition() == Vector3.zero)
        {
            gameOverState();
        }
        else 
        {
            healthBar.SetHealth(maxHealth);
        }

        isDying = false; // Reset the flag
    }

    public void gameOverState()
    {
        // Deactivate all items in the main canvas
        if (mainCanvas != null)
        {
            foreach (Transform child in mainCanvas.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        gameOverUI.SetActive(true);
        Time.timeScale = 0;

        // Disable all audio sources
        foreach (var audioSource in audioSources)
        {
            if (audioSource != null)
            {
                audioSource.enabled = false;
            }
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}