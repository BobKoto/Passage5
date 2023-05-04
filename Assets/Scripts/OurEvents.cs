using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Find this in Assets\Scripts
// Here we keep a master list of Events

[System.Serializable]
public class CanvasNextPagePressedEvent : UnityEvent { }
[System.Serializable]
public class CloudTextEvent : UnityEvent<int, int, string>{ }
[System.Serializable]
public class CloudTextEventExtinguished : UnityEvent { }
[System.Serializable]
public class CloudTextEventWaitNextPage : UnityEvent<int, int, string, bool> { }
[System.Serializable]
public class MontyPlayButtonTouchEvent : UnityEvent { }

[System.Serializable]
public class MontyDoorTouchEvent : UnityEvent<int> { }
//[System.Serializable]
//public class MontyPlayButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyMoveOnButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyDoorDownEvent : UnityEvent<int> { }
[System.Serializable]
public class AudioClipFinishedEvent : UnityEvent { }
[System.Serializable]
public class CubeGameBoardUpEvent : UnityEvent { }  //we receive these from CubeGameBoard // 5/3/23 moved to OurEvents.cs 

[System.Serializable]   //moved these 3 to OurEvents.cs 5/3/23
public class CubeGameBoardEvent : UnityEvent<string, bool, string, int> { }  //this declaration i guess is needed to accept
[System.Serializable]
public class CubeGamePlayButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class CubeGameMoveOnButtonTouchEvent : UnityEvent { }

[System.Serializable]
public class PlayerMoveStopEvent : UnityEvent { }   // We shouldn't need anymore since we have CallResetJoystick()

//Find this in Assets\Scripts
