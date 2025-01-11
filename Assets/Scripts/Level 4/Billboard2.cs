using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard2 : MonoBehaviour
{
    public Transform cam;

    // Update is called once per frame
    void LateUpdate()
    {
        // Get the direction the camera is facing
        Vector3 targetDirection = transform.position + cam.forward;

        // Create a rotation that faces the camera
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection - transform.position);

        // Add a 180-degree rotation along the Y-axis
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + 180f, targetRotation.eulerAngles.z);

        // Apply the rotation to the object
        transform.rotation = targetRotation;
    }
}