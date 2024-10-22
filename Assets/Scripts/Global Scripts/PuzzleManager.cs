using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{

    public GameObject[] puzzlePieceHolder1;
    public GameObject[] puzzlePieceHolder2;
    public GameObject[] puzzlePieceHolder3;
    public JournalSystem journalSystem;
    public UnityEvent chickenDinner;

    private int blueCounter;
    private int purpleCounter;
    private int redCounter;

    public void Winner()
    {
        if (puzzlePieceHolder1[0].activeInHierarchy && puzzlePieceHolder2[1].activeInHierarchy && puzzlePieceHolder3[2].activeInHierarchy)
        {
            Debug.Log("You win!!");
            journalSystem.AddClue("VALVE: 2");
            //chickenDinner.Invoke();
        }
    }

    public void BlueATM()
    {
        Debug.Log("BLUE");
        blueCounter++;

        if (blueCounter >= 3)
        {
            blueCounter = 0;
        }

        switch (blueCounter)
        {
            case 0:
                puzzlePieceHolder1[0].SetActive(true);
                puzzlePieceHolder1[1].SetActive(false);
                puzzlePieceHolder1[2].SetActive(false);
                break;
            case 1:
                puzzlePieceHolder1[0].SetActive(false);
                puzzlePieceHolder1[1].SetActive(true);
                puzzlePieceHolder1[2].SetActive(false);
                break;
            case 2:
                puzzlePieceHolder1[0].SetActive(false);
                puzzlePieceHolder1[1].SetActive(false);
                puzzlePieceHolder1[2].SetActive(true);
                break;
        }

        Winner(); // Check for a win after updating
    }

    public void purpleATM()
    {
        Debug.Log("PURPLE");
        purpleCounter++;

        if (purpleCounter >= 3)
        {
            purpleCounter = 0;
        }

        switch (purpleCounter)
        {
            case 0:
                puzzlePieceHolder2[0].SetActive(true);
                puzzlePieceHolder2[1].SetActive(false);
                puzzlePieceHolder2[2].SetActive(false);
                break;
            case 1:
                puzzlePieceHolder2[0].SetActive(false);
                puzzlePieceHolder2[1].SetActive(true);
                puzzlePieceHolder2[2].SetActive(false);
                break;
            case 2:
                puzzlePieceHolder2[0].SetActive(false);
                puzzlePieceHolder2[1].SetActive(false);
                puzzlePieceHolder2[2].SetActive(true);
                break;
        }

        Winner(); // Check for a win after updating
    }

    public void redATM()
    {
        Debug.Log("RED");
        redCounter++;

        if (redCounter >= 3)
        {
            redCounter = 0;
        }

        switch (redCounter)
        {
            case 0:
                puzzlePieceHolder3[0].SetActive(true);
                puzzlePieceHolder3[1].SetActive(false);
                puzzlePieceHolder3[2].SetActive(false);
                break;
            case 1:
                puzzlePieceHolder3[0].SetActive(false);
                puzzlePieceHolder3[1].SetActive(true);
                puzzlePieceHolder3[2].SetActive(false);
                break;
            case 2:
                puzzlePieceHolder3[0].SetActive(false);
                puzzlePieceHolder3[1].SetActive(false);
                puzzlePieceHolder3[2].SetActive(true);
                break;
        }

        Winner(); // Check for a win after updating
    }

}