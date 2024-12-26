using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaircaseText : MonoBehaviour
{
    public TextMeshProUGUI staircaseMessage; // Reference to the TextMeshProUGUI Text element

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Display the text
            if (staircaseMessage != null)
            {
                staircaseMessage.gameObject.SetActive(true);
            }

            // Disable the trigger
            GetComponent<Collider>().enabled = false;
        }
    }
}
