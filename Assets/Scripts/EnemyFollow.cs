using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public TMP_Text countdownText;  // Reference to the TextMeshPro countdown text
    public float countdownDuration = 5f;  // Countdown duration
    private float currentCountdown;

    // Colors for different stages of the countdown
    private static readonly Color SAFE_COLOR = Color.green;  // Green
    private static readonly Color WARNING_COLOR = new Color(1f, 0.55f, 0f);  // Dark Orange
    private static readonly Color DANGER_COLOR = new Color(0.86f, 0.08f, 0.24f);  // Crimson
    private static readonly Color FLASH_COLOR = Color.red;  // Flashing color

    public float distanceBehindPlayer = 0f; // Distance to spawn behind the player
    private bool isChasing = false; // Flag to check if the enemy should chase

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Position the enemy behind the player initially
        PositionBehindPlayer();

        // Start the countdown
        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        // Only update the destination if chasing
        if (isChasing && player != null && agent != null)
        {
            agent.SetDestination(player.position);
            Debug.Log($"Enemy moving towards player at position: {player.position}"); // Debug statement
        }
    }

    // Coroutine to handle the countdown
    IEnumerator StartCountdown()
    {
        // Set the initial message and start the countdown
        countdownText.text = "Get Ready! Escape the Maze and Stay Away From the Enemy!";
        currentCountdown = countdownDuration;

        yield return new WaitForSeconds(2f);  // Display the message for 2 seconds before starting the countdown

        while (currentCountdown > 0)
        {
            countdownText.text = Mathf.Ceil(currentCountdown).ToString();  // Update the countdown text
            UpdateCountdownColor();  // Change the color based on time left
            yield return new WaitForSeconds(1f);  // Wait for 1 second
            currentCountdown--;
        }

        // When countdown hits 0, hide the text and let the enemy start chasing
        countdownText.text = "RUN!";
        countdownText.color = FLASH_COLOR;
        yield return new WaitForSeconds(1f);
        countdownText.enabled = false;

        // Start chasing the player
        StartChasing();
    }

    // Function to start chasing the player
    void StartChasing()
    {
        isChasing = true;
        Debug.Log("Enemy has started chasing the player."); // Debug statement
    }

    // Function to position the enemy behind the player
    void PositionBehindPlayer()
    {
        Vector3 behindPlayer = player.position - player.forward * distanceBehindPlayer;
        transform.position = behindPlayer;
    }

    // Trigger detection when the enemy touches the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDies();
        }
    }

    // Function to handle player death
    private void PlayerDies()
    {
        Debug.Log("Player has died!");
        player.gameObject.SetActive(false);
    }

    // Update the color of the countdown text based on the remaining time
    void UpdateCountdownColor()
    {
        if (currentCountdown > 4f)
        {
            countdownText.color = SAFE_COLOR;
        }
        else if (currentCountdown > 3f) 
        {
            countdownText.color = WARNING_COLOR;
        }
        else if (currentCountdown > 0f)
        {
            countdownText.color = DANGER_COLOR;
        }
        else
        {
            countdownText.color = FLASH_COLOR;
        }
    }

}
