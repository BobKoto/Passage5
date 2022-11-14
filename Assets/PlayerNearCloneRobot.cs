using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearCloneRobot : MonoBehaviour
{
    public float stopWavingAfterXSeconds = 10f;
    public MyIntEvent m_MyEvent;
    const string noMoney = "#No money.";
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("WaveArmsTF", true);
            TellTextCloud(noMoney);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // anim.SetBool("WaveArmsTF", false);
            StartCoroutine(StopWavingAfterXSeconds(stopWavingAfterXSeconds));
        }

    }
    IEnumerator StopWavingAfterXSeconds(float x)
    {
        yield return new WaitForSeconds (x);
        anim.SetBool("WaveArmsTF", false);
    }
    public void TellTextCloud(string caption)
    {
        m_MyEvent.Invoke(5, 4, caption);
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
