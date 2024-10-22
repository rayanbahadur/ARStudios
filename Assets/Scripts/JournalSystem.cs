using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalSystem : MonoBehaviour
{
    public TMP_Text journalText;
    private HashSet<string> cluesSet = new HashSet<string>();
    //private List<string> cluesList = new List<string>();  // To preserve order

    public void AddClue(string clue)
    {
        if (cluesSet.Add(clue))  // Add only if unique
        {
            //cluesList.Add(clue);  // Maintain insertion order
            journalText.text += "\n" + clue;  // Append to existing text
        }
    }
    public void ClearJournal()
    {
        journalText.text = "";
        cluesSet.Clear();
        //cluesList.Clear();
    }
}
