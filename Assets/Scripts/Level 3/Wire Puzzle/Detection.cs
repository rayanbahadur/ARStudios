using System;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;

public class Detection: MonoBehaviour
{
    [SerializeField] Material searchingMat, spottedMat;

    string playerTag;
    Transform lens;

    void Start()
    {
        lens.transform.parent.GetComponent<Transform>();
        playerTag = GameObject.FindGameObjectWithTag("Player").tag;
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == playerTag)
        {
            Vector3 direction = other.transform.position - lens.position;
            RaycastHit hit;

            if(Physics.Raycast(lens.transform.position, direction.normalized, out hit, 1000))
            {
                Debug.Log(hit.collider.name);

                if (hit.collider.gameObject.tag == playerTag)
                {
                    lens.GetComponentInParent<MeshRenderer>().material = spottedMat;
                }
                else
                {
                    lens.GetComponentInParent<MeshRenderer>().material = searchingMat;
                }
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform.tag == playerTag)
        {
            lens.GetComponentInParent<MeshRenderer>().material = searchingMat;
        }
    }
}
