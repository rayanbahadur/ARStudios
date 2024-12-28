using System;
using UnityEngine;
using UnityEngine.Events;
public class Detection : MonoBehaviour
{
    [SerializeField] Color searchingColor = Color.green; // Light color when searching
    [SerializeField] Color spottedColor = Color.red;    // Light color when spotting the player
    public UnityEvent gameOverEvent;

    private Light spotLight; // Reference to the Spotlight component

    void Start()
    {
        // Get the Light component attached to this GameObject
        spotLight = GetComponent<Light>();
        if (spotLight == null || spotLight.type != LightType.Spot)
        {
            Debug.LogError("No Spot Light component found on this GameObject. Please attach this script to a spotlight.");
            return;
        }

        // Set the initial spotlight color to searchingColor
        spotLight.color = searchingColor;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            RaycastHit hit;

            Debug.Log($"Trigger detected: {other.gameObject.name}, Tag: {other.tag}");


            // Cast a ray from the spotlight toward the player
            if (Physics.Raycast(transform.position, direction.normalized, out hit, 1000))
            {
                Debug.Log(hit.collider.tag);

                if (hit.collider.CompareTag("PlayerCapsule"))
                {
                    // Change the spotlight color to red (spotted)
                    spotLight.color = spottedColor;
                    gameOverEvent.Invoke();
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
