using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RaycastOutlineUtility
{
    public static void CheckIfPlayerIsLookingAtItem(Camera playerCamera, RectTransform crosshairRectTransform, GameObject item, Outline outline)
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera is not assigned or tagged as MainCamera.");
            return;
        }

        if (crosshairRectTransform == null)
        {
            Debug.LogError("Crosshair RectTransform is not assigned.");
            return;
        }
        
        Vector2 crosshairPosition = crosshairRectTransform.position; // Use the actual position of the crosshair
        Ray ray = playerCamera.ScreenPointToRay(crosshairPosition); // Cast ray from the crosshair position
        RaycastHit[] hits = Physics.RaycastAll(ray);

        bool isLookingAtItem = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == item)
            {
                isLookingAtItem = true;
                break;
            }
        }

        if (isLookingAtItem)
        {
            if (outline != null)
            {
                outline.enabled = true; // Enable the highlight
                Debug.Log("Player is looking at the object.");
            }
        }
        else
        {
            if (outline != null)
            {
                outline.enabled = false; // Disable the highlight
            }
        }
    }
}