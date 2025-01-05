using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    public static LeverManager Instance; // Singleton instance
    public List<Lever> levers; // List of all levers
    public Transform gate; // Reference to the gate
    public float openHeight = 5f; // Height to move the gate upwards
    public float openSpeed = 2f; // Speed at which the gate opens
    [SerializeField] private PlayerProgress playerProgress; // Reference to the PlayerProgress script

    private void Awake()
    {
        // Initialize the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckAllLevers()
    {
        // Check if all levers are activated
        foreach (Lever lever in levers)
        {
            if (!lever.isActivated)
            {
                return; // If any lever is not activated, return
            }
        }

        // If all levers are activated, open the gate
        StartCoroutine(OpenGate());
    }

    private IEnumerator OpenGate()
    {
        Vector3 targetPosition = gate.position + Vector3.up * openHeight;
        while (Vector3.Distance(gate.position, targetPosition) > 0.01f)
        {
            gate.position = Vector3.MoveTowards(gate.position, targetPosition, openSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("All levers activated. Gate opened!");
    }

    // Method to reset all levers
    public void ResetLevers()
    {
        foreach (Lever lever in levers)
        {
            lever.ResetLever();
        }
        Debug.Log("All levers have been reset.");

        // Set the task progress back to 60%
        if (playerProgress != null)
        {
            playerProgress.SetProgress(60);
        }
    }
}
