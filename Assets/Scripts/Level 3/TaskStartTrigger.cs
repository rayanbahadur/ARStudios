using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStartTrigger : MonoBehaviour
{
    [SerializeField] private TaskProgressBar progressBar;
    [SerializeField] private string taskText;
    [SerializeField] private PlayerProgress playerProgressBar;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            progressBar.SetTaskText(taskText);
            playerProgressBar.SetProgress(0);
        }
    }
}
