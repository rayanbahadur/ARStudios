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
            // Remove items used in the crafting from the grid
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    if (grid[x, y] != null)
                    {
                        Inventory.Instance.Remove(grid[x, y]);
                        grid[x, y] = null;
                    }
                }
            }

            // Add the crafted item to the inventory
            Inventory.Instance.Add(outputItem);

            // Clear the grid
            ClearGrid();
        }
    }

    // Recipe Matching
    private void UpdateOutput()
    {
        outputItem = GetRecipeOutput(); // Get the output item based on the current grid configuration
    }

    private Item GetRecipeOutput()
    {
        // Define recipes
        var poisonCurePotion = Resources.Load<Item>("CraftedItems/PoisonCurePotion");

        var poisonCureRecipe = new string[,] {
            { null, "RedPotion", null },
            { null, "BluePotion", null },
            { null, null, null }
        };

        if (MatchRecipe(poisonCureRecipe)) return poisonCurePotion;

        return null;
    }

    private bool MatchRecipe(string[,] recipe)
    {
        // Check if the grid matches the recipe
        for (int x = 0; x < recipe.GetLength(0); x++)
        {
            // Check each cell in the recipe
            for (int y = 0; y < recipe.GetLength(1); y++)
            {
                // If the recipe cell is not null and the grid cell is either null or does not match the recipe cell
                if (recipe[x, y] != null && (grid[x, y] == null || grid[x, y].itemName != recipe[x, y]))
                    return false;
            }
        }
        return true;
    }
}