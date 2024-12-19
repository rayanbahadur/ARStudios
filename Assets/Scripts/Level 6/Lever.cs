using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform leverHandle; // Reference to the lever handle (child of the board)
    public float rotationAngle = 80f; // Angle to rotate the lever
    public bool isActivated = false; // Bool to check if the lever is activated

    private void Start()
    {
        // Ensure leverHandle is assigned
        if (leverHandle == null)
        {
            leverHandle = transform.GetChild(0); // Assuming the lever handle is the first child
        }
    }

    private void OnMouseDown()
    {
        if (!isActivated)
        {
            // Rotate the lever handle
            leverHandle.localRotation = Quaternion.Euler(rotationAngle, 0, 0);
            isActivated = true;

            // Check if all levers are activated
            LeverManager.Instance.CheckAllLevers();
        }
    }
}
