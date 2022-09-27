using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Header("UI Components")]
    public InputTouchScreen inputTouchScreen;           // UI component to receive player's touch input

    // enable/disable player's touch input
    public void SetTouchInputStatus(bool status)
    {
        inputTouchScreen.enabled = status;
    }
}
