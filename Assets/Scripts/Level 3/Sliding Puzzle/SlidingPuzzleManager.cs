using UnityEngine;

public class SlidingPuzzleManager : MonoBehaviour
{
    public TargetTile[] targetTiles; // Array of all the target tiles
    public GameObject hatch; // Reference to the hatch GameObject

    private bool puzzleCompleted = false;

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
        hatch.SetActive(false);
    }
}
