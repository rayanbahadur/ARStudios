using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the zombie
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Zombie took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0f)
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
