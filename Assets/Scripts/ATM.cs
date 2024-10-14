using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ATM : MonoBehaviour
{
    public GameObject atmScreen;

    myControls inputActions;

    public UnityEvent myAction;

    private void Awake()
    {
        inputActions = new myControls();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            LeanTween.scale(atmScreen, Vector3.one, 2).setEaseInBounce();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            LeanTween.scale(atmScreen, Vector3.zero, 2).setEaseInBounce();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (inputActions.Player.ActionKey.WasPerformedThisFrame())
                myAction.Invoke();
        }
    }

    public void OnEnable()
    {
        inputActions.Player.Enable();
    }

    public void OnDisable()
    {
        inputActions.Player.Disable();
    }


}
