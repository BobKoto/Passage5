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
    //public Transform  playerTransform; commented 11/7/22 see EnableTheTextCloud() below
    public GameObject textCloud;
    public GameObject cloudText;
    public MyIntEvent m_MyEvent;
    public int cloudTextDuration = 6;
    // Start is called before the first frame update
    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new MyIntEvent();

        m_MyEvent.AddListener(EnableTheTextCloud);
    }

    public void EnableTheTextCloud(int x, int y, string _caption)
    {
        // Next 3 lines commented after moving textcloud to a UI Canvas  - 11/7/22 - delete after some time (of testing)
        //Vector3 newCloudPosition = new Vector3 (playerTransform.position.x + 4f, playerTransform.position.y + 5f, playerTransform.position.z - 2f);
        //Debug.Log("textCloud pos = " + textCloud.transform.position + " Player pos = " + playerTransform.position);
        //textCloud.transform.position = newCloudPosition;
        cloudText.GetComponent<TextMeshProUGUI>().text = _caption;
        Debug.Log(this.name + "  Set caption string to " + _caption);
        textCloud.SetActive(true);
        StartCoroutine(RemoveCloudAfterXSeconds(cloudTextDuration));
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
