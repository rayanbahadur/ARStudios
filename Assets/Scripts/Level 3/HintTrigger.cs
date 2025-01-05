using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintTrigger : MonoBehaviour
{
    public string[] newHints;  // New hints to set
    public HintSystem hintSystem;  // Reference to the HintSystem script

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the player tag
        if (other.CompareTag("Player"))
        {
            if (hintSystem != null)
            {
                hintSystem.hints = newHints;
                hintSystem.currentHintIndex = 0;  // Reset the hint index
            }
        }
    }
}
