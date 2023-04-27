using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ResetVirtualJoystick : MonoBehaviour
{
    //Component of UI_Virtual_Joystick_Move
    //public InputAction joystickAction;
    //public InputAction moveAction;
    //InputControl inputControl;
    //InputActionReference moveActionReference;
    public RectTransform moveHandle;  //resets position but dracula stays alive and keeps moving 
    Vector2 moveHandlePosition = Vector2.zero;
    private Gamepad gamepad;
    private void Start()
    {
    }

    private void OnEnable()
    {
        // Get the gamepad device
        gamepad = Gamepad.current;
    }
    private void ResetJoystick()
    {
        Debug.Log("ResetVJ recvd MontyST SendMessage.... trying to set jstick to Vector2.zero");
        moveHandle.localPosition =  moveHandlePosition;//resets position but dracula stays alive and keeps moving  - oh well :|
        Debug.Log("RVJ did moveHandle.position =  moveHandlePosition;");
        InputAction moveAction = new InputAction("Move", InputActionType.PassThrough, "Gamepad/leftStick");// " < Gamepad>/leftStick");
        InputAction moveAction2 = new InputAction("Move", InputActionType.PassThrough, "<Gamepad>/leftStick");// " < Gamepad>/leftStick");
        moveAction.Enable();
        moveAction.ApplyBindingOverride("leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        moveAction2.Enable();
        moveAction2.ApplyBindingOverride("leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        Debug.Log("moveAction.ApplyBindingOverride" + "(leftStick [Gamepad]" + "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        Debug.Log
            ("moveAction.BindingDisplayString is " + moveAction.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice));
    }
    //private void OnEnable()
    //{
    //    joystickAction.Enable();
    //    moveAction.Enable();
    //}
    //private void OnDisable()
    //{
    //    joystickAction.Disable();
    //    moveAction.Disable();
    //}
} //end class 
/* 
 * as of 4/27/23 we try a reboot on this 
 * public class ResetVirtualJoystick : MonoBehaviour
{
    //Component of UI_Virtual_Joystick_Move
    //public InputAction joystickAction;
    //public InputAction moveAction;
    //InputControl inputControl;
    //InputActionReference moveActionReference;
    //public RectTransform moveHandle;  //resets position but dracula stays alive and keeps moving 
    //Vector2 moveHandlePosition = Vector2.zero;
    private Gamepad gamepad;
    private void Start()
    {
        //  moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
        // inputControl = a
    }
    //private void OnEnable()
    //{
    //    joystickAction.Enable();
    //    moveAction.Enable();
    //}
    //private void OnDisable()
    //{
    //    joystickAction.Disable();
    //    moveAction.Disable();
    //}
    private void OnEnable()
    {
        // Get the gamepad device
        gamepad = Gamepad.current;
    }
    private void ResetJoystick()
    {
        Debug.Log("ResetVJ recvd MontyST SendMessage.... trying to set jstick to Vector2.zero");
        //moveHandle.position =  moveHandlePosition;//resets position but dracula stays alive and keeps moving  - oh well :|
        // InputAction moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick", null,null,null);
        //InputAction moveAction = new InputAction("MoveStop", binding: "<Gamepad>/leftStick");

        InputAction moveAction = new InputAction("Move", InputActionType.PassThrough, "Gamepad/leftStick");// " < Gamepad>/leftStick");
        moveAction.Enable();
       // moveAction             ["<Gamepad>/leftStick"].SetValue(Vector2.zero);

        //  moveAction.actionMap                           //Set(Vector2.zero);
      //  moveAction.ApplyBindingOverride("Gamepad/leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        moveAction.ApplyBindingOverride("leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        Debug.Log("moveAction.ApplyBindingOverride" + "(leftStick [Gamepad]" + "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        //Debug.Log(" binding index = " + moveAction.GetBindingIndex());
        //Debug.Log(" activeControl = " + moveAction.activeControl);
        ////moveAction.name
        Debug.Log
            ("moveAction.BindingDisplayString is " + moveAction.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice));

        // Create a new input event with a Vector2 value of (0,0)
        //  var inputEvent = new InputEvent<Vector2>(gamepad.leftStick, new Vector2(0, 0));
        //var inputEvent = new InputEvent      (gamepad.leftStick, new Vector2(0, 0));

        //// Trigger the input event to set the value of the left stick to (0,0)
        //InputSystem.QueueEvent(inputEvent);

    }
} //end class 
*/
/*
InputAction moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
moveAction.Enable();
moveAction.ApplyBindingOverride("<Gamepad>/leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
//--------------------------
InputAction moveAction = new InputAction("Move", InputActionType.Value, "LS", "Gamepad");
moveAction.Enable();
moveAction.ApplyBindingOverride("LS", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");

*/
/*
 * ORIGINAL to @35
 *         InputAction moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
        //InputAction moveAction = new InputAction("Move", InputActionType.Value, "Left Stick [Gamepad]");
        moveAction.Enable();

        // ...

        // Reset the binding of the Move action to Vector2.zero
        // moveAction.ApplyBindingOverride("<Gamepad>/leftStick", "<Vector2> = (0,0)");
        moveAction.ApplyBindingOverride("<Gamepad>/leftStick", "<Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
        moveAction.ApplyBindingOverride("<Gamepad>/leftStick:disabled", "<Gamepad>/leftStick");
        moveAction.ApplyBindingOverride("<Gamepad>/leftStick", "");
 * */
//public static void ApplyBindingOverride(this InputAction action, int bindingIndex, string path);
//public static void ApplyBindingOverride(this InputAction action, string newPath, string group = null, string path = null);
//public static void ApplyBindingOverride(this InputAction action, InputBinding bindingOverride);
//public static void ApplyBindingOverride(this InputAction action, int bindingIndex, InputBinding bindingOverride);

//public static int ApplyBindingOverride(this InputActionMap actionMap, InputBinding bindingOverride);
//public static void ApplyBindingOverride(this InputActionMap actionMap, int bindingIndex, InputBinding bindingOverride);
//public static void ApplyBindingOverrides(this InputActionMap actionMap, IEnumerable<InputBinding> overrides);

//public static string GetBindingDisplayString(this InputAction action, InputBinding.DisplayStringOptions options = 0, string group = null);
//public static string GetBindingDisplayString(this InputAction action, InputBinding bindingMask, InputBinding.DisplayStringOptions options = 0);
//public static string GetBindingDisplayString(this InputAction action, int bindingIndex, InputBinding.DisplayStringOptions options = 0);
//public static string GetBindingDisplayString(this InputAction action, int bindingIndex, out string deviceLayoutName, out string controlPath, InputBinding.DisplayStringOptions options = 0);

//from the only method - none of this worked :(
//var newInputControl = InputSystem.GetDevice<Gamepad> ();
//if (newInputControl != null)
//{
//    inputControl = newInputControl;
//}
// Debug.Log("newIC devID = " + inputControl.device);
// Debug.Log("Dev Name = " + inputControl.displayName);
// newInputControl.leftStick.device
// newInputControl.Set(new Vector2(0f, 0f));

// Reset the binding of the Move action to Vector2.zero
// moveAction.ApplyBindingOverride(1, "<Gamepad>/leftStick", "<Vector2> = (0,0)");
//moveAction.ApplyBindingOverride("<Gamepad>/leftStick", "<Vector2> = (0,0)");
//if (InputSystem.TryResetDevice(Gamepad.current)) Debug.Log("try reset ok"); else Debug.Log("try reset failed");
// throws ArgumentNullException: Value cannot be null.

//moveAction.ApplyBindingOverride("<Gamepad>/leftStick:disabled", "<Gamepad>/leftStick");
//  moveAction.ApplyBindingOverride("<Gamepad>/leftStick", null);  //was ""
// moveAction.ApplyBindingOverride("UI_Virtual_Joystick_Move", "");  //was ""

//next time try UI_Virtual_Joystick_Move?????

// moveAction.ApplyBindingOverride("Left Stick[Gamepad]", " < Vector2> = (0,0)");

// ...
// Show all gamepads in the system.
// Debug.Log("Show all gamepads in the system.  " + string.Join("\n", Gamepad.all));  //nothing...
//joystickAction.ReadValue<Vector2>();
//joystickAction.ApplyBindingOverride("<Vector2> = none");
//Move.ReadValue<Vector2>();
//Move.ApplyBindingOverride("<Vector2> = none");
//============== probing moveAction variables -- keep if only for the foreach syntax!
//InputBinding binding;
//string bindingString = null;
//foreach (InputBinding b in moveAction.bindings)
//{
//    Debug.Log(" IPbinding is " + b);
//    binding = b;
//    bindingString = b.ToString();
//}
//Debug.Log("param to override " + bindingString + " < Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
//moveAction.ApplyBindingOverride(bindingString, "< Vector2>{" + Vector2.zero.x + "," + Vector2.zero.y + "}");
//InputControl ipControl;
//string ipControlString = null;
//Debug.Log("moveAction.controls length/Count = " + moveAction.controls.Count);
//foreach (InputControl i in moveAction.controls)
//{
//    Debug.Log(" IPcontrol is " + i);
//    ipControl = i;
//    ipControlString = i.ToString();
//}