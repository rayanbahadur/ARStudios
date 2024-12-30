using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CheckpointData
{
    public string levelToLoad;
    public float x;
    public float y;
    public float z;
}

public class CheckpointManagerG : MonoBehaviour
{
    public Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveCheckpoint();
        }
    }
    public void SaveCheckpoint()
    {
        // Create a data object
        CheckpointData data = new CheckpointData
        {
            levelToLoad = SceneManager.GetActiveScene().buildIndex.ToString(),
            x = player.position.x,
            y = player.position.y,
            z = player.position.z
        };

        // Convert to JSON and save
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("CheckpointData", jsonData);
        PlayerPrefs.Save();

        Debug.Log("Checkpoint saved: " + jsonData);
    }

}
