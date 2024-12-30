using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vocals : MonoBehaviour
{
    private AudioSource source;

    public static Vocals instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.5f);
    }

    public void Speak(AudioObject clip)
    {
        if (source.isPlaying) source.Stop();

        source.PlayOneShot(clip.clip);

        if (PlayerPrefs.GetInt("Captions") == 1)
        {
            SubtitlesUI.instance.SetSubtitle(clip.subtitle, clip.clip.length);
        }
    }
}

/*
 * Assignment:
 * public AudioObject clipToPlay
 * Vocals.instance.Speak(clipToPlay)
 */