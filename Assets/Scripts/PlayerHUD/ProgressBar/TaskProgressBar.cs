using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TaskProgressBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI taskText;
    public float animationDuration = 0.5f; // Duration of the animation in seconds

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
        StartCoroutine(AnimateProgress(progressPercentage));
    }

    private IEnumerator AnimateProgress(int targetProgress)
    {
        float startProgress = slider.value;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float newProgress = Mathf.Lerp(startProgress, targetProgress, elapsedTime / animationDuration);
            slider.value = newProgress;
            fill.color = gradient.Evaluate(slider.normalizedValue);
            progressText.text = Mathf.RoundToInt(newProgress) + "%";
            yield return null;
        }

        slider.value = targetProgress;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        progressText.text = targetProgress + "%";
    }

    public void SetTaskText(string task)
    {
        taskText.text = task;
    }
}