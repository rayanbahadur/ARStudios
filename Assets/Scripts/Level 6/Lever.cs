using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform leverHandle; // Reference to the lever handle (child of the board)
    public float rotationAngle = 80f; // Angle to rotate the lever
    public bool isActivated = false; // Bool to check if the lever is activated
    public float activationDistance = 3f; // Maximum distance to activate the lever
    private Transform playerTransform; // Reference to the player's transform

    private void Start()
    {
        // Ensure leverHandle is assigned
        if (leverHandle == null)
        {
            leverHandle = transform.GetChild(0); // Assuming the lever handle is the first child
        }

        // Find the player in the scene
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnMouseDown()
    {
        if (!isActivated && IsPlayerCloseEnough())
        {
            // Rotate the lever handle
            leverHandle.localRotation = Quaternion.Euler(rotationAngle, 0, 0);
            isActivated = true;

            // Check if all levers are activated
            LeverManager.Instance.CheckAllLevers();
        }
    }

    private bool IsPlayerCloseEnough()
    {
        if (playerTransform == null)
        {
            return false;
        }

        float distance = Vector3.Distance(playerTransform.position, transform.position);
        return distance <= activationDistance;
    }

    // Method to reset the lever
    public void ResetLever()
    {
        leverHandle.localRotation = Quaternion.Euler(0, 0, 0);
        isActivated = false;
    }
}
