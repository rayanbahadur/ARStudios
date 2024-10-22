using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayerInput : MonoBehaviour
{
    void Update()
    {
        for (int i = 1; i <= 7; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int slotIndex = i;
                Debug.Log($"Key {i} pressed. Selecting slot {slotIndex}.");
                Inventory.Instance.SelectSlot(slotIndex);
            }
        }
    }
}