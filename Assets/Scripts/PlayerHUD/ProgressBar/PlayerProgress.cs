using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{

    public int maxProgress = 100;
    public int currentProgress;
    public TaskProgressBar taskProgressBar; // Reference to the TaskProgressBar component

    void Start()
    {
        currentProgress = 0;
        taskProgressBar.SetMaxProgress(maxProgress); // Initialize the task progress bar
        taskProgressBar.SetProgress(currentProgress);
    }

    // Update is called once per frame
    void Update()
    {
        // Test taking damage
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddProgress(10);
        }
    }

    public void AddProgress(int progress)
    {
        currentProgress += progress;
        if (currentProgress > maxProgress)
        {
            currentProgress = maxProgress;
        }
        taskProgressBar.SetProgress(currentProgress); // Update the task progress bar
    }

    public void ResetProgress()
    {
        currentProgress = 0;
        taskProgressBar.SetProgress(currentProgress); // Update the task progress bar
    }
}
