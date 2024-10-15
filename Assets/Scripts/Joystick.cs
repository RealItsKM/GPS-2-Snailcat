using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform joystickBackground;
    private RectTransform joystickHandle;
    public Vector2 inputVector;
    private Vector2 joystickCenter;

    void Start()
    {
        joystickBackground = GetComponent<RectTransform>();
        joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
        joystickCenter = joystickBackground.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        /*
        if (TutorialMode.popUpOn)
        {
            Debug.Log("B");
            inputVector = Vector2.zero;
            return;
        }
        */

        if (CameraControls.isDragging == false)
        {
            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position))
            {
                position.x = (position.x / joystickBackground.sizeDelta.x) * 2 - 1;
                position.y = (position.y / joystickBackground.sizeDelta.y) * 2 - 1;

                inputVector = new Vector2(position.x, position.y);
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

                joystickHandle.anchoredPosition = new Vector2(inputVector.x * (joystickBackground.sizeDelta.x / 2), inputVector.y * (joystickBackground.sizeDelta.y / 2));
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }
}
