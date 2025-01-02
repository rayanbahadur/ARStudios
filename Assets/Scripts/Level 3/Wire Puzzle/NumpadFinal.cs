using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NumpadFinal : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private Outline outline;

    private myControls inputActions;

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
        inputActions.Player.Enable();
    }
    private void Start()
    {
        player.SetActive(true);
        camera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        outline.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inputActions.Player.ActionKey.triggered) {
                ToggleCamera();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        outline.enabled = false;
    }

    private void ToggleCamera()
    {
        bool active = !player.activeSelf;
        player.SetActive(active);
        camera.SetActive(!active);
        mainCanvas.SetActive(active);
        Cursor.visible = !active;
        Cursor.lockState = !active ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
