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
    [SerializeField] private GameObject inventoryToolbar;
    [SerializeField] private GameObject paperRecipeEasy;
    [SerializeField] private GameObject paperRecipeMedium;
    [SerializeField] private GameObject paperRecipeHard;

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

        // Set the appropriate paper recipe and riddle text based on difficulty
        string difficulty = PlayerPrefs.GetString("Difficulty", "Easy");
        switch (difficulty)
        {
            case "Easy":
                paperRecipeEasy.SetActive(true);
                paperRecipeMedium.SetActive(false);
                paperRecipeHard.SetActive(false);
                SetRecipeRiddleText("To cure the poison, you must combine:\n" +
                                    "A flask to hold the cure at the base,\n" +
                                    "A small blue potion to soothe the pain in the middle,\n" +
                                    "And a small red potion to heal the wound at the top.\n" +
                                    "Align them vertically in the second column.");
                break;
            case "Medium":
                paperRecipeEasy.SetActive(false);
                paperRecipeMedium.SetActive(true);
                paperRecipeHard.SetActive(false);
                SetRecipeRiddleText("To cure the poison, you must combine:\n" +
                                    "A flask to hold the cure at the bottom,\n" +
                                    "A small blue potion to neutralize the poison in the middle,\n" +
                                    "And a small red potion to heal the wound at the top.\n" +
                                    "The organization resembles a stick, lying between 1 and 3.");
                break;
            case "Hard":
                paperRecipeEasy.SetActive(false);
                paperRecipeMedium.SetActive(false);
                paperRecipeHard.SetActive(true);
                SetRecipeRiddleText("To cure the poison, you must combine:\n" +
                                    "A flask to hold the cure at the bottom,\n" +
                                    "A rare blue herb to neutralize the poison in the middle,\n" +
                                    "And a small red potion to heal the wound at the top.\n" +
                                    "The column you seek is the only even prime number.");
                break;
        }
    }

    void Update()
    {
        // Check for interaction input (E key) only if the player is in range and the UI is inactive
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer <= interactionRange)
        {
            // Check if the player is looking at the item and show the interaction prompt
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
                // Hide the interaction prompt if the player is not looking at the item
                interactionPrompt.SetActive(false);
            }
        }
        else
        {
            outline.enabled = false;
            interactionPrompt.SetActive(false);
        }
    }

    // Show or hide the paper recipe UI
    private void TogglePaperUI()
    {
        // Disable character movement, cursor, and inventory on show and vice versa on hide
        isPaperUIActive = !isPaperUIActive;
        paperRecipeUI.SetActive(isPaperUIActive);
        firstPersonController.enabled = !isPaperUIActive;
        Cursor.lockState = isPaperUIActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaperUIActive;
        outline.enabled = !isPaperUIActive;
        interactionPrompt.SetActive(!isPaperUIActive);
        inventoryToolbar.SetActive(!isPaperUIActive);
        craftingUI.SetActive(false); // Should always be hidden when the paper UI is active

        if (!isPaperUIActive)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // Set the text of the paper recipe riddle
    public void SetRecipeRiddleText(string text)
    {
        recipeRiddleText.text = text;
    }
}