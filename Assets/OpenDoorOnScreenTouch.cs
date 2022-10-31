using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenDoorOnScreenTouch : MonoBehaviour, IPointerClickHandler
{
    Animator anim;
    public string operateButton = "DoorSlideDown";
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("SlidingDoor1").GetComponent<Animator>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        audioManager.PlayAudio(audioManager.clipapert);
        Debug.Log("Open Door touched");
        anim.SetTrigger(operateButton);
    }
    //void Update()
    //    {

    //    }
}
