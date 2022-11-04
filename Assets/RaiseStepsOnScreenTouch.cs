using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaiseStepsOnScreenTouch : MonoBehaviour, IPointerClickHandler
{
    Animator anim;
    public string operateButton = "Steps1Raise";
    public GameObject stepsRaiser;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("RaiseSteps1").GetComponent<Animator>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        audioManager.PlayAudio(audioManager.clipapert);
        Debug.Log("Raise Steps touched");
        anim.SetTrigger(operateButton);
        stepsRaiser.SetActive(false);
    }
}
