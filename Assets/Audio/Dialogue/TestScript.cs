using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] AudioObject clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Vocals.instance.Speak(clip);
    }
}
