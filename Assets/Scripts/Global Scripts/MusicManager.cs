using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClip[]))]

public class MusicManager : MonoBehaviour
{
    [Header("Music Tracks")]
    [SerializeField] private AudioClip[] musicTracks;

    [Header("Settings")]
    [SerializeField] private AudioSource audioSource;
    public float startDelay = 3f; // Delay before the music starts, default to 3 seconds

    private int musicIndex = 0; // Holds the current index of the clip being played

    void Start()
    {
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic() { 
        yield return new WaitForSeconds(startDelay);

        PlayNextTrack();
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;
        audioSource.clip = musicTracks[musicIndex];
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        audioSource.Play();

        StartCoroutine(WaitTrackEnd());
    }

    private IEnumerator WaitTrackEnd() {
        yield return new WaitForSeconds(audioSource.clip.length);

        // Move to the next track, loop back to 0 if at the end
        musicIndex = (musicIndex + 1) % musicTracks.Length;

        PlayNextTrack();
    }

    // Call this function to change the music
    public void SetMusicTracks(AudioClip[] newTracks, bool playImmediately = false)
    {
        musicTracks = newTracks;
        musicIndex = 0;

        if (playImmediately && musicTracks.Length > 0)
        {
            StopAllCoroutines();
            PlayNextTrack();
        }
    }

    public void applyVolumeNow()
    {
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }
}
