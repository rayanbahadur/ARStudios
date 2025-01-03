using System;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;

public class CamRotation: MonoBehaviour
{

    Transform camGFX;

    bool startNextRotation = true;
    [SerializeField] bool rotRight;

    [SerializeField] float yaw;
    [SerializeField] float pitch;


    public float secondsToRot; // Accessed by the DifficultyAssignment script
    [SerializeField] float rotSwitchTime;

    void Start()
    {
        camGFX = transform.GetChild(0);
        camGFX.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);
        SetupStartRotation();
    }

    void Update()
    {
        if (startNextRotation && rotRight)
        {
            StartCoroutine(Rotate(yaw, secondsToRot));
        }
        else if (startNextRotation && !rotRight)
        {
            StartCoroutine(Rotate(-yaw, secondsToRot));
        }
    }

    IEnumerator Rotate(float yaw, float duration)
    {
        startNextRotation = false;

        Quaternion initialRotation = transform.rotation;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.rotation = initialRotation * Quaternion.AngleAxis(timer / duration * yaw, Vector3.up);
            yield return null;
        }

        yield return new WaitForSeconds(rotSwitchTime);

        startNextRotation=true;
        rotRight = !rotRight;
    }

    void SetupStartRotation()
    {
        if (rotRight)
        {
            transform.localRotation = Quaternion.AngleAxis(-yaw / 2, Vector3.up);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(yaw / 2, Vector3.up);
        }
    }
}
