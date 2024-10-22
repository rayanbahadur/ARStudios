using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Import UnityEvent namespace

public class Keypad : MonoBehaviour
{
    public string correctCode = "284";
    private string inputCode = "";
    public UnityEvent chickenDinner;

    public void EnterDigit(string digit)
    {
        if (inputCode.Length < 3)
        {
            inputCode += digit;
        }

        if (inputCode.Length == 3)
        {
            CheckCode();
        }
    }

    private void CheckCode()
    {
        if (inputCode == correctCode)
        {
            chickenDinner.Invoke();
        }
        else
        {
            inputCode = ""; // Reset the input code if incorrect
        }
    }
}
