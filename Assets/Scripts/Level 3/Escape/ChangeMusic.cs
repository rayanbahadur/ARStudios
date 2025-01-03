using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;
    [SerializeField] AudioClip[] audioClips;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            musicManager.SetMusicTracks(audioClips, true);
        }
    }
}
