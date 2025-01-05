using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform handTransform; // Reference to the hand's transform
    private Transform currentItemTransform; // Transform of the current item in hand
    private bool isAnimating = false; // Prevent overlapping animations
    public float swordDamage = 25f; // Damage dealt by the sword
    public float raycastRange = 2f; // Range of the sword attack

    void Update()
    {
        // Check if there is a child item attached to the hand
        if (handTransform.childCount > 0)
        {
            currentItemTransform = handTransform.GetChild(0); // Get the first child (current hand item)

            // Check if the item's name matches "Sword(Clone)"
            if (currentItemTransform.name == "Sword(Clone)")
            {
                // Trigger animation on E press
                if (Input.GetKeyDown(KeyCode.Q) && !isAnimating)
                {
                    StartCoroutine(AnimateSword());
                }
            }
            else
            {
                ResetItemPosition();
            }
        }
        else
        {
            currentItemTransform = null;
        }
    }

    /// <summary>
    /// Performs the animation for the sword and applies damage once during the swing.
    /// </summary>
    private IEnumerator AnimateSword()
    {
        isAnimating = true;

        Quaternion initialRotation = currentItemTransform.localRotation;
        Quaternion intermediateRotation = Quaternion.Euler(0f, 0f, 0f); // Step 1: Rotate to (0, 0, 0)
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 80f);      // Step 2: Rotate to (0, 0, 80)

        float time = 0f;
        float animationSpeed = 5f;

        // Step 1: Rotate to (0, 0, 0)
        while (time < 1f)
        {
            time += Time.deltaTime * animationSpeed;
            currentItemTransform.localRotation = Quaternion.Lerp(initialRotation, intermediateRotation, time);
            yield return null;
        }

        // Perform raycast once here (at the midpoint of the swing)
        PerformRaycast();

        time = 0f;

        // Step 2: Rotate to (0, 0, 80)
        while (time < 1f)
        {
            time += Time.deltaTime * animationSpeed;
            currentItemTransform.localRotation = Quaternion.Lerp(intermediateRotation, targetRotation, time);
            yield return null;
        }

        time = 0f;

        // Step 3: Return to the initial position
        while (time < 1f)
        {
            time += Time.deltaTime * animationSpeed;
            currentItemTransform.localRotation = Quaternion.Lerp(targetRotation, initialRotation, time);
            yield return null;
        }

        isAnimating = false;
    }

    /// <summary>
    /// Casts a ray from the crosshair to detect enemies and applies damage once per swing.
    /// </summary>
    private void PerformRaycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Cast from the center of the screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            ZombieHealth zombieHealth = hit.collider.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(swordDamage);
                Debug.Log($"Zombie hit! Dealt {swordDamage} damage.");
            }
        }
    }

    /// <summary>
    /// Resets the item position for non-sword items.
    /// </summary>
    private void ResetItemPosition()
    {
        if (currentItemTransform != null)
        {
            currentItemTransform.localPosition = Vector3.zero;
            currentItemTransform.localRotation = Quaternion.identity;
        }
    }
}
