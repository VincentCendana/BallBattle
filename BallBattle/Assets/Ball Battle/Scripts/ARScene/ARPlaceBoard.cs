using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class ARPlaceBoard : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject visualizerButton;                                     // UI button to show/hide AR planes visualization
    public TextMeshProUGUI placeBoardPrompt;                                // text UI prompt to place game board/field

    [Header("Game Components")]
    public ARSceneToggles sceneToggles;                                     // AR scene toggle controller
    public ARPlaneManager planeManager;                                     // AR scene's plane manager component
    public ARRaycastManager raycastManager;                                 // AR system's raycast manager
    public GameObject boardObjects;                                         // the game board's objects to be placed on a surface
    private Vector2 touchPosition;                                          // player's touch input position

    [Header("AR Status")]
    private bool objectPlaced = false;                                      // object status to determine if the plane has been moved to a surface
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();      // list of AR raycast hits from touch input

    private void Awake()
    {
        // disable game board at start
        boardObjects.SetActive(false);

        // prompt to place game field
        placeBoardPrompt.gameObject.SetActive(true);
    }

    private void Update()
    {
        // touch input to setup game field position if it is not yet placed
        if (!objectPlaced)
        {
            FieldSetup();
        }
    }

    private void FieldSetup()
    {
        // cancel raycast if no touch input
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        // set raycast from touch position
        if (raycastManager.Raycast(touchPosition, hits, trackableTypes: TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (!objectPlaced)
            {
                // set object position
                boardObjects.transform.position = hitPose.position;
                boardObjects.transform.rotation = hitPose.rotation;

                // enable object
                boardObjects.SetActive(true);

                // disable AR placement object renderers
                if (sceneToggles.visualizerToggle.toggleStatus)
                {
                    sceneToggles.ToggleVisualizer();
                }

                visualizerButton.SetActive(false);
                placeBoardPrompt.gameObject.SetActive(false);

                planeManager.enabled = false;

                // update object placed status
                objectPlaced = true;
            }
        }
    }

    // get player's touch input position
    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        else
        {
            touchPosition = default;
            return false;
        }
    }

    // place and activate the game field components on the assigned transform
    private void PlaceObject (Transform objTransform)
    {
        boardObjects.transform.SetParent(objTransform);
        boardObjects.transform.localPosition = Vector3.zero;
        boardObjects.transform.rotation = Quaternion.identity;
        boardObjects.transform.localScale = Vector3.one;

        boardObjects.SetActive(true);
    }
}
