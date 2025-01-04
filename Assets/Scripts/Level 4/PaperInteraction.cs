using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using StarterAssets;

public class PaperInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private GameObject paperRecipeUI;
    [SerializeField] private TextMeshProUGUI recipeRiddleText;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject craftingUI;

    private Transform playerTransform;
    private Camera playerCamera;
    private RectTransform crosshairRectTransform;
    private Outline outline;
    private bool isPaperUIActive = false;
    private FirstPersonController firstPersonController;
    private myControls inputActions;

    void Awake()
    {
        inputActions = new myControls();
        inputActions.Player.Enable();
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = Camera.main;

        GameObject crosshair = GameObject.Find("MainCanvas/Crosshair");
        if (crosshair != null)
        {
            crosshairRectTransform = crosshair.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Crosshair not found in the MainCanvas.");
        }

        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
        else
        {
            Debug.LogError("Outline component not found on the object.");
        }

        firstPersonController = FindObjectOfType<FirstPersonController>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer <= interactionRange)
        {
            RaycastOutlineUtility.CheckIfPlayerIsLookingAtItem(playerCamera, crosshairRectTransform, gameObject, outline);
            if (outline.enabled)
            {
                interactionPrompt.SetActive(true);
                if (inputActions.Player.ActionKey.triggered)
                {
                    TogglePaperUI();
                }
            }
            else
            {
                interactionPrompt.SetActive(false);
            }
        }
        else
        {
            outline.enabled = false;
            interactionPrompt.SetActive(false);
        }
    }

    private void TogglePaperUI()
    {
        isPaperUIActive = !isPaperUIActive;
        paperRecipeUI.SetActive(isPaperUIActive);
        firstPersonController.enabled = !isPaperUIActive;
        Cursor.lockState = isPaperUIActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaperUIActive;
        outline.enabled = !isPaperUIActive;
        interactionPrompt.SetActive(!isPaperUIActive);
        craftingUI.SetActive(false);

        if (!isPaperUIActive)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void SetRecipeRiddleText(string text)
    {
        recipeRiddleText.text = text;
    }
}