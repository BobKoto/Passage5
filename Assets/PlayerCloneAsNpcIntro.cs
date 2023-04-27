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
    [Header("Text Cloud Events")]
    public CloudTextEvent m_CloudTextEvent;  //for TextCloud 
    public CanvasNextPagePressedEvent m_CanvasNextPagePressedEvent;
    public CloudTextEventWaitNextPage m_CloudTextEventWaitNextPage;
    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;
    [Header("Intro Duration")]
    public float introDuration = 4;
    [Header("The UI stuff as GameObjects")]
    public GameObject nextPage;
    public GameObject nowPlay;

    const string playerCloneAsNPCSpeaks1 = "#Hello. Pardon the #'s - a former employer";
    const string playerCloneAsNPCSpeaks2 = "#My new job is puzzles... \n #Lead on!";
 
    int originalCamOnPlayerCloneAsNPCPriority;
    //public CloudTextEvent m_CloudTextEvent;
    bool nextPagePressed;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello from PlayerCloneAsNpcIntro");

        originalCamOnPlayerCloneAsNPCPriority = camOnPlayerCloneAsNPC.Priority;

        if (m_CloudTextEvent == null)
            m_CloudTextEvent = new CloudTextEvent();
       // CloudTextEvent.AddListener(OnCanvasNextPagePressedEvent);

        if (m_CanvasNextPagePressedEvent == null)
            m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();
        m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);

        if (m_CloudTextEventWaitNextPage == null)
            m_CloudTextEventWaitNextPage = new CloudTextEventWaitNextPage();
        // m_CloudTextEventWaitNextPage.AddListener(EnableTheTextCloudAndWaitForNextPage);
        if (nowPlay) nowPlay.SetActive(false);
        if (nextPage) nextPage.SetActive(false);
        StartCoroutine(Intro(introDuration));
    }

    void TellTextCloud(string caption)
    {
        m_CloudTextEvent.Invoke(5, 4, caption);
    }
    void TellTextCloud(string caption, bool waitForNextPagePressed)
    {
        m_CloudTextEventWaitNextPage.Invoke(5, 4, caption, true);
    }

    IEnumerator Intro(float duration)
    {
        //  yield return new WaitForSeconds(2f); //timing?  it works but I don't like (script execution order better solution? warily yes)
        Debug.Log(" PlayerCloneAsNpcIntro Execute IEnumerator Intro(float duration)");
        playerArmature.SetActive(false);
        inputControls.SetActive(false);
        camOnPlayerCloneAsNPC.Priority = 12;
        TellTextCloud(playerCloneAsNPCSpeaks1, true);
        if (nextPage) nextPage.SetActive(true);
        //  yield return new WaitForSeconds(duration);
        yield return new WaitUntil(() => nextPagePressed); 
        playerArmature.SetActive(true);
       // inputControls.SetActive(true);
        playerCloneAsNPC.SetActive(false);
        camOnPlayerCloneAsNPC.Priority = originalCamOnPlayerCloneAsNPCPriority;
        TellTextCloud(playerCloneAsNPCSpeaks2);
       // if (nowPlay) nowPlay.SetActive(true);
        if (nextPage) nextPage.SetActive(false);
    }
    void OnCanvasNextPagePressedEvent()
    {
        Debug.Log(this.name + " says next page pressed Remve Listener");
        nextPagePressed = true;
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
    }
    public void OnCanvasNextPagePressed()
    {
        m_CanvasNextPagePressedEvent.Invoke();
    }

    void OnDisable()
    {
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
        StopAllCoroutines();
    }

}
