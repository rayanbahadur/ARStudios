using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    private const int GRID_SIZE = 3;
    private Item[,] grid; // 3x3 grid

    public event Action onCraftingGridChanged; // Event to notify when the crafting grid has changed
    public Item outputItem; // The item that will be crafted
    [SerializeField] private PlayerProgress playerProgress;

    public CraftingSystem()
    {
        grid = new Item[GRID_SIZE, GRID_SIZE]; // Initialize the grid
    }

    public Item getItem(int x, int y) => grid[x, y]; // Get the item at the specified position

    public void setItem(int x, int y, Item item)
    {
        grid[x, y] = item; // Set the item at the specified position
        onCraftingGridChanged?.Invoke(); // Notify that the crafting grid has changed
        UpdateOutput(); // Update the output item
    }

    public void removeItem(int x, int y)
    {
        grid[x, y] = null; // Remove the item at the specified position
        onCraftingGridChanged?.Invoke(); // Notify that the crafting grid has changed
        UpdateOutput(); // Update the output item
    }

    public void ClearGrid()
    {
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int y = 0; y < GRID_SIZE; y++)
            {
                grid[x, y] = null;
            }
        }
        onCraftingGridChanged?.Invoke();
        UpdateOutput();
    }

    public void ClaimOutput()
    {
        if (outputItem != null)
        {
            // Add the crafted item to the inventory
            Inventory.Instance.Add(outputItem);

            // Clear the crafting grid
            ClearGrid();
        }
    }

    // Recipe Matching
    private void UpdateOutput()
    {
        outputItem = GetRecipeOutput(); // Get the output item based on the current grid configuration
        onCraftingGridChanged.Invoke(); // Notify that the output item has changed
    }

    private Item GetRecipeOutput()
    {
        // Define recipes
        var poisonCurePotion = Resources.Load<Item>("CraftedItemsPrefabs/PoisonCurePotion");

        var poisonCureRecipe = new string[,] { // grid[column, row]
            { null, null, null }, 
            { "PotionFlask", "BluePotion", "RedPotion" }, // Coordinates [1,0], [1,1], [1,2]
            { null, null, null }
        };

        var gravityPotion = Resources.Load<Item>("CraftedItemsPrefabs/GravityPotion");

        var gravityPotionRecipe = new string[,] {
            { "BluePotion", null, null },
            { null, "FatBluePotion", null },
            { null, null, "PotionFlask" }
        };

        if (MatchRecipe(poisonCureRecipe))
        {
            if (playerProgress.currentProgress < 80)
            {
            playerProgress.SetProgress(80);
            playerProgress.SetTaskText("Drink the potion to cure the poison.");
            }
            
            return poisonCurePotion;
        } 
        if (MatchRecipe(gravityPotionRecipe)) return gravityPotion;

        return null;
    }

    private bool MatchRecipe(string[,] recipe)
    {
        // Check if the grid matches the recipe
        for (int x = 0; x < GRID_SIZE; x++)
        {
            // Check each cell in the recipe
            for (int y = 0; y < GRID_SIZE; y++)
            {
                // Log the current grid and recipe cell being compared
                string gridItemName = grid[x, y]?.itemName ?? "null";
                string recipeItemName = recipe[x, y] ?? "null";
                Debug.Log($"Comparing grid[{x}, {y}] ({gridItemName}) with recipe[{x}, {y}] ({recipeItemName})");

                // If the recipe cell is not null and the grid cell is either null or does not match the recipe cell
                if (recipe[x, y] != null && (grid[x, y] == null || grid[x, y].itemName != recipe[x, y]))
                {
                    Debug.Log($"Mismatch at grid[{x}, {y}]: grid item is {gridItemName}, recipe item is {recipeItemName}");
                    return false;
                }
            }
        }
        Debug.Log("Recipe matched successfully!");
        return true;
    }
}