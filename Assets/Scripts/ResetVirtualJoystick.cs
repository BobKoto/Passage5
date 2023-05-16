using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ResetVirtualJoystick : MonoBehaviour
{
    //Component of UI_Virtual_Joystick_Move
    public RectTransform moveHandle;  //resets position but dracula stays alive and keeps moving 
    Vector2 moveHandlePosition = Vector2.zero;

    [SerializeField] private InputAction joystickAction;

    private void Start()
    {
        Debug.Log(" hello from REsetVirtualJoystick  with Touch Mimic");
        joystickAction = new InputAction("Joystick", binding: "<Gamepad>/leftStick");
        joystickAction.Enable();
    }
    private void ResetJoystick()
    {
        Debug.Log("ResetVJ recvd MontyST SendMessage.... set jstick to Vector2.zero  Then mimic a slight move...");
        moveHandle.localPosition =  moveHandlePosition;//resets position but dracula stays alive and keeps moving  - oh well :|
       // Debug.Log("RVJ did moveHandle.position =  moveHandlePosition; =" + moveHandlePosition);
        InputAction moveAction = new InputAction("Move", InputActionType.Value, "Gamepad/leftStick");// " < Gamepad>/leftStick");
        moveAction.Enable();
        moveAction.ApplyBindingOverride("leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
       // Debug.Log("moveAction.BindingDisplayString is " + moveAction.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice));
        // Now mimic touch - we fixed x/y coord of jstick (Gamepad.anything doesn't work for virtual GPs) 
        // Start touch.  This and the next 2 statements work!
        InputSystem.QueueStateEvent(Touchscreen.current,
            new UnityEngine.InputSystem.LowLevel.TouchState { touchId = 1, phase = TouchPhase.Began, position = new Vector2(194f, 199f) });
        // Move touch.
        InputSystem.QueueStateEvent(Touchscreen.current,
            new UnityEngine.InputSystem.LowLevel.TouchState { touchId = 1, phase = TouchPhase.Moved, position = new Vector2(191f, 178f) });
        // End touch.
        InputSystem.QueueStateEvent(Touchscreen.current,
            new UnityEngine.InputSystem.LowLevel.TouchState { touchId = 1, phase = TouchPhase.Ended, position = new Vector2(191f, 178f) });
    }
    //private void OnDisable()
    //{
    //    joystickAction.Disable();
    //    moveAction.Disable();
    //}
} //end class 
