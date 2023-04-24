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

    public int cloudTextDuration = 6;
    public CinemachineVirtualCamera playerFacingCamera; //2/2/23 don't activate this Cam until we have a clean flow - if ever
    int originalCamPriority;                            //2/2/23 don't activate this Cam until we have a clean flow - if ever

    //public CloudTextEvent m_CloudTextEvent;
    //public CloudTextEventWaitNextPage m_CloudTextEventWaitNextPage;
    public CanvasNextPagePressedEvent m_CanvasNextPagePressedEvent;

   // public CanvasNextPagePressedEvent canvasNextPagePressedEvent;
    bool nextPagePressed;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello from TextCloudHandler");
        //if (m_CloudTextEvent == null)
        //    m_CloudTextEvent = new CloudTextEvent();
        //m_CloudTextEvent.AddListener(EnableTheTextCloud);

        //if (m_CloudTextEventWaitNextPage == null)
        //    m_CloudTextEventWaitNextPage = new CloudTextEventWaitNextPage();
        //m_CloudTextEventWaitNextPage.AddListener(EnableTheTextCloudAndWaitForNextPage);

        if (m_CanvasNextPagePressedEvent == null)
            m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();
        m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);

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
        Debug.Log("call StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));");
        StartCoroutine(RemoveCloudAfterNextPagePressed(nextPagePressed));
        //Debug.Log(this.name + "  EnableTheTextCloud called via event x = " + x + " y = " + y + " z = " + z);
    }
    IEnumerator RemoveCloudAfterXSeconds(int x)
    {
        yield return new WaitForSeconds (x);
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
    }
    IEnumerator RemoveCloudAfterNextPagePressed(bool nextPressed)
    {
        yield return new WaitUntil(() => nextPagePressed);
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
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

    private void OnDisable()
    {
        //  m_CloudTextEvent.RemoveListener(EnableTheTextCloud);  //I guess we should do this
        m_CanvasNextPagePressedEvent.RemoveListener(OnCanvasNextPagePressedEvent);
        StopAllCoroutines();
    }
}
