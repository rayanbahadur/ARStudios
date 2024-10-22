using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    public Item Item;

    [SerializeField]
    private float pickupRange = 3.5f; 

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Pickup()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) <= pickupRange)
        {
            Debug.Log("Picking up " + Item.name);
            Inventory.Instance.Add(Item);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Item is too far to pick up");
        }
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
