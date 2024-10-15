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

    public float rotationSpeed = 10f;  // Speed of the rotation

    private void Start()
    {
        rb.useGravity = false;
    }

    void Update()
    {
        
        if (TutorialMode.tutorialOn)
        {
            if (TutorialMode.popUpOn)
            {
                return;
            }
        }
        

        if (!isStunned)
        {
            float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
            float moveVertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow keys

            float joystickHorizontal = joystick.Horizontal();
            float joystickVertical = joystick.Vertical();

            //Combine WASD and joystick 
            float horizontal = Mathf.Abs(joystickHorizontal) > Mathf.Abs(moveHorizontal) ? joystickHorizontal : moveHorizontal;
            float vertical = Mathf.Abs(joystickVertical) > Mathf.Abs(moveVertical) ? joystickVertical : moveVertical;

            //Remap directions for joystick: Up -> Left, Right -> Forward, Down -> Right, Left -> Backward
            Vector3 direction = new Vector3(-vertical, 0, horizontal).normalized;

            //Check if the input is significant enough 
            if (direction.magnitude >= 0.1f)
            {
                Vector3 targetVelocity = direction * speed;

                rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);


                RotatePlayer(direction);
            }
            else
            {
                //If not moving, stop horizontal movement but keep Y velocity
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        else //When stunned
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void RotatePlayer(Vector3 direction)
    {
        //Determine the target rotation based on the movement direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
