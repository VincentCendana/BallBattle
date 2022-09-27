using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARSceneToggles : MonoBehaviour
{
    [Header("Camera Components")]
    public ARCameraBackground cameraBackground;         // AI camera background to get the real world environment image

    [Header("UI Components")]
    public ToggleUI visualizerToggle;                   // toggle to enable/disable AR plane visualizer toggle
    public ToggleUI environmentToggle;                  // toggle to enable/disable real world environment render

    [Header("AR Components")]
    public ARPlaneManager planeManager;                 // AR Plane Manager component to spawn in AR planes
    public MeshRenderer defaultPlaneMesh;               // plane prefab material
    public Material visualizedMaterial;                 // Plane material to be visualized
    public Material unvisualizedMaterial;               // Plane material to be unvisualized

    private void Start()
    {
        // set initial toggle values
        environmentToggle.SetToggle(cameraBackground.isActiveAndEnabled);
        visualizerToggle.SetToggle(true);
    }

    // toggle to enable/disable real world environment render
    public void ToggleEnvironment()
    {
        // switch toggle status
        environmentToggle.SwitchToggle();

        // set camera status
        cameraBackground.enabled = environmentToggle.toggleStatus;
    }

    // toggle to enable/disable AR plane visualizer
    public void ToggleVisualizer()
    {
        // switch toggle status
        visualizerToggle.SwitchToggle();

        // enable/disable AR visualizer
        foreach (var plane in FindObjectsOfType<ARPlane>())
        {
            if (visualizerToggle.toggleStatus)
            {
                plane.GetComponent<MeshRenderer>().material = visualizedMaterial;
            }
            else
            {
                plane.GetComponent<MeshRenderer>().material = unvisualizedMaterial;
            }
        }
    }
}
