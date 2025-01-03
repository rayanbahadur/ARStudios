using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyAssignment : MonoBehaviour
{
    [SerializeField] private CamRotation securityCamera;
    [SerializeField] private Transform keycard;
    // Start is called before the first frame update
    void Start()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy");
        if (difficulty == "Easy")
        {
            securityCamera.secondsToRot = (float)(securityCamera.secondsToRot * 1.2);
            keycard.localPosition = new Vector3(-0.8f, -1.28f, 1.96f);

        }
        else if (difficulty == "Medium")
        {
            keycard.localPosition = new Vector3(1.57f, -1.23f, -1.13f);
        }
        else
        {
            securityCamera.secondsToRot = (float)(securityCamera.secondsToRot * 0.6);
            keycard.localPosition = new Vector3(-1.53f, -1.28f, -2.13f);
        }
    }
}
