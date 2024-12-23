using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    public MatchEntity _ownerMatchEntity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovablePair CollidedMoveable))
        {
            _ownerMatchEntity.PairObjectInteraction(true, CollidedMoveable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovablePair CollidedMoveable))
        {
            _ownerMatchEntity.PairObjectInteraction(false, CollidedMoveable);
        }
    }
}
