using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    public TMP_Text displayTimeText;

    private float currentTime = 30f;
    private float displayTime;

    public UnityEvent gameOverEvent;

    void Update()
    {
        currentTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 100) % 100);

        displayTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

        if (currentTime < 0)
        {
            Time.timeScale = 0;
            displayTimeText.text = "GAME OVER";
            gameOverEvent.Invoke();
        }
    }
}
