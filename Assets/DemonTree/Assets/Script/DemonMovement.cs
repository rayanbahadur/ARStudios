using UnityEngine;
using System.Collections;

public class DemonMovement : MonoBehaviour
{
    private Animator anim;
    int hIdles;
    int hAngry;
    int hAttack;
    int hGrabs;
    int hThumbsUp;

    public float rotationSpeed = 10f; // Speed at which the demon spins
    public float movementAmplitude = 0.1f; // Amplitude of the forward and backward movement
    public float movementFrequency = 1f; // Frequency of the forward and backward movement

    private Quaternion targetRotation;
    private Vector3 initialPosition;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        hIdles = Animator.StringToHash("Idles");
        hAngry = Animator.StringToHash("Angry");
        hAttack = Animator.StringToHash("Attack");
        hGrabs = Animator.StringToHash("Grabs");
        hThumbsUp = Animator.StringToHash("ThumbsUp");

        // Set the demon to idle mode
        if (anim != null)
        {
            anim.SetBool(hIdles, true);
        }

        // Initialize the target rotation and initial position
        targetRotation = transform.rotation;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the target rotation
        targetRotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Calculate the forward and backward movement
        float movementOffset = Mathf.Sin(Time.time * movementFrequency) * movementAmplitude;
        transform.position = initialPosition + transform.forward * movementOffset;
    }
}

