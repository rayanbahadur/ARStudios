using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Follow the player
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    // Trigger detection when the enemy touches the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDies();
        }
    }

    // Method to handle player death
    private void PlayerDies()
    {
        UnityEngine.Debug.Log("Player has died!");
        player.gameObject.SetActive(false);
    }
}
