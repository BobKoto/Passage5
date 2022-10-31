using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem.EnhancedTouch;

public class ActOnTouch : MonoBehaviour, IPointerClickHandler  //, IPointerDownHandler, IPointerUpHandler
{
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
       // audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        audioManager.PlayAudio(audioManager.clipapert);
        Debug.Log("Sphere touched");
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
    //protected void OnEnable()
    //{
    //    Debug.Log(" EnhancedTouchSupport.Enable()");
    //    EnhancedTouchSupport.Enable();
    //}
    //protected void OnDisable()
    //{
    //    EnhancedTouchSupport.Disable();
    //}
}
