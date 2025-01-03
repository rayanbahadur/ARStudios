using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{
    public CraftingSystem craftingSystem;
    public GameObject gridContainer;
    public GameObject outputSlot; 
    public GameObject craftingUI;

    private Dictionary<string, Image> gridSlots;
    private Image outplotSlotGridImage; // Grid image component for the output slot
    private Image outputSlotItemImage; // Item image component for the output slot
    private Vector2Int selectedGridSlot = new Vector2Int(-1, -1); // Track the currently selected grid slot
    private Dictionary<(Item item, int slotIndex), Vector2Int> placedItems = new Dictionary<(Item item, int slotIndex), Vector2Int>(); // Track the items placed in the grid

    void Start()
    {
        // Initialize the grid slots dictionary
        gridSlots = new Dictionary<string, Image>();

        // Find all grid slot images and add them to the dictionary
        foreach (Transform child in gridContainer.transform)
        {
            if (child.name.StartsWith("grid_"))
            {
                Image image = child.GetComponentInChildren<Image>();
                Button button = image.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnGridSlotClicked(child.name));
                }
                Image itemImage = child.Find("itemImage").GetComponent<Image>();
                itemImage.enabled = false; // Hide the itemImage initially
                gridSlots[child.name] = itemImage;
                // Debug.Log($"Added {child.name} to gridSlots dictionary.");
            }
        }


        // Get the output slot images
        outplotSlotGridImage = outputSlot.GetComponentInChildren<Image>();
        outputSlotItemImage = outputSlot.transform.Find("itemImage").GetComponent<Image>();
        outputSlotItemImage.enabled = false; // Hide the itemImage initially

        // Add listener to the output slot button
        Button outputSlotButton = outplotSlotGridImage.GetComponent<Button>();
        if (outputSlotButton != null)
        {
            outputSlotButton.onClick.AddListener(OnOutputSlotClicked);
        }

        // Subscribe to the crafting grid changed event
        craftingSystem.onCraftingGridChanged += UpdateUI;

        // Hide the crafting UI initially
        craftingUI.SetActive(false);
    }

    void OnDestroy()
    {
        // Unsubscribe from the crafting grid changed event
        craftingSystem.onCraftingGridChanged -= UpdateUI;
    }

    void Update()
    {
        // Toggle the crafting UI with the 'C' key
        if (Input.GetKeyDown(KeyCode.C))
        {
            craftingUI.SetActive(!craftingUI.activeSelf);

            if (craftingUI.activeSelf)
            {
                // Unlock and show the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("Crafting UI opened.");
            }
            else
            {
                // Lock and hide the cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log("Crafting UI closed.");

                // Return all items in the grid to the inventory
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        RemoveItemFromGrid(x, y);
                    }
                }

                // Clear the crafting grid
                craftingSystem.ClearGrid();
                selectedGridSlot = new Vector2Int(-1, -1); // Reset the selected grid slot
            }
        }

        // Handle placing/removing items based on the key pressed
        if (craftingUI.activeSelf && selectedGridSlot.x != -1 && selectedGridSlot.y != -1)
        {
            for (int i = 1; i <= CustomPlayerInput.inventoryToolbarMaxSize; i++) // Only allow key presses up to the size of the toolbar
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    Debug.Log($"Key {i} pressed.");
                    PlaceOrRemoveItem(i - 1);
                }
            }
        }
    }

    private void OnGridSlotClicked(string slotName)
    {
        int x = int.Parse(slotName.Split('_')[1]);
        int y = int.Parse(slotName.Split('_')[2]);
        selectedGridSlot = new Vector2Int(x, y);
        Debug.Log($"Selected grid slot at position ({x}, {y})");
    }

    private void OnOutputSlotClicked()
    {
        Debug.Log("Output slot clicked.");
        selectedGridSlot = new Vector2Int(-1, -1); // Reset the selected grid slot
        // Handle output slot click logic here if needed
        // --- Should replace most recently placed item with output item ---
        // -- Then it should clear the UI and add the output item to the inventory --

    }

    private void PlaceOrRemoveItem(int slotIndex)
    {
        int x = selectedGridSlot.x;
        int y = selectedGridSlot.y;

        Debug.Log($"Placing/removing item in grid slot at position ({x}, {y})");

        string slotName = $"grid_{x}_{y}";
        if (slotIndex < Inventory.Instance.Items.Count)
        {
            Item item = Inventory.Instance.Items[slotIndex];

            // Check if the grid slot is already occupied
            if (craftingSystem.getItem(x, y) != null)
            {
                Debug.Log($"Grid slot ({x}, {y}) is already occupied. Removing the item first.");
                RemoveItemFromGrid(x, y);
            }

            // Place the item in the new grid slot
            craftingSystem.setItem(x, y, item);
            placedItems[(item, slotIndex)] = new Vector2Int(x, y);
            Inventory.Instance.Remove(item);
            Inventory.Instance.RemoveItemFromHand(); // Remove the item from the player's hand
            Debug.Log($"Added item {item.itemName} to grid slot ({x}, {y}) and removed from inventory slot {slotIndex}");

            // Update the grid slot image
            if (gridSlots.ContainsKey(slotName))
            {
                Image itemImage = gridSlots[slotName];
                itemImage.sprite = item.icon;
                itemImage.color = Color.white;
                itemImage.enabled = true; // Show the itemImage
            }
        }
        else
        {
            RemoveItemFromGrid(x, y);
        }
    }

    private void RemoveItemFromGrid(int x, int y)
    {
        string slotName = $"grid_{x}_{y}";
        Item item = craftingSystem.getItem(x, y);

        if (item != null)
        {
            craftingSystem.removeItem(x, y);
            Debug.Log($"Removed item {item.itemName} from grid slot ({x}, {y})");

            // Clear the grid slot image
            if (gridSlots.ContainsKey(slotName))
            {
                Image itemImage = gridSlots[slotName];
                itemImage.sprite = null;
                itemImage.color = Color.clear;
                itemImage.enabled = false; // Hide the itemImage
            }

            // Return the item to the inventory
            Inventory.Instance.Add(item);
            Debug.Log($"Returned item {item.itemName} to the inventory");
        }
    }
    private void UpdateUI()
    {
        // Clear the placed items dictionary
        placedItems.Clear();
        Debug.Log("Cleared placed items dictionary.");

        // Update the grid slots
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                string slotName = $"grid_{x}_{y}";
                if (gridSlots.ContainsKey(slotName))
                {
                    Item item = craftingSystem.getItem(x, y);
                    Image itemImage = gridSlots[slotName];
                    if (item != null)
                    {
                        itemImage.sprite = item.icon;
                        itemImage.color = Color.white;
                        itemImage.enabled = true; // Show the itemImage

                        // Find the slot index for the item
                        int slotIndex = -1;
                        for (int i = 0; i < Inventory.Instance.Items.Count; i++)
                        {
                            if (Inventory.Instance.Items[i] == item)
                            {
                                slotIndex = i;
                                break;
                            }
                        }

                        if (slotIndex != -1)
                        {
                            placedItems[(item, slotIndex)] = new Vector2Int(x, y); // Track the placed item
                            Debug.Log($"Updated grid slot {slotName} with item {item.itemName} at inventory slot {slotIndex}");
                        }
                    }
                    else
                    {
                        itemImage.sprite = null;
                        itemImage.color = Color.clear;
                        itemImage.enabled = false; // Hide the itemImage
                        Debug.Log($"Cleared grid slot {slotName}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Grid slot {slotName} not found in gridSlots dictionary.");
                }
            }
        }

        // Update the output slot
        if (craftingSystem.outputItem != null)
        {
            outputSlotItemImage.sprite = craftingSystem.outputItem.icon;
            outputSlotItemImage.color = Color.white;
            outputSlotItemImage.enabled = true; // Show the itemImage
            Debug.Log($"Updated output slot with item {craftingSystem.outputItem.itemName}");
        }
        else
        {
            outputSlotItemImage.sprite = null;
            outputSlotItemImage.color = Color.clear;
            outputSlotItemImage.enabled = false; // Hide the itemImage
            Debug.Log("Cleared output slot");
        }

        // Check if the current grid configuration matches any recipe
        if (craftingSystem.outputItem != null)
        {
            Debug.Log("Recipe matched: " + craftingSystem.outputItem.itemName);
        }
        else
        {
            Debug.Log("No matching recipe found.");
        }
    }
}