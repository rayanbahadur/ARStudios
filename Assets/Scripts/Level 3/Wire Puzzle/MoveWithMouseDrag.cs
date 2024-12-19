using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MoveWithMouseDrag : MonoBehaviour
{
    private Camera mainCamera;
    private float CameraZDist;

    void Start()
    {
        mainCamera = Camera.main;
        CameraZDist = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseUp()
    {
        transform.hasChanged = false;
    }
    private void OnMouseDrag()
    {
        Vector3 ScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDist);
        Vector3 NewWorldPosition = mainCamera.ScreenToWorldPoint(ScreenPosition);
        transform.position = NewWorldPosition;
    }
}
