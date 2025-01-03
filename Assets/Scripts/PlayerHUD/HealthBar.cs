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

        fill.color = gradient.Evaluate(slider.normalizedValue);
        healthText.text = "HEALTH: " + health;
    }
}
