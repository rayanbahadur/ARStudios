using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenVentSimple : MonoBehaviour
{
    public Outline outline;
    public GameObject interactionPrompt;
    public TextMeshProUGUI interactionText; // Text for the interaction prompt

    private myControls inputActions; // Input actions for player interaction
    private bool inTrigger = false;

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
        if ((inTrigger && inputActions.Player.ActionKey.triggered) || (inTrigger && inputActions.Player.LMouseClick.triggered))
        {
            gameObject.SetActive(false);
            interactionPrompt.SetActive(false);
            this.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = true; // Highlight the vent
            inTrigger = true;
            interactionText.text = "Press 'E' to Open Vent";
            interactionPrompt.SetActive(true);
        }
}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = false;
            inTrigger = false;
            interactionPrompt.SetActive(false);
        }
    }
}
