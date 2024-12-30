using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private TMP_Text musicTextValue = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private readonly float defaultAudio = 0.5f;


    [Header("Sound Effects Settings")]
    [SerializeField] private TMP_Text soundEffectsTextValue = null;
    [SerializeField] private Slider soundEffectsSlider = null;

    [Header("Captions Settings")]
    [SerializeField] private Toggle captionsToggle = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    private int levelToLoad;
    [SerializeField] private Button[] levelButtons = null;

    [Header("Continue Game")]
    [SerializeField] private Button continueButton;
    [SerializeField] private CheckpointLoader checkpointLoader;




    private void Start()
    {
        // Load saved settings or set default values
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 0.5f);
        captionsToggle.isOn = PlayerPrefs.GetInt("Captions", 0) == 1;

        // Update UI and audio sources based on slider values
        SetMusic();
        SetSoundEffect();


        // Bind captions toggle directly to PlayerPrefs
        captionsToggle.onValueChanged.AddListener(value => PlayerPrefs.SetInt("Captions", value ? 1 : 0));
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = int.Parse(PlayerPrefs.GetString("SavedLevel"));

        }
        else
        {
            levelToLoad = 1;
        }
        SetButtonInteractivity();
    }

    private void SetButtonInteractivity()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i + 1) <= levelToLoad;
            int levelIndex = i + 1;
            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() => SceneManager.LoadScene(levelIndex));
        }

    }

    public void NewGameDialogYes()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if there is a next level
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetString("SavedLevel", currentSceneIndex.ToString());
            // Load the next level
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    public void ContinueGame()
    {
        // check if CheckpointData playerprefs has value, if so load, if not, check SavedLevel playprefs then load the level, if none disable button
        if (PlayerPrefs.HasKey("CheckpointData"))
        {
            if (checkpointLoader != null)
            {
                checkpointLoader.LoadCheckpoint();
            }
        }
        else if (PlayerPrefs.HasKey("SavedLevel")){
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(currentSceneIndex);
            }
        }
        else
        {
            if (continueButton != null)
            {
                continueButton.interactable = false;
            }
        }
    }

    public void SetDifficulty(string DifficultyValue) {
        PlayerPrefs.SetString("Difficulty", DifficultyValue);
        StartCoroutine(ConfirmationBox());
    }

    public void SetMusic()
    {
        musicTextValue.text = musicSlider.value.ToString("0.0");
    }

    public void SetSoundEffect()
    {
        soundEffectsTextValue.text = soundEffectsSlider.value.ToString("0.0");
    }


    public void AudioApply()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundEffectVolume", soundEffectsSlider.value);
        PlayerPrefs.SetInt("Captions", captionsToggle.isOn ? 1 : 0);

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            musicSlider.value = soundEffectsSlider.value = defaultAudio;
            musicTextValue.text = soundEffectsTextValue.text = defaultAudio.ToString("0.0");
            captionsToggle.isOn = false;
            AudioApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

}
