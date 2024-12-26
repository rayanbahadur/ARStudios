using UnityEngine;

public class TargetTile : MonoBehaviour
{
    public string requiredTag; // Tag of the crate that matches this tile
    private bool isOccupied = false; // Whether the correct crate is on this tile

    public bool IsOccupied => isOccupied; // Property to check the state

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the tile is the correct crate
        if (other.CompareTag(requiredTag))
        {
            isOccupied = true;
            Debug.Log($"Correct crate placed on tile: {gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset state if the crate leaves the tile
        if (other.CompareTag(requiredTag))
        {
            isOccupied = false;
            Debug.Log($"Crate removed from tile: {gameObject.name}");
        }
    }
}
