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
    public string _newGameLevel;
    public string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;


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
    }


    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
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
