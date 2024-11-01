using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource; // The AudioSource for background music
    public AudioSource[] soundEffectSources; // Array to hold multiple sound effect AudioSources


    private void Awake()
    {
        // Implementing Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    private void Start()
    {
        SetVolume(musicSource);
        SetSoundEffectVolumes();
    }


    public void SetVolume(AudioSource audioSource)
    {
        // Retrieve the saved volume levels and apply them
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f); // Default to 0.5 if not set
        float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.5f); // Default to 0.5 if not set

        // Depending on the type of audio source, set its volume
        if (audioSource.clip != null) // Ensure there is an audio clip
        {
            audioSource.volume = (audioSource == musicSource) ? musicVolume : soundEffectVolume;
        }
    }
    public void SetSoundEffectVolumes()
    {
        // Apply sound effect volume to all sound effect AudioSources
        float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.5f); // Default to 0.5 if not set

        foreach (var soundEffect in soundEffectSources)
        {
            if (soundEffect != null)
            {
                soundEffect.volume = soundEffectVolume;
            }
        }
    }

    public void PlaySoundEffect(AudioSource soundEffect)
    {
        if (soundEffect != null && soundEffect.clip != null)
        {
            soundEffect.Play();
        }
    }
}
