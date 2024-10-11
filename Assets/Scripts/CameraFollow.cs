using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;               // The player object to follow
    public Vector3 offset;                 // Offset position of the camera relative to the player
    public float smoothSpeed = 0.125f;     // Speed of the smoothing
    public float rotationSpeed = 5f;       // Speed of the rotation smoothing
    public bool isManualControl = false;

    private Vector3 velocity = Vector3.zero;  // For SmoothDamp function

    private void Start()
    {
        // Directly set the initial position of the camera
        transform.position = player.position + offset;
        transform.LookAt(player);

        isManualControl = false;
    }

    private void FixedUpdate()
    {
        if (!isManualControl)
        {
            Vector3 desiredPosition = player.position + offset;

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;

            Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void SetManualControl(bool isManual)
    {
        isManualControl = isManual;
    }
}
