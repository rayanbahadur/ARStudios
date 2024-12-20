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
    public float circleRadius = 5f; // Radius of the circular path
    public float circleSpeed = 1f; // Speed of the circular movement

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

        // Initialize the initial position
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the circular movement
        float angle = Time.time * circleSpeed;
        float x = Mathf.Cos(angle) * circleRadius;
        float z = Mathf.Sin(angle) * circleRadius;
        transform.position = initialPosition + new Vector3(x, 0, z);

        // Calculate the forward and backward movement
        float movementOffset = Mathf.Sin(Time.time * movementFrequency) * movementAmplitude;
        transform.position += transform.forward * movementOffset;

        // Rotate the demon to face the direction of movement
        Vector3 direction = new Vector3(x, 0, z).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
