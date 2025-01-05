using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the zombie
    public int currentHealth; // Current health of the zombie
    public HealthBar healthBar; // Reference to the HealthBar component

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        healthBar.SetMaxHealth(maxHealth); // Initialize the health bar
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
        Destroy(gameObject); // Destroy the zombie GameObject
    }
}
