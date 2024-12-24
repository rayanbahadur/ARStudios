using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePair : MonoBehaviour
{
    private Camera _mainCamera;
    private float _cameraZDist;
    private Vector3 _initPos;
    private bool _isConnected;

    private const string _portTag = "Port";
    private const float _dragResponseThreshold = 2;
    void Start()
    {
        _mainCamera = Camera.main;
        _cameraZDist = _mainCamera.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDrag()
    {
        Vector3 ScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraZDist);
        Vector3 NewWorldPosition = _mainCamera.ScreenToWorldPoint(ScreenPos);
    if (!_isConnected)
        {
            transform.position = NewWorldPosition;
        }
    else if (Vector3.Distance(transform.position, NewWorldPosition) > _dragResponseThreshold)
        {
            _isConnected = false;
        }
    }

    private void OnMouseUp()
    {
        if (!_isConnected)
        {
            transform.position = _initPos;
        }
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetInitialPosition(Vector3 NewPosition) {
        _initPos = NewPosition;
        transform.position = _initPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_portTag))
        {
            _isConnected = true;
            transform.position = other.transform.position;
        }
    }
}
