using UnityEngine;

public class SlidingPuzzleManager : MonoBehaviour
{
    public TargetTile[] targetTiles; // Array of all the target tiles
    public GameObject hatch; // Reference to the hatch GameObject
    [SerializeField] private PlayerProgress playerProgress;


    private bool puzzleCompleted = false;
    private AudioSource audioSource;
    private bool hasProgressBeenAdded = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!puzzleCompleted && CheckAllTilesOccupied())
        {
            puzzleCompleted = true;
            OpenHatch();
        }
    }

    private bool CheckAllTilesOccupied()
    {
        // Check if all target tiles are occupied
        foreach (TargetTile tile in targetTiles)
        {
            if (!tile.IsOccupied)
            {
                return false; // Return false if any tile is not occupied
            }
        }
        return true; // All tiles are occupied
    }

    private void OpenHatch()
    {
        Debug.Log("Puzzle Completed! Opening hatch...");

        float soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 1.0f);

        // Play the unlock sound
        if (audioSource != null)
        {
            audioSource.volume = soundEffectVolume;
            audioSource.Play();
        }

        hatch.SetActive(false);
        if (!hasProgressBeenAdded)
        {
            playerProgress.AddProgress(100);
        }
    }
}
