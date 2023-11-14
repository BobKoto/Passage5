using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using TMPro;
public class TextCloudHandler : MonoBehaviour
{//Component of TextCloudHandleHolder
    public AudioManager audioManager;
    public GameObject voiceCloud;
    public GameObject voiceCloudText;
    public GameObject thoughtCloud;  // parent of the text
    public GameObject thoughtCloudText;
    public GameObject menuButton;
    public GameObject lightButton;
    [Header("The UI stuff as GameObjects")]
    public GameObject nextPage;
    public GameObject nowPlay;

    public CloudTextExtinguishedEvent m_CloudTextExtinguishedEvent;
    bool nextPagePressed, waitingForNextPagePress, isSilentThought;

    enum CloudBehavior :int
    {
        followTimeOut =  5,
        waitForNextPagePress = 6,
        addThoughtAndFollowTimeOut = 7,
        addThoughtAndWaitForNextPagePress = 8,   //what mostly will be used when we have a thought
        waitForNextPageOrTimeout = 9  //fade out if user doesn't press NextPage
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello from TextCloudHandler");
        if (m_CloudTextExtinguishedEvent == null)
            m_CloudTextExtinguishedEvent = new CloudTextExtinguishedEvent();

        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        //audioManager.PlayAudio(audioManager.tick, .09f);

