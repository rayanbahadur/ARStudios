using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    public Item Item;

    [SerializeField]
    public float pickupRange = 3.5f; 

    private Transform playerTransform;
    private Camera playerCamera;
    private RectTransform crosshairRectTransform;
    private Outline outline;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = Camera.main; // Assuming the player's camera is tagged as "MainCamera"

        // Find the crosshair in the MainCanvas
        GameObject crosshair = GameObject.Find("MainCanvas/Crosshair");
        if (crosshair != null)
        {
            crosshairRectTransform = crosshair.GetComponent<RectTransform>();
            Debug.Log("Crosshair found.");
        }
        else
        {
            Debug.LogError("Crosshair not found in the MainCanvas.");
        }

        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
            Debug.Log("Outline component found and disabled.");
        }
        else
        {
            Debug.LogError("Outline component not found on the object.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            TryPickup();
        }

        if (playerTransform != null && outline != null)
        {
            float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            if (distanceToPlayer <= pickupRange)
            {
                RaycastOutlineUtility.CheckIfPlayerIsLookingAtItem(playerCamera, crosshairRectTransform, gameObject, outline);
            }
            else
            {
                outline.enabled = false; // Disable the highlight
            }
        }
    }

    void TryPickup()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) <= pickupRange && IsLookingAtItem())
        {
            Debug.Log("Picking up " + Item.name);
            Inventory.Instance.Add(Item);
            Destroy(gameObject);
        }
    }

    private bool IsLookingAtItem()
    {
        Vector2 crosshairPosition = crosshairRectTransform.position; // Use the actual position of the crosshair
        Ray ray = playerCamera.ScreenPointToRay(crosshairPosition); // Cast ray from the crosshair position
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }
}