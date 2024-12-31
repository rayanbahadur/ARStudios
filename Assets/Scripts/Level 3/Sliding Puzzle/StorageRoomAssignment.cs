using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageRoomAssignment : MonoBehaviour
{
    [Header("Storage Rooms Variations")]
    [SerializeField] private GameObject easyRoom;
    [SerializeField] private GameObject mediumRoom;
    [SerializeField] private GameObject hardRoom;

    // Start is called before the first frame update
    void Start()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty", "Medium");
        if (difficulty == "Hard")
        {
            hardRoom.SetActive(true);
            easyRoom.SetActive(false);
            mediumRoom.SetActive(false);
        }
        else if(difficulty == "Easy"){
            hardRoom.SetActive(false);
            easyRoom.SetActive(true);
            mediumRoom.SetActive(false);
        }
        else
        {
            hardRoom.SetActive(false);
            easyRoom.SetActive(false);
            mediumRoom.SetActive(true);
        }
    }
}
