using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.EventSystems;

public class RiddleAssigner : MonoBehaviour
{
    [Header("Riddle Components")]
    [SerializeField] private GameObject riddleUI; // UI panel for displaying the riddle
    [SerializeField] private TextMeshProUGUI riddleText; // Text field for riddle content

    [Header("Interaction Settings")]
    [SerializeField] private GameObject interactionPrompt; // Prompt to show interaction availability
    [SerializeField] private TextMeshProUGUI interactionText; // Text field for interaction message
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private PlayerProgress playerProgress;

    private string riddleContent; // Accumulated content of all riddles
    private string[][] riddles = new string[][]
    {
        new string[]
        {
            "I stand on four, yet walk no more,\nYour gaze must sweep my lowest floor.\nSeek where my sturdy limbs reside,\nA clue is there for you to bide.\n\n",
            "I carry your thoughts, your meals, your art,\nOn a surface smooth, a sturdy part.\nBut what supports me, steady and true,\nHolds a number, just for you.\n\n",
            "A surface strong, where hands do play,\nBut look beneath, where feet may stray.\nA sturdy post, it bears the weight,\nYour clue lies there; don’t hesitate.\n\n",
            "Standing firm through every chore,\nA leg of strength, its secret store.\nBeneath the top where work is done,\nFind your hint, your journey's begun.\n\n",
            "Four pillars hold this giant's grace,\nBut one hides truth in its embrace.\nBend low, observe, and you will see,\nThe answer lies where it should be.\n\n"
        },
        new string[]
        {
            "A porcelain throne, a captive’s seat,\nA place of despair, where ends meet.\nBut dare to reach where the waters flow,\nA secret lies where few would go.\n\n",
            "A royal seat for times of need,\nA hollow place where secrets breed.\nLook deep within, where shadows play,\nA number hides to guide your way.\n\n",
            "A swirling dance in porcelain white,\nSearch within where dark meets light.\nWhere waters gather, secrets dwell,\nFind the clue within its well.\n\n",
            "A silent witness to many a tale,\nWithin its depths, the truth prevails.\nDare to delve where waters spin,\nYour secret's there, deep within.\n\n",
            "Around the rim, beneath the flow,\nA clue is hidden, this you know.\nGaze into this watery maze,\nAnd see the number through the haze.\n\n"
        },
        new string[]
        {
            "By your side each dawn and night,\nA holder of tools for pearly white.\nSearch beneath this daily chore,\nA hidden clue lies near its core.\n\n",
            "For bristles and paste, it holds the weight,\nA mundane item, yet a twist of fate.\nAround its rest, a secret resides,\nLook closely now, where it hides.\n\n",
            "It stands for your clean routine,\nBut look outside, where it's unseen.\nA daily chore, a secret face,\nSearch around its outer space.\n\n",
            "Humble and small, it holds your care,\nBut outside its form, there’s more to spare.\nA hidden sign on its surface plain,\nThe answer lies, your effort’s gain.\n\n",
            "By your sink it stands with pride,\nBut a secret’s etched along the side.\nA careful look, a closer view,\nThe number’s there, waiting for you.\n\n"
        },
        new string[]
        {
            "They hold you in, these metal foes,\nYet one among them subtly shows,\nA mark unseen, a number slight,\nFind the bar that gives you light.\n\n",
            "They line your world, these prison walls,\nA cage of steel, but one still calls.\nSearch the bars, both near and far,\nA single one bears your star.\n\n",
            "A barrier strong, it keeps you in,\nBut one lone bar conceals within.\nFeel its surface, cold and bare,\nOne has a secret; find it there.\n\n",
            "A cage of steel, a row of lines,\nOne holds the clue among the signs.\nSeek the bar that stands apart,\nIts hidden truth will guide your heart.\n\n",
            "Count the bars, from left to right,\nBut one will stand out, clear and bright.\nIts surface tells of a hidden mark,\nThe number’s there to light your dark.\n\n"
        }
    };

    private Transform playerTransform;
    private Camera playerCamera;
    private RectTransform crosshairRectTransform;
    private Outline outline;
    private bool isRiddleUIActive = false;
    private FirstPersonController firstPersonController;
    private myControls inputActions;
    private bool hasProgressBeenAdded = false; // Flag to track if progress has been added

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
        inputActions.Player.Enable(); // Enable player-specific input actions
    }

    private void Start()
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
        GenerateRiddleContent();
    }

    private void Update()
    {

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer <= interactionRange)
        {
            RaycastOutlineUtility.CheckIfPlayerIsLookingAtItem(playerCamera, crosshairRectTransform, gameObject, outline);
            if (outline.enabled)
            {

                if (inputActions.Player.ActionKey.triggered)
                {
                    interactionPrompt.SetActive(false);
                    ToggleRiddleUI();
                    if (!hasProgressBeenAdded)
                    {
                        playerProgress.AddProgress(30);
                        hasProgressBeenAdded = true;
                    }
                }
            }
        }
        else
        {
            outline.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            interactionText.text = "Press 'E' to Read the Page";
            interactionPrompt.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false);
        }
    }
    private void GenerateRiddleContent()
    {
        // Iterate through the riddle categories and compile their content
        for (int i = 0; i < riddles.Length; i++)
        {
            // Randomly choose one variation of the current riddle
            riddleContent += $"{i + 1}) " + riddles[i][Random.Range(0, riddles[i].Length)];
        }
        // Assign the compiled riddles to the text field for display
        riddleText.text = riddleContent;
    }
    private void ToggleRiddleUI()
    {
        isRiddleUIActive = !isRiddleUIActive;
        riddleUI.SetActive(isRiddleUIActive);
        firstPersonController.enabled = !isRiddleUIActive;
        Cursor.lockState = isRiddleUIActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isRiddleUIActive;
        outline.enabled = !isRiddleUIActive;
        interactionPrompt.SetActive(!isRiddleUIActive);

        if (!isRiddleUIActive)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
