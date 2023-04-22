using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class PlayerCloneAsNpcIntro : MonoBehaviour
{//Component of PlayerCloneAsNPC

    [Header("The Player(s)")]
    public GameObject playerArmature;
    public GameObject playerCloneAsNPC;
    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera camOnPlayerCloneAsNPC;
    [Header("Text Cloud Event")]
    public MyIntEvent m_MyEvent;  //for TextCloud 
    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;
    [Header("Intro Duration")]
    public float introDuration = 4;

    int originalCamOnPlayerCloneAsNPCPriority;

    const string playerCloneAsNPCSpeaks1 = "#Hello. Pardon the #'s - a former employer";
    const string playerCloneAsNPCSpeaks2 = "#My new job is puzzles... \n #Lead on!";

    public CanvasNextPagePressedEvent canvasNextPagePressedEvent;
    bool nextPagePressed;
    // Start is called before the first frame update
    void Start()
    {
        if (canvasNextPagePressedEvent == null)
            canvasNextPagePressedEvent = new CanvasNextPagePressedEvent();
        canvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);
        originalCamOnPlayerCloneAsNPCPriority = camOnPlayerCloneAsNPC.Priority;
        StartCoroutine(Intro(introDuration));
    }

    void TellTextCloud(string caption)
    {
        m_MyEvent.Invoke(5, 4, caption);
    }
    void TellTextCloud(string caption, bool nextPagePressed)
    {
        m_MyEvent.Invoke(5, 4, caption);
    }

    IEnumerator Intro(float duration)
    {
      //  yield return new WaitForSeconds(2f); //timing?  it works but I don't like (script execution order better solution? warily yes)
        playerArmature.SetActive(false);
        inputControls.SetActive(false);
        camOnPlayerCloneAsNPC.Priority = 12;
        TellTextCloud(playerCloneAsNPCSpeaks1);
      //  yield return new WaitForSeconds(duration);
        yield return new WaitUntil(() => nextPagePressed); 
        playerArmature.SetActive(true);
        inputControls.SetActive(true);
        playerCloneAsNPC.SetActive(false);
        camOnPlayerCloneAsNPC.Priority = originalCamOnPlayerCloneAsNPCPriority;
        TellTextCloud(playerCloneAsNPCSpeaks2);
    }
    void OnCanvasNextPagePressedEvent()
    {
        Debug.Log("next page pressed");
        nextPagePressed = true;
    }
    public void OnCanvasNextPagePressed()
    {
        canvasNextPagePressedEvent.Invoke();
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}

}
