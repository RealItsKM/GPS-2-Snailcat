using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 5f;
    public Rigidbody rb;  // Reference to the player's Rigidbody
    public static bool isStunned = false;
    public static int timesCaught;
    public GameObject stunTimer;

    private void Start()
    {
        rb.useGravity = false;
    }

    void Update()
    {
        if (!isStunned)
        {
            float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
            float moveVertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow keys

            float joystickHorizontal = joystick.Horizontal();
            float joystickVertical = joystick.Vertical();

            // Combine WASD and joystick input
            float horizontal = Mathf.Abs(joystickHorizontal) > Mathf.Abs(moveHorizontal) ? joystickHorizontal : moveHorizontal;
            float vertical = Mathf.Abs(joystickVertical) > Mathf.Abs(moveVertical) ? joystickVertical : moveVertical;

            // Remap directions for joystick: Up -> Left, Right -> Forward, Down -> Right, Left -> Backward
            Vector3 direction = new Vector3(-vertical, 0, horizontal).normalized;

            // Check if the input (from either WASD or joystick) is significant enough to move the player
            if (direction.magnitude >= 0.1f)
            {
                // Calculate the target velocity based on the direction and speed
                Vector3 targetVelocity = direction * speed;

                // Move the player using Rigidbody's velocity to handle collisions properly
                rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
            }
            else
            {
                // If not moving, stop horizontal movement but keep the Y velocity
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        else // When stunned
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    public void StunPlayer(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;

            timesCaught++;

            stunTimer.SetActive(true);

            StartCoroutine(EndStun(duration));
        }
    }

    IEnumerator EndStun(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStunned = false;
        stunTimer.SetActive(false);
        Debug.Log("Stun Over");
    }
}