        if (menuButton) menuButton.SetActive(false);
        if (lightButton) lightButton.SetActive(false);
    }
    public void EnableTheTextCloud(int cloudBehavior, int cloudTimeout, string _caption)
    {
        voiceCloud.SetActive(false);     // just to be sure 
        thoughtCloud.SetActive(false);
        isSilentThought = ((CloudBehavior)cloudBehavior == CloudBehavior.addThoughtAndFollowTimeOut
                        || (CloudBehavior)cloudBehavior == CloudBehavior.addThoughtAndWaitForNextPagePress
                        || (CloudBehavior)cloudBehavior == CloudBehavior.waitForNextPageOrTimeout);
        if (!isSilentThought)  // just play voices
        {
            voiceCloudText.GetComponent<TextMeshProUGUI>().text = _caption;
            voiceCloud.SetActive(true);
            PlayVoice();
            // audioManager.PlayAudio(audioManager.compVoice0, 1f); //here maybe we can look for spaces in the string to adjust audio length
        }
        else // handle a thought 
        {
            thoughtCloudText.GetComponent<TextMeshProUGUI>().text = _caption;
            thoughtCloud.SetActive(true);
        }

        switch ((CloudBehavior)cloudBehavior)
        {
            case CloudBehavior.followTimeOut:
                StartCoroutine(RemoveCloudAfterXSeconds(cloudTimeout));
                break;
            case CloudBehavior.waitForNextPagePress:
                waitingForNextPagePress = true;
                StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));
                break;
            case CloudBehavior.addThoughtAndFollowTimeOut:   //11/12/23 option 7
                StartCoroutine(RemoveCloudAfterXSeconds(cloudTimeout));
                break;
            case CloudBehavior.addThoughtAndWaitForNextPagePress:    //11/12/23 option 8
                waitingForNextPagePress = true;
                StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));
                break;
            case CloudBehavior.waitForNextPageOrTimeout: //11/12/23 
                waitingForNextPagePress = true;
                StartCoroutine(RemoveCloudAfterNextPagePressOrTimeout(true,cloudTimeout));
                break;
            default:
                Debug.Log(this.name + " EnableTheTextCloud recvd INVALID Behavior code " + (CloudBehavior)cloudBehavior);
                break;
        }
    }
    void PlayVoice()
    {
        int randomVoice = Random.Range(0, 4);  //As per doc this returns 0,1,2 or 3  (not 4)

        //Debug.Log("Random audio = " + randomVoice);
        if (MontyStopTrigger.evilTwinSpeaking)
        {
            audioManager.PlayAudio(audioManager.strom, 1f);  //play evilTwin's voice
        }
        else //play voice of goodTwin or our player
            switch (randomVoice)
            {
                case 0: audioManager.PlayAudio(audioManager.compVoice0, 1f); break;
                case 1: audioManager.PlayAudio(audioManager.compVoice1, 1f); break;
                case 2: audioManager.PlayAudio(audioManager.compVoice2, 1f); break;
                case 3: audioManager.PlayAudio(audioManager.compVoice3, 1f); break;
                default: break;
            }
    }
    IEnumerator RemoveCloudAfterXSeconds(int paramCloudTimeout)
    {
        //Debug.Log(this.name + "  ****** RemoveCloudAfterXSeconds Called ***** wait timeout then deactivate voice & thought clouds");
        yield return new WaitForSeconds (paramCloudTimeout);
        voiceCloud.SetActive(false);
        thoughtCloud.SetActive(false);
        m_CloudTextExtinguishedEvent.Invoke();
    }
    IEnumerator RemoveCloudAfterNextPagePressed(bool nextPressed)
    {
        yield return new WaitForSeconds(1f);  //added 6/4/23
        if (nextPage) nextPage.SetActive(true);  //moved here 6/4/23
        yield return new WaitUntil(() => nextPagePressed);
        nextPagePressed = false;
        thoughtCloud.SetActive(false);
        voiceCloud.SetActive(false);
        if (nextPage) nextPage.SetActive(false);
      //  Debug.Log(this.name + "  ****** now invoking m_CloudTextExtinguishedEvent ***** ");
        m_CloudTextExtinguishedEvent.Invoke();
    }
    IEnumerator RemoveCloudAfterNextPagePressOrTimeout(bool nextPressed, int timeout)   //11/12/23
    {
        float startTime = Time.unscaledTime;
        float thisTime;
        bool waitTimeOver = false;
        if (nextPage) nextPage.SetActive(true);  
        while (!waitTimeOver && !nextPagePressed)
        {
            thisTime = Time.unscaledTime;
            waitTimeOver = thisTime - startTime > timeout;
            yield return null;
        }
        yield return new WaitForSeconds(1f);  //added 6/4/23
        if (nextPage) nextPage.SetActive(true);  //moved here 6/4/23


        yield return new WaitUntil(() => nextPagePressed || waitTimeOver) ;
        nextPagePressed = false;
        thoughtCloud.SetActive(false);
        voiceCloud.SetActive(false);
        if (nextPage) nextPage.SetActive(false);
        m_CloudTextExtinguishedEvent.Invoke();
    }
    public void OnCanvasNextPagePressed()
    {
        if (waitingForNextPagePress)
        {
            nextPagePressed = true;
            voiceCloud.SetActive(false);
            waitingForNextPagePress = false;
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
//public void EnableTheTextCloudAndWaitForNextPage(int x, int y, string _caption, bool waitForNextPagePressed)
//{
//    cloudText.GetComponent<TextMeshProUGUI>().text = _caption;
//    textCloud.SetActive(true);
//    audioManager.PlayAudio(audioManager.strom, 1f); //here maybe we can look for spaces in the string
//    Debug.Log(this.name + " call StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));... OLD CASE");
//    m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);  //moved here from START()
//    StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));
//}
//// Find the active cam   keep for maybe use later 
//CinemachineVirtualCameraBase[] virtualCameras = FindObjectsOfType<CinemachineVirtualCameraBase>();
//float highestPriority = float.MinValue;
//CinemachineVirtualCameraBase activeVirtualCamera = null;
//foreach (CinemachineVirtualCameraBase virtualCamera in virtualCameras)
//{
//    if (virtualCamera.Priority > highestPriority)
//    {
//        highestPriority = virtualCamera.Priority;
//        activeVirtualCamera = virtualCamera;
//    }
//}
//if (activeVirtualCamera != null)
//{
//  //  Debug.Log(this.name + "  Active Cinemachine Camera: " + activeVirtualCamera.Name);  //6/10/23 comment for now - we may use
//}
//public void OnCloudTextEventExtinguished()    //5/20/23 only invoke the event - let other scripts handle  //7/2/23 designed out I'm pretty sure 
//{
//    Debug.Log(this.name + " says the cloud went away... So enable the Play box");
//    m_CloudTextExtinguishedEvent.RemoveListener(OnCloudTextEventExtinguished);
//    if (nowPlay) nowPlay.SetActive(true);
//    if (inputControls) inputControls.SetActive(true);
//}
// //Deimped and Deleted from Start()
//if (m_CanvasNextPagePressedEvent == null)
//    m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();

//        originalCamPriority = playerFacingCamera.Priority;

// pretty sure this CAM stuff is handled elsewhere - it has been dormant for a while :

//public CinemachineVirtualCamera playerFacingCamera; //2/2/23 don't activate this Cam until we have a clean flow - if ever
//int originalCamPriority;                            //2/2/23 don't activate this Cam until we have a clean flow - if ever
//
//void OnCanvasNextPagePressedEvent()
//{
//  //  Debug.Log(this.name + " says next page pressed remove Listener -- AND If TextCloud is up ERASE it....");
//    textCloud.SetActive(false);   //5/21/23
//    m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
//}
//public void OnCanvasNextPagePressed()
//{
//    if (waitingForNextPagePress)
//    {
//        nextPagePressed = true;
//        m_CanvasNextPagePressedEvent.Invoke();
//        waitingForNextPagePress = false;
//    }
//}