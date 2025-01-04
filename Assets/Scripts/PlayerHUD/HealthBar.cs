using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthText;
    public Image heartImage; 
    public Sprite poisonHeartSprite; 
    public Sprite normalHeartSprite; 
    public Gradient poisonGradient;
    public AudioSource audioSource; 
    public AudioClip coughingSound;

    private bool isPoisoned = false; // Track the poison status

    // Set the max health of the player
    public void SetMaxHealth(int health) 
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
        healthText.text = "HEALTH: " + health;
    }

    // Set the health of the player
    public void SetHealth(int health)
    {
        slider.value = health;

        if (isPoisoned)
        {
            fill.color = poisonGradient.Evaluate(slider.normalizedValue);
        }
        else
        {
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
        healthText.text = "HEALTH: " + health;
    }

    // Toggle poison effect
    public void TogglePoisonEffect()
    {
        isPoisoned = !isPoisoned;

        if (isPoisoned)
        {
            fill.color = poisonGradient.Evaluate(slider.normalizedValue);
            heartImage.sprite = poisonHeartSprite;

            float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.8f);
            // Play the coughing sound
            if (audioSource != null)
            {
                audioSource.volume = soundEffectVolume;
                audioSource.clip = coughingSound;
                audioSource.loop = true;
                audioSource.Play();
            }

        }
        else
        {
            fill.color = gradient.Evaluate(slider.normalizedValue);
            heartImage.sprite = normalHeartSprite;

            // Stop the coughing sound
            if (audioSource != null)
            {
                audioSource.loop = false; // Disable looping
                audioSource.Stop();
            }
        }
    }
}