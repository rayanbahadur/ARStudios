using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float gravityStrength = 9.8f; // Gravity strength

    void Update()
    {
        // Handle gravity changes with alternative keys
        if (Input.GetKeyDown(KeyCode.Keypad8)) // Change gravity upward (Numpad 8)
        {
            ChangeGravity(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2)) // Change gravity downward (Numpad 2)
        {
            ChangeGravity(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4)) // Change gravity to the left (Numpad 4)
        {
            ChangeGravity(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6)) // Change gravity to the right (Numpad 6)
        {
            ChangeGravity(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5)) // Reset gravity to default (Numpad 5)
        {
            ChangeGravity(Vector3.down);
        }
    }

    void ChangeGravity(Vector3 newDirection)
    {
        Physics.gravity = newDirection * gravityStrength;
        Debug.Log($"Gravity changed to: {newDirection}");
    }
}
