using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 lastCheckpointPosition;

    private void Awake()
    {
        // Ensure there is only one instance of CheckpointManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to set the last checkpoint position
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        Debug.Log("Checkpoint set at: " + checkpointPosition);
    }

    // Method to get the last checkpoint position
    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}
