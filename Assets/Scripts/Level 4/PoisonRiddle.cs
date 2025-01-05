using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonRiddle : MonoBehaviour
{
    [SerializeField] private GameObject poisonSmoke;
    [SerializeField] private PlayerHealth playerHealth;
    private float tickRate;
    private float nextTickTime;

    void Start()
    {
        // Set the appropriate tick rate based on difficulty
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy");
        switch (difficulty)
        {
            case "Easy":
                tickRate = 3f; // Tick every 3 seconds = 5 minutes
                break;
            case "Medium":
                tickRate = 1.5f; // Tick every 1.5 seconds = 2.5 minutes
                break;
            case "Hard":
                tickRate = 0.8f; // Tick every 0.8 seconds = 1.5 minutes
                break;
        }

        // Turn on the poison effect in the health bar
        playerHealth.healthBar.TogglePoisonEffect();

        // Activate the poison smoke particle effect
        poisonSmoke.SetActive(true);

        // Initialize the next tick time
        nextTickTime = Time.time + tickRate;
    }

    void Update()
    {
        // Check if it's time for the next tick
        if (Time.time >= nextTickTime)
        {
            // Apply damage to the player
            playerHealth.TakeDamage(1);

            // Update the next tick time
            nextTickTime = Time.time + tickRate;
        }
    }
}