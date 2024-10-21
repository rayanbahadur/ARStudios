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

    private GameObject currentHandItem; // To keep track of the current item in hand

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
        Items.Add(item);
        ListItems();    
    }

    public void Remove(Item item) 
    {
        Items.Remove(item);
        ListItems();
    }

    public void ListItems()
    {
        for (int i = 0; i < ItemContent.childCount; i++)
        {
            Transform slot = ItemContent.GetChild(i);
            var itemName = slot.Find("ItemName").GetComponent<Text>();
            var itemIcon = slot.Find("ItemIcon").GetComponent<Image>();

            if (i < Items.Count)
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
            if (slotIndex <= Items.Count)
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

    private void RemoveItemFromHand()
    {
        // Destroy the current item in hand if there is one
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            currentHandItem = null;
            Debug.Log("Current hand item removed.");
        }
    }

    //private void DisplayItemInHand(Item item)
    //{
    //    // Destroy the current item in hand if there is one
    //    if (currentHandItem != null)
    //    {
    //        Destroy(currentHandItem);
    //        Debug.Log("Current hand item destroyed.");
    //    }

    //    // Instantiate the new item and set it as a child of the hand position
    //    currentHandItem = Instantiate(item.itemPrefab, HandPosition);
    //    currentHandItem.transform.localPosition = Vector3.zero; 
    //    currentHandItem.transform.localRotation = Quaternion.identity;
    //    Debug.Log($"Item displayed in hand: {item.itemName}");
    //}

    private void DisplayItemInHand(Item item)
    {
        // Destroy the current item in hand if there is one
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            Debug.Log("Current hand item destroyed.");
        }

        // Instantiate the new item and set it as a child of the hand position
        currentHandItem = Instantiate(item.itemPrefab, HandPosition);
        currentHandItem.transform.localPosition = Vector3.zero;
        currentHandItem.transform.localRotation = Quaternion.identity;

        // Ensure the item is visible
        currentHandItem.SetActive(true);

        // Debug information
        Debug.Log($"Item displayed in hand: {item.itemName}");
        Debug.Log($"Item position: {currentHandItem.transform.position}");
        Debug.Log($"Item rotation: {currentHandItem.transform.rotation}");
        Debug.Log($"Item scale: {currentHandItem.transform.localScale}");
        Debug.Log($"Item layer: {LayerMask.LayerToName(currentHandItem.layer)}");

        // Check if MeshRenderer is enabled
        MeshRenderer meshRenderer = currentHandItem.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Debug.Log($"MeshRenderer is present and enabled: {meshRenderer.enabled}");
        }
        else
        {
            Debug.LogError("MeshRenderer component is missing on the hammer prefab.");
        }

        // Check if the item is active
        Debug.Log($"Item active state: {currentHandItem.activeSelf}");
    }

}
