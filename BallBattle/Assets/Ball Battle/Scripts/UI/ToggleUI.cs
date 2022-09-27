using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject toggleOn;         // toggle display when on
    public GameObject toggleOff;        // toggle display when off
    public bool toggleStatus = false;   // toggle display status

    // update toggle status
    public void SetToggle(bool isActive)
    {
        toggleStatus = isActive;

        // set toggle UI display
        toggleOff.SetActive(!isActive);
        toggleOn.SetActive(isActive);
    }

    // change current toggle settings
    public void SwitchToggle()
    {
        toggleStatus = !toggleStatus;

        // set toggle UI display
        if (toggleStatus)
        {
            toggleOff.SetActive(false);
            toggleOn.SetActive(true);
        }
        else
        {
            toggleOff.SetActive(true);
            toggleOn.SetActive(false);
        }
    }
}
