using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    [SerializeField] AudioObject clip;
    [SerializeField] float audioDelay = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayAudioWithDelay());
        }
    }

    private IEnumerator PlayAudioWithDelay()
    {
        yield return new WaitForSeconds(audioDelay);
        Vocals.instance.Speak(clip);
        gameObject.SetActive(false);
    }
}
