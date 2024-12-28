using System;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] Color searchingColor = Color.green; // Light color when searching
    [SerializeField] Color spottedColor = Color.red;    // Light color when spotting the player

    private Light spotLight; // Reference to the Spotlight component
    private string playerTag;

    void Start()
    {
        // Get the Light component attached to this GameObject
        spotLight = GetComponent<Light>();
        if (spotLight == null || spotLight.type != LightType.Spot)
        {
            Debug.LogError("No Spot Light component found on this GameObject. Please attach this script to a spotlight.");
            return;
        }

        // Find the player's tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTag = player.tag;
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }

        // Set the initial spotlight color to searchingColor
        spotLight.color = searchingColor;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Vector3 direction = other.transform.position - transform.position;
            RaycastHit hit;

            Debug.Log($"Trigger detected: {other.gameObject.name}, Tag: {other.tag}");


            // Cast a ray from the spotlight toward the player
            if (Physics.Raycast(transform.position, direction.normalized, out hit, 1000))
            {
                Debug.Log(hit.collider.tag);

                if (hit.collider.CompareTag(playerTag))
                {
                    // Change the spotlight color to red (spotted)
                    spotLight.color = spottedColor;
                }
                else
                {
                    // Change the spotlight color to green (searching)
                    spotLight.color = searchingColor;
                }
            }
        }
    }
}
