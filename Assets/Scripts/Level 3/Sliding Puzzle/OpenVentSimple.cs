using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenVentSimple : MonoBehaviour
{
    public Outline outline;
    private myControls inputActions; // Input actions for player interaction

    // Start is called before the first frame update
    private void Awake()
    {
        inputActions = new myControls(); // Initialise input actions
        inputActions.Player.Enable();
    }
    void Start()
    {
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        outline.enabled = true; // Highlight the vent
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && inputActions.Player.LMouseClick.triggered)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        outline.enabled = false;
    }
}
