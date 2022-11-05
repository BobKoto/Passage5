using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


    [System.Serializable]
    public class MyIntEvent : UnityEvent<int, int, string>
    {
    }
public class TextCloudHandler : MonoBehaviour
{
    public Transform  playerTransform;
    public GameObject textCloud;
    public TextMeshPro cloudTextString;
    public MyIntEvent m_MyEvent;
    // Start is called before the first frame update
    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new MyIntEvent();

        m_MyEvent.AddListener(EnableTheTextCloud);
    }

    public void EnableTheTextCloud(int x, int y, string _caption)
    {
        Debug.Log("textCloud pos = " + textCloud.transform.position + " Player pos = " + playerTransform.position);
        Vector3 newCloudPosition = new Vector3 (playerTransform.position.x + 4f, playerTransform.position.y + 5f, playerTransform.position.z - 6f);
        textCloud.transform.position = newCloudPosition;
        cloudTextString.text = _caption;
        Debug.Log(this.name + "  Set caption string to " + _caption);
        textCloud.SetActive(true);
        StartCoroutine(RemoveCloudAfterXSeconds(6));
        //Debug.Log(this.name + "  EnableTheTextCloud called via event x = " + x + " y = " + y + " z = " + z);
    }
    IEnumerator RemoveCloudAfterXSeconds(int x)
    {
        yield return new WaitForSeconds (x);
        textCloud.SetActive(false);
    }
    private void OnDisable()
    {
        m_MyEvent.RemoveListener(EnableTheTextCloud);  //I guess we should do this
        StopAllCoroutines();
    }
}
