using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the zombie
    public int currentHealth; // Current health of the zombie
    public HealthBar healthBar; // Reference to the HealthBar component

    private PlayerProgress playerProgress; // Reference to the PlayerProgress script

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
            //Debug.Log("Progress set to 75.");
        }

        Destroy(gameObject); // Destroy the zombie GameObject
    }
}
