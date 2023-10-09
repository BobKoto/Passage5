using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
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
    CloudTextExtinguishedEvent m_CloudTextExtinguishedEvent;
    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;
    [Header("Intro Duration")]
    public float introDuration = 4;
    [Header("The UI stuff as GameObjects")]
    public GameObject nextPage;
    public GameObject nowPlay;
    public GameObject startAvatarIntro;
    public GameObject parachuteButton;
    public CanvasGroup imageCanvasGroup; // Reference to the button's CanvasGroup

    const string playerCloneAsNPCSpeaks1 = "#Hello. Pardon the #'s. \n#A former employer.";
    const string playerCloneAsNPCSpeaks2 = "#My new job is yours to figure out.\n #Lead on!";
    const string playerCloneAsNPCSpeaks3 = "#OK. Forward!";
    int originalCamOnPlayerCloneAsNPCPriority;
    bool nextPagePressed, waitingNextPagePress, waitingForTextExtinguishEvent, testExtinguishedReceived;
    AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        Debug.Log("Hello from PlayerCloneAsNpcIntro");

        originalCamOnPlayerCloneAsNPCPriority = camOnPlayerCloneAsNPC.Priority;

        if (m_CloudTextEvent == null)
            m_CloudTextEvent = new CloudTextEvent();

        if (m_CanvasNextPagePressedEvent == null)
            m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();

        if (m_CloudTextExtinguishedEvent == null)
            m_CloudTextExtinguishedEvent = new CloudTextExtinguishedEvent();

        if (nowPlay) nowPlay.SetActive(false);
        if (nextPage) nextPage.SetActive(false);
        if (parachuteButton) parachuteButton.SetActive(false);
        imageCanvasGroup = startAvatarIntro.GetComponent<CanvasGroup>();
        if (startAvatarIntro) startAvatarIntro.SetActive(true);
    }
    public void OnStartAvatarIntroButtonPress()
    {
        StartCoroutine(FadeImage());
        StartCoroutine(Intro());
    }
    IEnumerator FadeImage()
    {
        float alphaSetting = 1f;
        while (alphaSetting > 0)
        {
            alphaSetting -= .1f;
            imageCanvasGroup.alpha = alphaSetting;
            yield return new WaitForSeconds(.1f);
        }
        startAvatarIntro.SetActive(false);
        yield return null;
    }
    IEnumerator Intro()
    {
        playerArmature.SetActive(false);
        inputControls.SetActive(false);
        camOnPlayerCloneAsNPC.Priority = 12;  //6/10/23 change from 25 back to 12 
        TellTextCloud(playerCloneAsNPCSpeaks1, true);
        waitingForTextExtinguishEvent = true;
        m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);
        yield return new WaitUntil(() => !waitingForTextExtinguishEvent);  //when TCHandler sees NextPage press and extinguishes text cloud 
        playerArmature.SetActive(true);

        skinnedMeshRendererPlayerCloneAsNPC.enabled = false;  //5/26/23 so instead we do this 
        camOnPlayerCloneAsNPC.Priority = originalCamOnPlayerCloneAsNPCPriority;

        TellTextCloud(playerCloneAsNPCSpeaks2, true);
        waitingForTextExtinguishEvent = true;
        m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);
        yield return new WaitUntil(() => !waitingForTextExtinguishEvent);//5/24/23

        if (nextPage) nextPage.SetActive(true);     //7/3/23 
        waitingNextPagePress = true;
        Missions.missions[Missions.randomlyPickedMission].SetActive(true);   
        audioManager.PlayAudio(audioManager.theetone);
        yield return new WaitUntil(() => nextPagePressed); //7/3/23 
        nextPagePressed = false; //7/4/23 keep it tidy even tho we may not use it again 
        TellTextCloud(2,playerCloneAsNPCSpeaks3);
        waitingNextPagePress = false;
        if (nextPage) nextPage.SetActive(false);           //7/3/23 

        if (inputControls) inputControls.SetActive(true);
        playerCloneAsNPC.SetActive(false); //7/3/23 TIL Setting this false calls OnDisable() then continues to run! it killed this coroutine... 

    }
    void TellTextCloud(int timeout, string caption) => m_CloudTextEvent.Invoke(5, timeout, caption);  //(int cloudBehavior, int cloudTimeout, string _caption)
    void TellTextCloud(string caption, bool waitForNextPagePressed) => m_CloudTextEvent.Invoke(6, 4, caption);

    public void OnCanvasNextPagePressed()
    {
        if (waitingNextPagePress)  //5/25/23 
        {
            nextPagePressed = true;   //7/4/23 predicated on coroutine waiting for this AND coroutine should reset it if appropriate
        }
    }
    public void OnCloudTextExtinguishedEvent()
    {
        if (waitingForTextExtinguishEvent)  //7/4/23 if true there is a coroutine waiting for this
        {
            m_CloudTextExtinguishedEvent.RemoveListener(OnCloudTextExtinguishedEvent);  //5/24/23
            waitingForTextExtinguishEvent = false;
        }
    }
    void OnDisable()
    {
        Debug.Log(this.name + "  did OnDisable() !!!!!");
        m_CloudTextExtinguishedEvent.RemoveListener(OnCloudTextExtinguishedEvent);
        StopAllCoroutines();
        if (parachuteButton) parachuteButton.SetActive(true);
    }
}