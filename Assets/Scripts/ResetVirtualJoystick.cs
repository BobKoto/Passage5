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

    //private void OnEnable()
    //{
    //    // Get the gamepad device
    //    gamepad = Gamepad.current;
    //}
    private void Start()
    {
        Debug.Log(" hello from REsetVirtualJoystick  with Touch Mimic");
        joystickAction = new InputAction("Joystick", binding: "<Gamepad>/leftStick");
        joystickAction.Enable();
    }

    private void ResetJoystick()
    {
        Debug.Log("ResetVJ recvd MontyST SendMessage.... trying to set jstick to Vector2.zero");
        moveHandle.localPosition =  moveHandlePosition;//resets position but dracula stays alive and keeps moving  - oh well :|
        Debug.Log("RVJ did moveHandle.position =  moveHandlePosition; =" + moveHandlePosition);

        InputAction moveAction = new InputAction("Move", InputActionType.Value, "Gamepad/leftStick");// " < Gamepad>/leftStick");
        moveAction.Enable();
        moveAction.ApplyBindingOverride("leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        Debug.Log
            ("moveAction.BindingDisplayString is " + moveAction.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice));

        // Now try mimic touch - we need x/y coord   (Gamepad.anything doesn't work for virtual GPs)
        // Start touch.
        InputSystem.QueueStateEvent(Touchscreen.current,
            new UnityEngine.InputSystem.LowLevel.TouchState { touchId = 1, phase = TouchPhase.Began, position = new Vector2(194f, 199f) });

        // Move touch.
        InputSystem.QueueStateEvent(Touchscreen.current,
            new UnityEngine.InputSystem.LowLevel.TouchState { touchId = 1, phase = TouchPhase.Moved, position = new Vector2(191f, 178f) });

        // End touch.
        InputSystem.QueueStateEvent(Touchscreen.current,
            new UnityEngine.InputSystem.LowLevel.TouchState { touchId = 1, phase = TouchPhase.Ended, position = new Vector2(191f, 178f) });

        // or try this...
        // `StateEvent.From` creates a temporary buffer in unmanaged memory that holds  
        // a state event large enough for the given device and contains a memory
        // copy of the device's current state.

        //UnityEngine.InputSystem.LowLevel.InputEventPtr eventPtr;
        //using (UnityEngine.InputSystem.LowLevel.StateEvent.From(Touchscreen.current, out eventPtr))
        //{
        //    ((AxisControl)Touchscreen.current["Move"]).WriteValueIntoEvent(0.5f, eventPtr);   //gamepad["leftStick"]
        //    InputSystem.QueueEvent(eventPtr);
        //}
    }

    //private void OnDisable()
    //{
    //    joystickAction.Disable();
    //    moveAction.Disable();
    //}
} //end class 

//// Get a reference to the Gamepad device
//Gamepad gamepad = Gamepad.current;

//// Check if the Gamepad device exists
//if (gamepad != null)
//{
//    // Get a reference to the leftStick control on the Gamepad device
//    AxisControl leftStick = gamepad.leftStick;

//    // Create a new input event
//    InputEventPtr eventPtr = InputEventPtr.Zero;
//    InputEvent.Create(PointerMoveEvent.Type, Allocator.Temp, out eventPtr);

//    // Set the value of the leftStick control in the event
//    leftStick.WriteValueIntoEvent(new Vector2(0.5f, 0.5f), eventPtr);

//    // Queue the event
//    InputSystem.QueueEvent(eventPtr);
//}



//            ((AxisControl)Touchscreen.current["myControl"]).WriteValueIntoEvent(0.5f, eventPtr);   //gamepad["leftStick"]
//  InputAction moveAction2 = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");// " < Gamepad>/leftStick");
//   moveAction2.Enable();
//   moveAction2.ApplyBindingOverride("leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
//  Debug.Log("moveAction.ApplyBindingOverride" + "(leftStick [Gamepad]" + "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");