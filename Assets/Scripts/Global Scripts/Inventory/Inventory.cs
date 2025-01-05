using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public Transform HandPosition; // Reference to the hand position prefab

    public GameObject currentHandItem; // To keep track of the current item in hand

    private void Awake()
    {
        Instance = this;
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < ItemContent.childCount; i++)
        {
            Transform slot = ItemContent.GetChild(i);
            var itemName = slot.Find("ItemName").GetComponent<Text>();
            var itemIcon = slot.Find("ItemIcon").GetComponent<Image>();

            itemName.enabled = false;
            itemIcon.enabled = false;
        }
    }

    public void Add(Item item)
    {
        // Find the first null slot or add to the end if no null slots are found
        int index = Items.IndexOf(null);
        if (index != -1)
        {
            Items[index] = item;
        }
        else
        {
            Items.Add(item);
        }
        ListItems();
    }

    public void Remove(Item item)
    {
        int index = Items.IndexOf(item);
        if (index != -1)
        {
            Items[index] = null; // Set the item to null instead of removing it
            ListItems();
        }
    }

    public void ListItems()
    {
        for (int i = 0; i < ItemContent.childCount; i++)
        {
            Transform slot = ItemContent.GetChild(i);
            var itemName = slot.Find("ItemName").GetComponent<Text>();
            var itemIcon = slot.Find("ItemIcon").GetComponent<Image>();

            if (i < Items.Count && Items[i] != null)
            {
                Item item = Items[i];
                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
                itemName.enabled = true;
                itemIcon.enabled = true;
            }
            else
            {
                itemName.text = "";
                itemIcon.sprite = null;
                itemName.enabled = false;
                itemIcon.enabled = false;
            }
        }
    }

    public void SelectSlot(int slotIndex)
    {
        if (slotIndex >= 1 && slotIndex <= ItemContent.childCount)
        {
            if (slotIndex <= Items.Count && Items[slotIndex - 1] != null)
            {
                Item selectedItem = Items[slotIndex - 1];
                Debug.Log($"Slot {slotIndex} selected with item: {selectedItem.itemName}");
                DisplayItemInHand(selectedItem);
            }
            else
            {
                Debug.Log($"Slot {slotIndex} is empty. Removing current hand item.");
                RemoveItemFromHand();
            }
        }
        else
        {
            Debug.LogWarning($"Invalid slot index: {slotIndex}");
        }
    }

    public void RemoveItemFromHand()
    {
        // Destroy the current item in hand if there is one
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            currentHandItem = null;
            Debug.Log("Current hand item removed.");
        }
    }

    private void DisplayItemInHand(Item item)
    {
        // Destroy the current item in hand if there is one
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            Debug.Log("Current hand item destroyed.");
        }

        // Instantiate the new item
        currentHandItem = Instantiate(item.itemPrefab);

        // Set the parent to HandPosition
        currentHandItem.transform.SetParent(HandPosition, false);

        // Check if the item is a Potion
        if (item.itemName.Contains("Potion"))
        {
            // Assign different local position and rotation potions
            currentHandItem.transform.localPosition = new Vector3(0.00300000003f, -0.130999997f, 0.0359999985f);
            currentHandItem.transform.localRotation = Quaternion.Euler(88.0161438f, 179.999725f, 179.999725f);

        }
        else if (item.itemName == "Sword")
        {
            // Assign different local position and rotation for the sword
            currentHandItem.transform.localPosition = new Vector3(0.003f, -0.131f, 0.036f);
            currentHandItem.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);
        }
        else
        {
            // Reset local position, rotation, and scale for other items
            currentHandItem.transform.localPosition = Vector3.zero;
            currentHandItem.transform.localRotation = Quaternion.identity;
        }

        // Disable or remove Mesh Collider
        MeshCollider meshCollider = currentHandItem.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            if (!meshCollider.convex)
            {
                Debug.Log("Disabling concave Mesh Collider.");
                Destroy(meshCollider); // Remove the Mesh Collider completely
            }
        }

        // Ensure the item is visible
        currentHandItem.SetActive(true);

        Debug.Log($"Item displayed in hand: {item.itemName}");
    }

    // @Params effect: The effect of the potion
    // @Params itemName: The name of the potion item
    public void DrinkingAnimation(string effect, string itemName)
    {
        if (currentHandItem != null)
        {
            StartCoroutine(AnimateDrinking(effect, itemName));
        }
    }

    private IEnumerator AnimateDrinking(string effect, string itemName)
    {
        Transform itemTransform = currentHandItem.transform;
        Vector3 initialPosition = itemTransform.localPosition;
        Quaternion initialRotation = itemTransform.localRotation;
        Vector3 targetPosition = new Vector3(-0.150999993f, -0.488999993f, 0.130999997f);
        Quaternion targetRotation = Quaternion.Euler(86.301178f, 245.948456f, 327.900635f);

        float time = 0f;
        float animationSpeed = 2f;

        // Move to the drinking position
        while (time < 1f)
        {
            time += Time.deltaTime * animationSpeed;
            itemTransform.localPosition = Vector3.Lerp(initialPosition, targetPosition, time);
            itemTransform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, time);
            yield return null;
        }

        // Apply the effect
        switch (effect)
        {
            case "CurePoison":
                PoisonRiddle.Instance.CurePoison();
                break;
            case "GravityPotion":
                GravityRecipePaperInteraction.Instance.ApplyGravityEffect();
                break;
        }

        // Wait for a short duration to simulate drinking time
        yield return new WaitForSeconds(1f);

        // Remove item from inventory and hand
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemName == itemName)
            {
                Remove(Items[i]);
                break;
            }
        }
        RemoveItemFromHand();

        Debug.Log("Potion consumed: " + effect);
    }
} 
