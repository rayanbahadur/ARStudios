using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject AI;
    [SerializeField] private AudioSource audioSourceSE;
    [SerializeField] private GameObject EndGameUI;

    void Start()
    {
        audioSourceSE.volume = PlayerPrefs.GetFloat("SoundEffectVolume",1.0f);
        EndGameUI.SetActive(false);
    }
    public void RevealAI()
    {
        AI.SetActive(true);
    }

    public void RevealEndGameUI()
    {
        EndGameUI.SetActive(true);
    }
}
