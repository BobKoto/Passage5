using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using TMPro;


[System.Serializable]
public class MyIntEvent : UnityEvent<int, int, string>
{
}
public class TextCloudHandler : MonoBehaviour
{//Component of TextCloudHandleHolder
    //public Transform  playerTransform; commented 11/7/22 see EnableTheTextCloud() below
    public AudioManager audioManager;
    public GameObject textCloud;
    public GameObject cloudText;
    public MyIntEvent m_MyEvent;
    public int cloudTextDuration = 6;
    public CinemachineVirtualCamera playerFacingCamera; //2/2/23 don't activate this Cam until we have a clean flow - if ever
    int originalCamPriority;                            //2/2/23 don't activate this Cam until we have a clean flow - if ever

    public CanvasNextPagePressedEvent canvasNextPagePressedEvent;
    bool nextPagePressed;
    // Start is called before the first frame update
    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new MyIntEvent();

        m_MyEvent.AddListener(EnableTheTextCloud);

        if (canvasNextPagePressedEvent == null)
            canvasNextPagePressedEvent = new CanvasNextPagePressedEvent();
        canvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);

        originalCamPriority = playerFacingCamera.Priority;
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    public void EnableTheTextCloud(int x, int y, string _caption)
    {
        cloudText.GetComponent<TextMeshProUGUI>().text = _caption;
        textCloud.SetActive(true);
        audioManager.PlayAudio(audioManager.strom, 1f); //here maybe we can look for spaces in the string
        StartCoroutine(RemoveCloudAfterXSeconds(cloudTextDuration));
        //Debug.Log(this.name + "  EnableTheTextCloud called via event x = " + x + " y = " + y + " z = " + z);
    }
    public void EnableTheTextCloud(int x, int y, string _caption, Event nextPagePressed)
    {
        cloudText.GetComponent<TextMeshProUGUI>().text = _caption;
        textCloud.SetActive(true);
        audioManager.PlayAudio(audioManager.strom, 1f); //here maybe we can look for spaces in the string
        StartCoroutine(RemoveCloudAfterXSeconds(cloudTextDuration));
        //Debug.Log(this.name + "  EnableTheTextCloud called via event x = " + x + " y = " + y + " z = " + z);
    }
    IEnumerator RemoveCloudAfterXSeconds(int x)
    {
        yield return new WaitForSeconds (x);
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
    }
    IEnumerator RemoveCloudAfterNextPagePressedEvent(Event nextPressed)
    {
        yield return new WaitUntil(() => nextPagePressed);
        // playerFacingCamera.Priority = originalCamPriority; //2/2/23 don't activate/use this Cam until we have a clean flow - if ever
        textCloud.SetActive(false);
    }
    public void OnCanvasNextPagePressed()
    {
        canvasNextPagePressedEvent.Invoke();
    }

    void OnCanvasNextPagePressedEvent()
    {
        Debug.Log("next page pressed");
        nextPagePressed = true;
    }
    private void OnDisable()
    {
        m_MyEvent.RemoveListener(EnableTheTextCloud);  //I guess we should do this
        StopAllCoroutines();
    }
}
