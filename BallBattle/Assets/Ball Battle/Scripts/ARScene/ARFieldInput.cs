using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARFieldInput : MonoBehaviour
{
    [Header("Game Component")]
    public GameObject fieldObject;                  // game field object to zoom/rotate

    [Header("Zoom Components")]
    public float currentZoom;                       // game field's current zoom amount
    public float zoomSpeed;                         // speed when zooming in/out on game field's scale
    public float minZoom;                           // minimum zoom value for game field
    public float maxZoom;                           // maximum zoom value for game field

    [Header("Rotate Components")]
    public float rotateSpeed;                       // game field's rotation speed
    private Quaternion rotateInput;                 // input's rotation value

    private void Update()
    {
        // get zooming touch input
        ZoomInput();

        // get rotating touch input
        RotateInput();
    }

    // get two fingers pinch zoom input to scale field object up/down
    private void ZoomInput()
    {
        if (Input.touchCount == 2)
        {
            // get the two inputs reference
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // find the previous touch positions
            Vector2 touchZeroPrevPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPosition = touchOne.position - touchOne.deltaPosition;

            // find distance between touches for the current and previous frame
            float prevTouchDist = (touchZeroPrevPosition - touchOnePrevPosition).magnitude;
            float touchDist = (touchZero.position - touchOne.position).magnitude;

            // difference between touch distances at different frames
            float pinchMagnitude = touchDist - prevTouchDist;

            // set game field's scale according to the input results
            currentZoom += pinchMagnitude * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            GameManager.instance.currentScale = currentZoom;
            fieldObject.transform.localScale = Vector3.one * currentZoom;
        }
    }

    // get slide horizontal touch input to rotate object
    private void RotateInput()
    {
        // only rotate if there is one touch input
        if (Input.touchCount == 1)
        {
            // get first input touch
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // cancel rotation input if there is a maze player object to receive the drag input instead
                if (FindObjectOfType<MazePlayer>() != null)
                {
                    return;
                }

                rotateInput = Quaternion.Euler(0f, -touch.deltaPosition.x * rotateSpeed * Time.deltaTime, 0f);

                fieldObject.transform.rotation = rotateInput * fieldObject.transform.rotation;
            }
        }
    }
}
