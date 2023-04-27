using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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
public class PlayerMoveStopEvent : UnityEvent { }   // to see if we can STOP the fucking player from moving after we've done all else 
//Find this in Assets\Scripts

//We probably should move all public Events into here... for now we have these 2 and ok so far 

/*
[System.Serializable]
public class MontyPlayButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyMoveOnButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyDoorDownEvent : UnityEvent<int> { }
[System.Serializable]
public class AudioClipFinishedEvent : UnityEvent { }
*/