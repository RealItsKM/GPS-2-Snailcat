using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 5f;
    public Rigidbody rb;  // Reference to the player's Rigidbody

    private void Start()
    {
        // Ensure the Rigidbody is not using gravity (so the player doesn't fall)
        rb.useGravity = false;
    }

    void Update()
    {
        // Get input from the joystick
        float horizontal = joystick.Horizontal();
        float vertical = joystick.Vertical();

        // Remap joystick directions: Up -> Left, Right -> Forward, Down -> Right, Left -> Backward
        Vector3 direction = new Vector3(-vertical, 0, horizontal).normalized;

        // Check if the joystick input is significant enough to move the player
        if (direction.magnitude >= 0.1f)
        {
            // Calculate the target velocity based on the direction and speed
            Vector3 targetVelocity = direction * speed;

            // Move the player using Rigidbody's velocity to handle collisions properly
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        }
        else
        {
            // If not moving, stop horizontal movement but keep the Y velocity (for gravity or jumps)
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
}
