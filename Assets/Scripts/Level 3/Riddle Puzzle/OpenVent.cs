using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class OpenVent : MonoBehaviour
{
    [SerializeField] private PlayableDirector unlockLockbox;
    [SerializeField] private GameObject poster;
    [SerializeField] private TMP_Text ventMessage;

    private FirstPersonController firstPersonController;
    private myControls inputActions;
    private Collider ventCollider;

    private bool isPosterDeactivated => !poster.activeInHierarchy;
    private bool isScrewdriverInHand => Inventory.Instance != null && Inventory.Instance.currentHandItem != null &&
                                        Inventory.Instance.currentHandItem.CompareTag("Screwdriver");

    // Start is called before the first frame update
    void Start()
    {
        ventMessage.gameObject.SetActive(false); // Ensure vent message is hidden at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (isPosterDeactivated && isScrewdriverInHand && inputActions.Player.LMouseClick.triggered)
        {
            OpenVentAction();
        }
    }

    private void OpenVentAction()
    {
        // Play the animation
        if (unlockLockbox != null)
        {
            unlockLockbox.Play();
        }

        // Display the vent message
        ventMessage.gameObject.SetActive(true);

        this.enabled = false;
    }
}
