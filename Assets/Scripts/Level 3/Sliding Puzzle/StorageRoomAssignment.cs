using System.Collections.Generic;
using UnityEngine;

public class StorageRoomAssignment : MonoBehaviour
{
    [System.Serializable]
    public class RoomConfig
    {
        public GameObject room;    // The room GameObject
        public GameObject manager; // The corresponding manager GameObject
    }

    [Header("Storage Room Configurations")]
    [SerializeField] private List<RoomConfig> roomConfigs; // A list of configurations for each difficulty
    private void Start()
    {
        // Get the current difficulty
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy");

        // Activate the correct room and manager based on difficulty
        foreach (var config in roomConfigs)
        {
            bool isActive = string.Equals(config.manager.name, difficulty, System.StringComparison.OrdinalIgnoreCase);
            config.room.SetActive(isActive);
            config.manager.SetActive(isActive);
        }
    }
}
