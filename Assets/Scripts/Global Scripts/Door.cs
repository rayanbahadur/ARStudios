using UnityEngine;

public class Door : MonoBehaviour
{
    public float openAngle = 80f; // Target angle for the door to open
    public float openSpeed = 2f;  // Speed of door opening
    private bool isOpening = false;

    private Quaternion closedRotation; // Initial rotation of the door
    private Quaternion openRotation;   // Target rotation of the door

    private void Start()
    {
        // Store the initial (closed) rotation
        closedRotation = transform.rotation;

        // Calculate the target (open) rotation
        openRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, openAngle);
    }

    private void Update()
    {
        if (isOpening)
        {
            // Smoothly rotate the door to the open position
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
        Debug.Log("The door is opening...");
    }
}
