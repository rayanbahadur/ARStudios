using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject _AI;
    [SerializeField] private AudioSource audioSourceSE;
    [SerializeField] private AudioSource audioSourceM;

    void Start()
    {
        audioSourceSE.volume = PlayerPrefs.GetFloat("SoundEffectVolume",1.0f);
        audioSourceM.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
    }
    public void RevealAI()
    {
        _AI.SetActive(true);
    }
}
