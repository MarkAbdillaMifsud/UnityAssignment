using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckForJoystick : MonoBehaviour
{
    public Image moveKeysKeyboard;
    public Image moveKeysJoystick;
    public Image fireKeyKeyboard;
    public Image fireKeyJoystick;

    private void Start()
    {
        string[] joystickNames = Input.GetJoystickNames();

        if (joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]))
        {
            moveKeysJoystick.gameObject.SetActive(true);
            fireKeyJoystick.gameObject.SetActive(true);
            moveKeysKeyboard.gameObject.SetActive(false);
            fireKeyKeyboard.gameObject.SetActive(false);
        }
        else
        {
            moveKeysKeyboard.gameObject.SetActive(true);
            fireKeyKeyboard.gameObject.SetActive(true);
            moveKeysJoystick.gameObject.SetActive(false);
            fireKeyJoystick.gameObject.SetActive(false);
        }
    }
}
