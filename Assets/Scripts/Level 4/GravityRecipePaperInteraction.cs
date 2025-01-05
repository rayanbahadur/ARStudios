using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using StarterAssets;

public class GravityRecipePaperInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private GameObject paperRecipeUI;
    [SerializeField] private TextMeshProUGUI recipeRiddleText;
    [SerializeField] private TextMeshProUGUI gravityRecipeText;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject inventoryToolbar;

    private Transform playerTransform;
    private Camera playerCamera;
    private RectTransform crosshairRectTransform;
    private Outline outline;
    private bool isPaperUIActive = false;
    private FirstPersonController firstPersonController;
    private myControls inputActions;

    public static GravityRecipePaperInteraction Instance;

    void Awake()
    {
        inputActions = new myControls();
        inputActions.Player.Enable();
        Instance = this;
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
        
        SetRecipeRiddleText("Gravity Potion:\n" +
                "To create the gravity potion, you must combine:\n" +
                "A potion flask at the top right,\n" +
                "A fat blue potion in the center,\n" +
                "And a blue potion at the bottom left.\n" +
                "Align them diagonally like a division sign.");
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
                    interactionPrompt.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Q) && Inventory.Instance.currentHandItem != null && Inventory.Instance.currentHandItem.name.Contains("Potion_02"))
        {
            Inventory.Instance.DrinkingAnimation("GravityPotion", "GravityPotion");
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
        recipeRiddleText.enabled = false;
        gravityRecipeText.enabled = true;

        if (!isPaperUIActive)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // Set the text of the paper recipe riddle
    public void SetRecipeRiddleText(string text)
    {
        gravityRecipeText.text = text;
    }


    public void ApplyGravityEffect()
    {
        // For gravity level, they can use the gravity potion to make any effect now
        PlayerPrefs.SetString("GravityPotion", "Drank");
    }
}