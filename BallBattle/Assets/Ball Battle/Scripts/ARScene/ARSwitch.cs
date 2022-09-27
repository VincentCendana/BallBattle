using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class ARSwitch : MonoBehaviour
{
    [Header("UI Components")]
    public ToggleUI arModeToggle;               // Toggle to disable/enable AR mode

    [Header("Scene Information")]
    public string mainScene;                    // scene name for the default game scene
    public string arScene;                      // scene name for the AR version
    private string currentScene;                // current scene name

    [Header("Toggle Status")]
    private bool permissionTabActive = false;   // game's status of getting user's camera permission

    private void Awake()
    {
        // set initial scene data
        currentScene = SceneManager.GetActiveScene().name;
        UpdateARToggle();
    }

    // update AR toggle switch based on current scene
    public void UpdateARToggle()
    {
        arModeToggle.SetToggle(currentScene == arScene);
    }

    // switch scene to AR/Normal Mode
    public void SwitchGameScene()
    {
        // adjust ar toggle
        arModeToggle.SwitchToggle();

        // load the selected scene
        StartCoroutine(SetGameScene());
    }

    // get camera permission status and ar toggle status to select the appropriate scene
    private IEnumerator SetGameScene()
    {
        // update scene after a delay
        yield return new WaitForSeconds(0.5f);

        // load AR mode if AI toggle is set to on, otherwise load the normal scene
        if (arModeToggle.toggleStatus)
        {
            // get camera permission if user has not allowed camera access
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                GetCameraPermission();
            }
            else if (currentScene != arScene)
            {
                // load AR scene
                currentScene = arScene;
                UpdateARToggle();
                SceneManager.LoadScene(arScene);
            }
        }
        else if (currentScene != mainScene)
        {
            currentScene = mainScene;
            UpdateARToggle();
            SceneManager.LoadScene(mainScene);
        }

        yield break;
    }

    // get Android Native Camera permission if user has not allowed access yet
    public void GetCameraPermission()
    {
        // get camera permission
        Permission.RequestUserPermission(Permission.Camera);

        // set current status to getting camera permission
        permissionTabActive = true;
    }

    // function called when the application gains back focus after the permission tab is closed
    // load the appropriate scene if the permission settings is changed
    // load the main scene if user denied the permission, otherwise load the AR scene
    private void OnApplicationFocus(bool focus)
    {
        if (focus && permissionTabActive)
        {
            // reset getting permission status
            permissionTabActive = false;

            // load appropriate scene based on current settings (AR or normal mode)
            if (Permission.HasUserAuthorizedPermission(Permission.Camera) && currentScene != arScene)
            {
                currentScene = arScene;
                UpdateARToggle();
                SceneManager.LoadScene(arScene);
            }
            else if (currentScene != mainScene)
            {
                currentScene = mainScene;
                UpdateARToggle();
                SceneManager.LoadScene(mainScene);
            }
        }
    }
}
