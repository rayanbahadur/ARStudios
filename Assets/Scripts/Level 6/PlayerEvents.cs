using UnityEngine;
using UnityEngine.Playables;
using StarterAssets; // Namespace for the FirstPersonController and StarterAssetsInputs

public class PlayerEvents : MonoBehaviour
{
    public PlayableDirector director;               // Reference to the PlayableDirector
    public FirstPersonController firstPersonController; // Reference to the FirstPersonController script

    private bool isPlaying = false; // Track if the Timeline is playing

    private void OnEnable()
    {
        Debug.Log("OnEnable called.");
        if (director != null)
        {
            // Reset the Timeline immediately
            director.time = 0;
            director.Evaluate(); // Evaluate to apply the reset state
            Debug.Log("Timeline reset to start during OnEnable.");

            // Subscribe to PlayableDirector events
            director.played += OnTimelineStarted;
            director.stopped += OnTimelineStopped;
            Debug.Log("Subscribed to PlayableDirector events.");

            // Check current state
            if (director.state == PlayState.Playing)
            {
                Debug.Log("Timeline is already playing.");
                OnTimelineStarted(director);
            }
            else
            {
                Debug.Log("Timeline is not playing. Current state: " + director.state);
            }
        }
        else
        {
            Debug.LogWarning("PlayableDirector reference is missing.");
        }
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable called.");
        if (director != null)
        {
            // Unsubscribe from events to avoid memory leaks
            director.played -= OnTimelineStarted;
            director.stopped -= OnTimelineStopped;
            Debug.Log("Unsubscribed from PlayableDirector events.");
        }
    }

    private void OnTimelineStarted(PlayableDirector _)
    {
        Debug.Log("OnTimelineStarted called. Current state: " + director.state);
        isPlaying = true;
        DisableMovement();
    }

    private void OnTimelineStopped(PlayableDirector _)
    {
        Debug.Log("OnTimelineStopped called. Current state: " + director.state);
        isPlaying = false;
        EnableMovement();
    }

    private void DisableMovement()
    {
        Debug.Log("DisableMovement called.");
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false; // Disable the FirstPersonController script
            StarterAssetsInputs inputs = firstPersonController.GetComponent<StarterAssetsInputs>();
            if (inputs != null)
            {
                inputs.move = Vector2.zero;
                inputs.look = Vector2.zero;
                inputs.sprint = false;
                inputs.jump = false;
            }
            Debug.Log("FirstPersonController disabled during Timeline and inputs cleared.");
        }
        else
        {
            Debug.LogWarning("FirstPersonController reference is missing.");
        }
    }

    private void EnableMovement()
    {
        Debug.Log("EnableMovement called.");
        if (firstPersonController != null)
        {
            firstPersonController.enabled = true; // Re-enable the FirstPersonController script
            StarterAssetsInputs inputs = firstPersonController.GetComponent<StarterAssetsInputs>();
            if (inputs != null)
            {
                inputs.move = Vector2.zero; // Reset inputs to prevent residual movement
            }
            Debug.Log("FirstPersonController enabled after Timeline.");
        }
        else
        {
            Debug.LogWarning("FirstPersonController reference is missing.");
        }
    }
}
