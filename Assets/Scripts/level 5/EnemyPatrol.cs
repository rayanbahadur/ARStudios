using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    public int targetPoint = 0; // Current target patrol point
    public float patrolSpeed = 2f; // Movement speed while patrolling
    public float followSpeed = 4f; // Movement speed while following the player
    public float rotationSpeed = 5f; // Rotation smoothing speed
    public float detectionRadius = 5f; // Distance at which the enemy detects the player
    public float stoppingDistance = 1.5f; // Minimum distance to maintain from the player
    public Transform player; // Reference to the player

    private bool hasSpottedPlayer = false; // Tracks if the player has been spotted

    void Update()
    {
        if (!hasSpottedPlayer)
        {
            // Check distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.Log($"Distance to player: {distanceToPlayer}");

            // Spot the player if within detection radius
            if (distanceToPlayer <= detectionRadius)
            {
                hasSpottedPlayer = true;
                Debug.Log("Player spotted! Starting pursuit.");
            }
        }

        if (hasSpottedPlayer)
        {
            FollowPlayer(); // Always follow the player once spotted
        }
        else
        {
            Patrol(); // Patrol until the player is spotted
        }
    }

    void Patrol()
    {
        Debug.Log($"Patrolling towards point {targetPoint}");

        // Move toward the target patrol point
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, patrolSpeed * Time.deltaTime);

        // Check if the enemy has reached the patrol point
        if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.1f)
        {
            IncrementPoint();

            // Rotate an extra 90 degrees on the Y-axis
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90f, transform.rotation.eulerAngles.z);
            StartCoroutine(RotateToTarget(targetRotation));
        }
    }

    void FollowPlayer()
    {
        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log($"Following player. Distance to player: {distanceToPlayer}");

        // Only move if the enemy is farther than the stopping distance
        if (distanceToPlayer > stoppingDistance)
        {
            // Keep the enemy's current Y position while moving toward the player
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        // Smoothly rotate to face the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        if (directionToPlayer != Vector3.zero) // Avoid errors if the enemy reaches the exact position of the player
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void IncrementPoint()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
        Debug.Log($"Switching to patrol point {targetPoint}");
    }

    IEnumerator RotateToTarget(Quaternion targetRotation)
    {
        Debug.Log("Rotating to next patrol target");

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        transform.rotation = targetRotation; // Snap to the target rotation at the end
    }
}
