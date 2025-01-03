using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlUI : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject gameControlUI;

    private myControls inputActions;
    private FirstPersonController firstPersonController;

    private void Awake()
    {
        inputActions = new myControls(); // Initialise input actions
        inputActions.Player.Enable();
    }

    private void Start()
    {
        firstPersonController = FindObjectOfType<FirstPersonController>();
    }

    private void Update()
    {
        if (inputActions.Player.Escape.triggered)
        {
            ToggleShowUI(!gameControlUI.activeSelf);
        }
    }

    private void ToggleShowUI(bool activeState)
    {
        gameControlUI.SetActive(activeState);
        inventory.SetActive(!activeState);
        firstPersonController.enabled = !activeState;

        Cursor.visible = activeState;
        Cursor.lockState = activeState? CursorLockMode.None : CursorLockMode.Locked;

        // Disable the crosshair
        crosshair?.SetActive(!activeState);
    }
}
