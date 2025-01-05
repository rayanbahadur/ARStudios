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

    private bool isSwiping = false; // Track if the sword is currently swiping

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
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            currentHandItem = null;
            Debug.Log("Current hand item removed.");
        }
    }

    private void DisplayItemInHand(Item item)
    {
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            Debug.Log("Current hand item destroyed.");
        }

        currentHandItem = Instantiate(item.itemPrefab);
        currentHandItem.transform.SetParent(HandPosition, false);

        if (item.itemName == "Sword")
        {
            currentHandItem.transform.localPosition = new Vector3(0.003f, -0.131f, -0.2f);
            currentHandItem.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);
        }
        else
        {
            currentHandItem.transform.localPosition = Vector3.zero;
            currentHandItem.transform.localRotation = Quaternion.identity;
        }

        MeshCollider meshCollider = currentHandItem.GetComponent<MeshCollider>();
        if (meshCollider != null && !meshCollider.convex)
        {
            Destroy(meshCollider);
        }

        currentHandItem.SetActive(true);

        Debug.Log($"Item displayed in hand: {item.itemName}");
    }

    private void Update()
    {
        // Trigger sword swipe if the held item is a sword and the key is pressed
        if (currentHandItem != null && Input.GetKeyDown(KeyCode.Q))
        {
            if (currentHandItem.name.Contains("Sword") && !isSwiping)
            {
                StartCoroutine(SwipeSword());
            }
        }
    }

    private System.Collections.IEnumerator SwipeSword()
    {
        isSwiping = true;

        Transform swordTransform = currentHandItem.transform;

        Quaternion initialRotation = swordTransform.localRotation;
        Quaternion intermediateRotation = Quaternion.Euler(0f, 0f, 0f); // Step 1: Rotate to (0, 0, 0)
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 80f);      // Step 2: Rotate to (0, 0, 80)

        float time = 0f;
        float swipeSpeed = 5f;

        // Step 1: Rotate to (0, 0, 0)
        while (time < 1f)
        {
            time += Time.deltaTime * swipeSpeed;
            swordTransform.localRotation = Quaternion.Lerp(initialRotation, intermediateRotation, time);
            yield return null;
        }

        time = 0f;

        // Step 2: Rotate to (0, 0, 80)
        while (time < 1f)
        {
            time += Time.deltaTime * swipeSpeed;
            swordTransform.localRotation = Quaternion.Lerp(intermediateRotation, targetRotation, time);
            yield return null;
        }

        time = 0f;

        // Step 3: Return to the initial position
        while (time < 1f)
        {
            time += Time.deltaTime * swipeSpeed;
            swordTransform.localRotation = Quaternion.Lerp(targetRotation, initialRotation, time);
            yield return null;
        }

        isSwiping = false;
    }

}
