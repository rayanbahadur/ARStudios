using System.Collections;
using UnityEngine;
using TMPro;

public class RingingTextAnimation : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public float typingSpeed = 0.1f;
    private string[] messages = { "Brr..Brr..Brr...", "Ring... Ring...", "Redeem me!", "Can't you hear me?", "ARStudios calling!" };

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        while (true)
        {
            string message = messages[Random.Range(0, messages.Length)]; // Pick a random message
            displayText.text = ""; // Clear the text field initially

            foreach (char letter in message.ToCharArray())
            {
                displayText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(1f);
            displayText.text = ""; // Clear text before typing again
        }
    }
}
