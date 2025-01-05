using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar; // Reference to the HealthBar component
    public UnityEvent gameOverEvent;

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
            TakeDamage(20);
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
        if (currentHealth <= 0 && 
        CheckpointManager.Instance != null &&
        CheckpointManager.Instance.GetLastCheckpointPosition() == Vector3.zero)
        {
            gameOverEvent.Invoke();
        }
        else 
        {
            healthBar.SetHealth(maxHealth);
        }
    }

}