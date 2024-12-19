using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithPivots : MonoBehaviour
{
    public GameObject StartObject;
    public GameObject EndObject;
    private Vector3 InitialScale;
    void Start()
    {
        InitialScale = transform.localScale;
        UpdateTransformForScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartObject.transform.hasChanged || EndObject.transform.hasChanged) {
            UpdateTransformForScale();
        }
    }

    void UpdateTransformForScale() { 
        float distance = Vector3.Distance(StartObject.transform.position, EndObject.transform.position);
        transform.localScale = new Vector3(InitialScale.x, distance / 2f, InitialScale.z);

        Vector3 middlePoint = (StartObject.transform.position + EndObject.transform.position) / 2f;
        transform.position = middlePoint;

        Vector3 rotationDirection = (EndObject.transform.position - StartObject.transform.position);
        transform.up = rotationDirection;
    }
}
