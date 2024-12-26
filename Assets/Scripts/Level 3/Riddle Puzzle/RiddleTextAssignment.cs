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
    [SerializeField] private Outline outline; // Outline for highlighting interactable objects

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

    private FirstPersonController firstPersonController; // Reference to the player's controller
    private myControls inputActions; // Input action handler for player controls

    private void Awake()
    {
        inputActions = new myControls(); // Initialize input actions
        inputActions.Player.Enable(); // Enable player-specific input actions
    }

    private void Start()
    {
        InitializeFirstPersonController(); // Set up the first-person controller reference
        GenerateRiddleContent(); // Generate the compiled riddle content
    }

    private void InitializeFirstPersonController()
    {
        // Locate the first-person controller in the scene
        firstPersonController = FindFirstObjectByType<FirstPersonController>();

        // Ensure the riddle text field is properly assigned
        if (riddleText == null)
        {
            Debug.LogError("Riddle TextMeshProUGUI is not assigned in the Inspector!");
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
    private void ToggleRiddleUI(bool state)
    {
        riddleUI.SetActive(state); // Show or hide the riddle UI
        firstPersonController.enabled = !state; // Disable player movement when the UI is active
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked; // Lock or unlock the cursor
        Cursor.visible = state; // Show or hide the cursor
        interactionPrompt.SetActive(!state);
        outline.enabled = !state;

        // Clear UI selection when the riddle UI is closed
        if (!state)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Show interaction prompt if the player enters the trigger zone and the UI is inactive
        if (other.CompareTag("Player") && !riddleUI.activeSelf)
        {
            interactionText.text = $"Press 'E' to read page";
            interactionPrompt.SetActive(true);
            outline.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Activate the riddle UI if the player stays in the trigger zone and presses the action key
        if (other.CompareTag("Player") && inputActions.Player.ActionKey.triggered)
        {
            if (!riddleUI.activeSelf)
            {
                ToggleRiddleUI(true); // Show the riddle UI
            }
            else
            {
                ToggleRiddleUI(false); // Hide the riddle UI
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide interaction prompt when the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false);
            outline.enabled = false;
        }
    }
}
