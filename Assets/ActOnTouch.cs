using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActOnTouch : MonoBehaviour, IPointerClickHandler
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
    public void ObjectTouched()
    {
        Debug.Log("Sphere touched  in ObjectTouched");
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
