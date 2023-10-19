using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningFlash : MonoBehaviour
{//Component of Line(something)

    public float interval = 5;
    public float timeOn = 2;
    LineRenderer myLineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.enabled = false;
        Debug.Log("hello from " + this.name + " linerenderer.enable set false ");
        StartCoroutine (FlashTheLightning());
    }

    IEnumerator FlashTheLightning()
    {
        while (true)
        {
            myLineRenderer.enabled = false;
            yield return new WaitForSeconds(interval);
            myLineRenderer.enabled = true;
            yield return new WaitForSeconds(timeOn);
        }
    }
    void OnDisabble()
    {
        StopAllCoroutines();
    }
}
