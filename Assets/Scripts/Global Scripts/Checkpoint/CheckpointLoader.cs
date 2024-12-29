using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CheckpointLoader : MonoBehaviour
{
    public Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadCheckpoint();
        }
    }

    public void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointData"))
        {
            // Retrieve and deserialize JSON
            string jsonData = PlayerPrefs.GetString("CheckpointData");
            CheckpointData data = JsonUtility.FromJson<CheckpointData>(jsonData);

            // Subscribe to the sceneLoaded event
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                player = GameObject.FindWithTag("Player").transform;

                // Move the root object
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false; // Disable temporarily
                    player.position = new Vector3(data.x, data.y, data.z);
                    controller.enabled = true;  // Re-enable
                }
                else
                {
                    player.position = new Vector3(data.x, data.y, data.z);
                }

                // Adjust follow camera or child objects if necessary
                Transform followCamera = player.Find("PlayerFollowCamera");
                if (followCamera != null)
                {
                    followCamera.localPosition = Vector3.zero; // Reset local position
                }

                // Unsubscribe to prevent duplicate calls
                SceneManager.sceneLoaded -= (s, m) => { };
            };

            // Load the specified scene
            SceneManager.LoadScene(int.Parse(data.levelToLoad));

            Debug.Log($"Checkpoint loaded: Level={data.levelToLoad}, Position=({data.x}, {data.y}, {data.z})");
        }
        else
        {
            Debug.LogWarning("No checkpoint data found!");
        }
    }

}
