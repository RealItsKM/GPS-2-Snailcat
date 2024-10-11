using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControls : MonoBehaviour
{
    public CameraFollow cameraFollow; // Reference to CameraFollow script
    public float moveSpeed = 0.2f;    // Speed of camera movement
    public float minX = -10f;         // Minimum X position for the camera
    public float maxX = 10f;          // Maximum X position for the camera

    private Vector2 initialTouchPosition;
    public static bool isDragging = false;
    void Update()
    {
        // Check if the touch or mouse click is over a UI element
        if (IsPointerOverUIObject())
            return;

        // Touch input
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Handle touch begin
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                initialTouchPosition = touch.position;
                cameraFollow.SetManualControl(true); // Disable camera follow
            }
            // Handle touch movement
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 currentTouchPosition = touch.position;
                Vector2 deltaPosition = currentTouchPosition - initialTouchPosition;

                // Move the camera left or right along the X-axis
                float moveX = -deltaPosition.x * moveSpeed * Time.deltaTime;
                Vector3 newPosition = transform.position + new Vector3(0, 0, moveX); //put moveX at z because camerafollow's trasnform.look

                // Clamp the camera movement to within the defined limits
                newPosition.z = Mathf.Clamp(newPosition.z, minX, maxX);

                transform.position = newPosition;

                initialTouchPosition = currentTouchPosition;
            }
            // Handle touch end
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                cameraFollow.SetManualControl(false); // Re-enable camera follow
            }
        }

        // Mouse input
        else if (Input.GetMouseButtonDown(0)) // Mouse click start
        {
            isDragging = true;
            initialTouchPosition = Input.mousePosition;
            cameraFollow.SetManualControl(true); // Disable camera follow
        }
        else if (Input.GetMouseButton(0) && isDragging) // Mouse drag
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 deltaPosition = (Vector2)currentMousePosition - initialTouchPosition;

            // Move the camera left or right along the X-axis
            float moveX = -deltaPosition.x * moveSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(0, 0, moveX);

            // Clamp the camera movement to within the defined limits
            newPosition.z = Mathf.Clamp(newPosition.z, minX, maxX);

            transform.position = newPosition;

            initialTouchPosition = currentMousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Mouse click end
        {
            isDragging = false;
            cameraFollow.SetManualControl(false); // Re-enable camera follow
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // Check if any of the results belong to UI elements that should block the camera movement
        foreach (RaycastResult result in results)
        {
            // Check if the UI element should block the camera movement based on its tag or a specific component
            if (result.gameObject.CompareTag("BlockCamera")) // Replace with the appropriate tag or component check
            {
                return true;  // Block camera movement for this UI element
            }
        }

        // If no UI elements with the specified tag/component were found, allow camera movement
        return false;
    }
}
