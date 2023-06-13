using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using UnityEngine.Events;

public class PlayerCloneAsNpcIntro : MonoBehaviour
{//Component of PlayerCloneAsNPC  

    [Header("The Player(s)")]
    public GameObject playerArmature;
    public GameObject playerCloneAsNPC;
    public SkinnedMeshRenderer skinnedMeshRendererPlayerCloneAsNPC;
    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera camOnPlayerCloneAsNPC;
    [Header("Text Cloud Events")]
    public CloudTextEvent m_CloudTextEvent;  //for TextCloud 
    CanvasNextPagePressedEvent m_CanvasNextPagePressedEvent;
    //CloudTextWaitNextPageEvent m_CloudTextEventWaitNextPage;  //deimped in favor of using int param in CloudTextEvent to drive behavior
    CloudTextExtinguishedEvent m_CloudTextExtinguishedEvent;
    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;
    [Header("Intro Duration")]
    public float introDuration = 4;
    [Header("The UI stuff as GameObjects")]
    public GameObject nextPage;
    public GameObject nowPlay;

    const string playerCloneAsNPCSpeaks1 = "#Hello. Pardon the #'s. \n#A former employer.";
    const string playerCloneAsNPCSpeaks2 = "#My new job is yours to figure out.\n #Lead on!";
 
    int originalCamOnPlayerCloneAsNPCPriority;
    bool nextPagePressed, waitingNextPagePress, waitingForTextExtinguishEvent, testExtinguishedReceived;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        Debug.Log("Hello from PlayerCloneAsNpcIntro");

        originalCamOnPlayerCloneAsNPCPriority = camOnPlayerCloneAsNPC.Priority;

        if (m_CloudTextEvent == null)
            m_CloudTextEvent = new CloudTextEvent();

        if (m_CanvasNextPagePressedEvent == null)
            m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();
      //  m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);//5/25/23

        //if (m_CloudTextEventWaitNextPage == null)
        //    m_CloudTextEventWaitNextPage = new CloudTextWaitNextPageEvent();

        if (m_CloudTextExtinguishedEvent == null)
            m_CloudTextExtinguishedEvent = new CloudTextExtinguishedEvent();
      //  m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);//5/25/23 move to Intro()

        if (nowPlay) nowPlay.SetActive(false);
        if (nextPage) nextPage.SetActive(false);
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
      //  Debug.Log(" PlayerCloneAsNpcIntro Execute IEnumerator Intro(float duration)");
        playerArmature.SetActive(false);
        inputControls.SetActive(false);
        camOnPlayerCloneAsNPC.Priority = 12;  //6/10/23 change from 25 back to 12 
        //Debug.Log(this.name + "  camOnPlayerCloneAsNPC.Priority = 12;  should set the Active Cam to camOnPlayerCloneAsNPC *****");

        // Find the active cam
        CinemachineVirtualCameraBase[] virtualCameras = FindObjectsOfType<CinemachineVirtualCameraBase>();
        float highestPriority = float.MinValue;
        CinemachineVirtualCameraBase activeVirtualCamera = null;
        foreach (CinemachineVirtualCameraBase virtualCamera in virtualCameras)
        {
            if (virtualCamera.Priority > highestPriority)
            {
                highestPriority = virtualCamera.Priority;
                activeVirtualCamera = virtualCamera;
            }
        }
        if (activeVirtualCamera != null)
        {
            // Debug.Log(this.name + "  Active Cinemachine Camera: " + activeVirtualCamera.Name);  //6/10/23 comment for now - we may use
        }

        TellTextCloud(playerCloneAsNPCSpeaks1, true);
        waitingForTextExtinguishEvent = true;
        m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);
        yield return new WaitUntil(() => !waitingForTextExtinguishEvent);  //when TCH sees NextPage press and extinguishes text cloud 

      //  Debug.Log(this.name + " *****  GOT TextExtinguishEvent so set player active and do NEXT CLOUD ***** ");
        playerArmature.SetActive(true);

        // playerCloneAsNPC.SetActive(false);//5/25/23 this caused a bug where we kinda stopped here 
        skinnedMeshRendererPlayerCloneAsNPC.enabled = false;  //5/26/23 so instead we do this 
        camOnPlayerCloneAsNPC.Priority = originalCamOnPlayerCloneAsNPCPriority;

        TellTextCloud(playerCloneAsNPCSpeaks2, true);
        waitingForTextExtinguishEvent = true;
        m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);

       // Debug.Log(this.name + " *****  Drop into wait on !waitingForTextExtinguishEvent ***** ");
        yield return new WaitUntil(() => !waitingForTextExtinguishEvent);//5/24/23
      //  Debug.Log(this.name + " *****  Drop OUT OF wait on nextPagePressed & !waitingTextExtinguish ***** ");
        playerCloneAsNPC.SetActive(false);
        if (nextPage) nextPage.SetActive(false);

        if (nowPlay) nowPlay.SetActive(true);
        audioManager.PlayAudio(audioManager.clipapert);
        if (inputControls) inputControls.SetActive(true);

    }
    void TellTextCloud(string caption)
    {
        m_CloudTextEvent.Invoke(5, 4, caption);
    }
    void TellTextCloud(string caption, bool waitForNextPagePressed)
    {
        m_CloudTextEvent.Invoke(6, 4, caption);
    }

    void OnCanvasNextPagePressedEvent()
    {
        Debug.Log(this.name + " says next page pressed SET TRUE");
        nextPagePressed = true;
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent); //5/24/23
    }
    public void OnCanvasNextPagePressed()
    {
        if (waitingNextPagePress)  //5/25/23 
        {
         //   Debug.Log(this.name + " DOING...    m_CanvasNextPagePressedEvent.Invoke();");
            m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent); //5/24/23
            m_CanvasNextPagePressedEvent.Invoke();
        }

    }
    public void OnCloudTextExtinguishedEvent()
    {
      //  Debug.Log(this.name + " says the cloud went away... waitingTextExtinguish = " + waitingForTextExtinguishEvent);
        if (waitingForTextExtinguishEvent)
        {
      //      Debug.Log(this.name + " says RemoveListener(OnCloudTextExtinguishedEvent)  and  SEwaitingForTextExtinguishEvent = false;");
            m_CloudTextExtinguishedEvent.RemoveListener(OnCloudTextExtinguishedEvent);  //5/24/23
            waitingForTextExtinguishEvent = false;
        }
    }
    void OnDisable()
    {
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
        m_CloudTextExtinguishedEvent.RemoveListener(OnCloudTextExtinguishedEvent);
        StopAllCoroutines();
    }
}