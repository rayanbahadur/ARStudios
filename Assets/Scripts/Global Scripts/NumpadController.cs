using UnityEngine;
using TMPro;

public class NumpadController : MonoBehaviour
{
    public RandomNumberGenerator numberGenerator;
    public TextMeshProUGUI inputField;
    private int[] correctNumbers;

    void Start()
    {
        correctNumbers = numberGenerator.GetGeneratedNumbers(); // Get the correct numbers from the generator
    }

    public void OnNumpadButtonPressed(string number)
    {
        Debug.Log("Button " + number + " pressed!");
        // Append the number to the input field
        inputField.text += number;

        // Check the code after each input
        if (inputField.text.Length == correctNumbers.Length)
        {
            CheckCode();
        }
    }

    public void ClearInput()
    {
        // Clear the input field
        inputField.text = "";
    }

    void CheckCode()
    {
        // Check if the input matches the correct numbers
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (inputField.text[i].ToString() != correctNumbers[i].ToString())
            {
                Debug.Log("Incorrect code!");
                ClearInput(); // Clear the input if the code is wrong
                return;
            }
        }

        // If all numbers match, unlock the box
        UnlockBox();
    }

    void UnlockBox()
    {
        Debug.Log("Lockbox opened!");
    }
}
