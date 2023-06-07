using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using TMPro;
public class TextCloudHandler : MonoBehaviour
{//Component of TextCloudHandleHolder
    //public Transform  playerTransform; commented 11/7/22 see EnableTheTextCloud() below
    public AudioManager audioManager;
    public GameObject textCloud;
    public GameObject cloudText;
    public GameObject menuButton;
    public GameObject lightButton;
    [Header("The UI stuff as GameObjects")]
    public GameObject nextPage;
    public GameObject nowPlay;

    public int cloudTextDuration = 6;
    public CinemachineVirtualCamera playerFacingCamera; //2/2/23 don't activate this Cam until we have a clean flow - if ever
    int originalCamPriority;                            //2/2/23 don't activate this Cam until we have a clean flow - if ever

    //public CloudTextEvent m_CloudTextEvent;
    //public CloudTextEventWaitNextPage m_CloudTextEventWaitNextPage;
    CanvasNextPagePressedEvent m_CanvasNextPagePressedEvent;
    public CloudTextExtinguishedEvent m_CloudTextExtinguishedEvent;
   // [Header("The Input System canvas Joystick etc.")]
    //public GameObject inputControls;
    // public CanvasNextPagePressedEvent canvasNextPagePressedEvent;
    bool nextPagePressed, waitingForNextPagePress;

    enum CloudBehavior :int
    {
        followTimeOut =  5,
        waitForNextPagePress = 6
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello from TextCloudHandler");
        if (m_CanvasNextPagePressedEvent == null)
            m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();
       // m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);  //Commented so we wait/addlistener only as needed

        if (m_CloudTextExtinguishedEvent == null)
            m_CloudTextExtinguishedEvent = new CloudTextExtinguishedEvent();
//        m_CloudTextExtinguishedEvent.AddListener(OnCloudTextEventExtinguished); //5/20/23 only invoke the event - let other scripts handle

        originalCamPriority = playerFacingCamera.Priority;
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (menuButton) menuButton.SetActive(false);
        if (lightButton) lightButton.SetActive(false);

    }
    public void EnableTheTextCloud(int cloudBehavior, int cloudTimeout, string _caption)
    {
        cloudText.GetComponent<TextMeshProUGUI>().text = _caption;
        textCloud.SetActive(true);
        audioManager.PlayAudio(audioManager.strom, 1f); //here maybe we can look for spaces in the string to adjust audio length
        //CloudBehavior caseBehavior = (CloudBehavior)cloudBehavior; //5/14/23 moved to switch statement
        switch ((CloudBehavior)cloudBehavior)
        {
            case CloudBehavior.followTimeOut:
                StartCoroutine(RemoveCloudAfterXSeconds(cloudTimeout));
                break;

            case CloudBehavior.waitForNextPagePress:
                //   Debug.Log(this.name + " call StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed)); ....NEW CASE");
                m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);  //moved here from START()
                waitingForNextPagePress = true;
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
                    Debug.Log(this.name + "  Active Cinemachine Camera: " + activeVirtualCamera.Name);
                }
                StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));
                break;

            default:
                Debug.Log(this.name + " EnableTheTextCloud recvd INVALID Behavior code ");
                break;
        }
    }

    IEnumerator RemoveCloudAfterXSeconds(int paramCloudTimeout)
    {
        yield return new WaitForSeconds (paramCloudTimeout);
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
        m_CloudTextExtinguishedEvent.Invoke();
    }
    IEnumerator RemoveCloudAfterNextPagePressed(bool nextPressed)
    {
        yield return new WaitForSeconds(1f);  //added 6/4/23
        if (nextPage) nextPage.SetActive(true);  //moved here 6/4/23
        yield return new WaitUntil(() => nextPagePressed);
        nextPagePressed = false;
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
        if (nextPage) nextPage.SetActive(false);
      //  Debug.Log(this.name + "  ****** now invoking m_CloudTextExtinguishedEvent ***** ");
        m_CloudTextExtinguishedEvent.Invoke();
    }
    void OnCanvasNextPagePressedEvent()
    {
      //  Debug.Log(this.name + " says next page pressed remove Listener -- AND If TextCloud is up ERASE it....");
        textCloud.SetActive(false);   //5/21/23
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
    }
    public void OnCanvasNextPagePressed()
    {
        if (waitingForNextPagePress)
        {
            nextPagePressed = true;
            m_CanvasNextPagePressedEvent.Invoke();
            waitingForNextPagePress = false;
        }
    }
    //public void OnCloudTextEventExtinguished()    //5/20/23 only invoke the event - let other scripts handle
    //{
    //    Debug.Log(this.name + " says the cloud went away... So enable the Play box");
    //    m_CloudTextExtinguishedEvent.RemoveListener(OnCloudTextEventExtinguished);
    //    if (nowPlay) nowPlay.SetActive(true);
    //    if (inputControls) inputControls.SetActive(true);
    //}
    private void OnDisable()
    {
        //  m_CloudTextEvent.RemoveListener(EnableTheTextCloud);  //I guess we should do this
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
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