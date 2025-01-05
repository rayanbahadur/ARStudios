using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskProgressBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI taskText;


    // Set the max progress of the player
    public void SetMaxProgress(int progressPercentage) 
    {
        slider.maxValue = progressPercentage;
        slider.value = progressPercentage;

        fill.color = gradient.Evaluate(1f);
        progressText.text = progressPercentage + "%";
    }

    // Set the progress of the player
    public void SetProgress(int progressPercentage)
    {
        slider.value = progressPercentage;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        progressText.text = progressPercentage + "%";
    }

    public void SetTaskText(string task)
    {
        taskText.text = task;
    }
}