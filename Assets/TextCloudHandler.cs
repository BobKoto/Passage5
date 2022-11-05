using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    [System.Serializable]
    public class MyIntEvent : UnityEvent<int, int, int>
    {
    }
public class TextCloudHandler : MonoBehaviour
{
    public Transform  playerTransform;
    public GameObject textCloud;
    public MyIntEvent m_MyEvent;
    // Start is called before the first frame update
    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new MyIntEvent();

        m_MyEvent.AddListener(EnableTheTextCloud);
    }

    public void EnableTheTextCloud(int x, int y, int z)
    {
        Debug.Log("textCloud pos = " + textCloud.transform.position + " Player pos = " + playerTransform.position);
        Vector3 newCloudPosition = new Vector3 (playerTransform.position.x + 4f, playerTransform.position.y + 6f, playerTransform.position.z - 6f);
        textCloud.transform.position = newCloudPosition;

        textCloud.SetActive(true);
        StartCoroutine(RemoveCloudAfterXSeconds(3));
        //Debug.Log(this.name + "  EnableTheTextCloud called via event x = " + x + " y = " + y + " z = " + z);
    }
    IEnumerator RemoveCloudAfterXSeconds(int x)
    {
        yield return new WaitForSeconds (x);
       // textCloud.SetActive(false);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
