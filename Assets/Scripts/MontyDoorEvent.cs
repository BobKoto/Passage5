//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class MontyDoorEvent : MonoBehaviour
{
    public MontyDoorDownEvent montyDoorDownEvent;
    public void AlertObservers(string message)
    {
        Debug.Log(this.name + "  received Animation event received by MontyDoorEvent... " + message);
        if (message.Equals("Door1DownFinished"))
        {
            // Do other things based on an animation ending.
            Debug.Log(this.name + "  received Animation event received by MontyDoorEvent... " + message);
        }
        int x = DoorNumberToSend();
        montyDoorDownEvent.Invoke(x);   //we did try to use DoorNumberToSend() instead of x 
        Debug.Log("Invoked event with door # to send... " + DoorNumberToSend());
    }
    int DoorNumberToSend()
    {
        int _doorNumber = 88;
        switch (this.name)
        {
            case "MontySlidingDoor1":
                _doorNumber = 1;
                break;
            case "MontySlidingDoor2":
                _doorNumber = 2;
                break;
            case "MontySlidingDoor3":
                _doorNumber = 3;
                break;
        }
        return _doorNumber;
    }
}
