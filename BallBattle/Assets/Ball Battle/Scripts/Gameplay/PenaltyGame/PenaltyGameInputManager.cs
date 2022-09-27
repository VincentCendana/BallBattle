using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PenaltyGameInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("UI Component")]
    public RectTransform centerInput;                   // rect transform to track original position of a drag input

    [Header("Movement Component")]
    private Vector2 inputPosition;                      // joystick input position

    // pointer down event
    public void OnPointerDown(PointerEventData eventData)
    {
        centerInput.position = eventData.position;
        OnDrag(eventData);
    }

    // pointer up event
    public void OnPointerUp(PointerEventData eventData)
    {
        // reset input data
        inputPosition = Vector2.zero;
    }

    // touch drag input
    public void OnDrag(PointerEventData eventData)
    {
        // set input values based on drag input data
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
           centerInput,
           eventData.position,
           eventData.pressEventCamera,
           out inputPosition))
        {
            // normalize input value
            if (inputPosition.magnitude > 1.0f)
            {
                inputPosition = inputPosition.normalized;
            }
        }
    }

    // get user's horizontal input value
    public float InputHorizontal()
    {
        return inputPosition.x;
    }

    // get user's vertical input value
    public float InputVertical()
    {
        return inputPosition.y;
    }
}
