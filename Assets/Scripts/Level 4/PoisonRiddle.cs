using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonRiddle : MonoBehaviour
{
    [SerializeField] private GameObject poisonSmoke;
    [SerializeField] private PlayerHealth playerHealth;
    private float tickRate;
    private float nextTickTime;

    public static PoisonRiddle Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Set the appropriate tick rate based on difficulty
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy");
        switch (difficulty)
        {
            case "Easy":
                tickRate = 3f; // Tick every 3 seconds
                break;
            case "Medium":
                tickRate = 1.5f; // Tick every 1.5 seconds
                break;
            case "Hard":
                tickRate = 0.8f; // Tick every 0.8 seconds
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


        // Check if the player presses 'Q' to drink the potion
        if (Input.GetKeyDown(KeyCode.Q) && Inventory.Instance.currentHandItem != null && Inventory.Instance.currentHandItem.name.Contains("Potion_01"))
        {
            Inventory.Instance.DrinkingAnimation("CurePoison", "PoisonCurePotion");
        }
    }

    public void CurePoison()
    {
        // Turn off the poison effect in the health bar
        playerHealth.healthBar.TogglePoisonEffect();

        // Deactivate the poison smoke particle effect
        poisonSmoke.SetActive(false);

        // Stop the tick damage
        enabled = false;

        Debug.Log("Poison cured.");
    }
}