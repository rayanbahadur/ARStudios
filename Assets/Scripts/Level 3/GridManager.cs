using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cratePrefab;     // Prefab for crates
    [SerializeField] private GameObject tilePrefab;      // Prefab for floor tiles
    [SerializeField] private Transform gridParent;       // Parent to organize the grid in the Hierarchy

    [Header("Grid Settings")]
    private int gridSize = 3;                            // Default grid size for easy mode
    private float tileSpacing = 1.1f;                    // Spacing between tiles

    void Start()
    {
        // For testing, set difficulty here
        SetDifficulty("Easy");
        GenerateGrid();
    }

    public void SetDifficulty(string difficulty)
    {
        // Adjust grid size based on difficulty
        switch (difficulty)
        {
            case "Easy":
                gridSize = 3; // 3x3 grid
                break;
            case "Medium":
                gridSize = 4; // 4x4 grid
                break;
            default:
                gridSize = 3; // Default to 3x3
                break;
        }

        Debug.Log($"Difficulty set to {difficulty}, Grid size: {gridSize}x{gridSize}");
    }

    private void GenerateGrid()
    {
        // Clear any existing grid
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // Generate tiles and crates
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // Calculate position for each tile and crate
                Vector3 position = new Vector3(x * tileSpacing, 0, z * tileSpacing);

                // Instantiate floor tile
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, gridParent);
                tile.name = $"Tile_{x}_{z}";

                // Instantiate crate only if not the last tile
                if (!(x == gridSize - 1 && z == gridSize - 1)) // Leave one space empty
                {
                    GameObject crate = Instantiate(cratePrefab, position, Quaternion.identity, gridParent);
                    crate.name = $"Crate_{x}_{z}";
                }
            }
        }
    }
}
