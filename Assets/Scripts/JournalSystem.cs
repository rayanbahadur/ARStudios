using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalSystem : MonoBehaviour
{
    public TMP_Text journalText;  // Reference to the TextMeshPro component for displaying the journal clues
    private List<string> clues = new List<string>();  // List to store clues as they are collected

    // This function adds a new clue to the journal
    public void AddClue(string clue)
    {
        clues.Add(clue);  // Add the clue to the list
        UpdateJournal();  // Update the journal display with the new clue
    }

    // Function to update the journal text with all the collected clues
    private void UpdateJournal()
    {
        //journalText.text = "";  // Clear the existing text

        // Loop through each clue in the list and append it to the journal text
        foreach (string clue in clues)
        {
            journalText.text += "\n"+ clue;  // Add some spacing between clues
        }
    }
}
