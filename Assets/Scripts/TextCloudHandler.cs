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
    public CanvasNextPagePressedEvent m_CanvasNextPagePressedEvent;
    public CloudTextEventExtinguished m_CloudTextEventExtinguished;
    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;

    // public CanvasNextPagePressedEvent canvasNextPagePressedEvent;
    bool nextPagePressed;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello from TextCloudHandler");

        if (m_CanvasNextPagePressedEvent == null)
            m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();
       // m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);  //Commented so we wait/addlistener only as needed

        if (m_CloudTextEventExtinguished == null)
            m_CloudTextEventExtinguished = new CloudTextEventExtinguished();
        m_CloudTextEventExtinguished.AddListener(OnCloudTextEventExtinguished);

        originalCamPriority = playerFacingCamera.Priority;
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (menuButton) menuButton.SetActive(false);
        if (lightButton) lightButton.SetActive(false);
    }

    public void EnableTheTextCloud(int x, int y, string _caption)
    {
        cloudText.GetComponent<TextMeshProUGUI>().text = _caption;
        textCloud.SetActive(true);
        audioManager.PlayAudio(audioManager.strom, 1f); //here maybe we can look for spaces in the string
        Debug.Log(this.name + " call StartCoroutine(RemoveCloudAfterXSeconds(cloudTextDuration));");
        StartCoroutine(RemoveCloudAfterXSeconds(cloudTextDuration));
        //Debug.Log(this.name + "  EnableTheTextCloud called via event x = " + x + " y = " + y + " z = " + z);
    }
    public void EnableTheTextCloudAndWaitForNextPage(int x, int y, string _caption, bool waitForNextPagePressed)
    {
        cloudText.GetComponent<TextMeshProUGUI>().text = _caption;
        textCloud.SetActive(true);
        audioManager.PlayAudio(audioManager.strom, 1f); //here maybe we can look for spaces in the string
        Debug.Log(this.name + " call StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));");
        m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);  //moved here from START()
        StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));
        //Debug.Log(this.name + "  EnableTheTextCloud called via event x = " + x + " y = " + y + " z = " + z);
    }
    IEnumerator RemoveCloudAfterXSeconds(int x)
    {
        yield return new WaitForSeconds (x);
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
        m_CloudTextEventExtinguished.Invoke();
    }
    IEnumerator RemoveCloudAfterNextPagePressed(bool nextPressed)
    {
        yield return new WaitUntil(() => nextPagePressed);
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
        m_CloudTextEventExtinguished.Invoke();

    }
    void OnCanvasNextPagePressedEvent()
    {

        Debug.Log(this.name + " says next page pressed remove Listener");
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
    }
    public void OnCanvasNextPagePressed()
    {
        m_CanvasNextPagePressedEvent.Invoke();

    }
    void OnCloudTextEventExtinguished()
    {
        Debug.Log(this.name + " says the cloud went away... So enable the Play box");
        m_CloudTextEventExtinguished.RemoveListener(OnCloudTextEventExtinguished);
        if (nowPlay) nowPlay.SetActive(true);
        if (inputControls) inputControls.SetActive(true);
    }
    private void OnDisable()
    {
        //  m_CloudTextEvent.RemoveListener(EnableTheTextCloud);  //I guess we should do this
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
        StopAllCoroutines();
    }
}
