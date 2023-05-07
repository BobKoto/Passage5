using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearCloneRobot : MonoBehaviour
{   //Component of PlayerClone and PlayerClone(x) // x is 1 for now 
    public float stopWavingAfterXSeconds = 10f;
    public float stopWalkingAfterXSeconds = 10f;
    public CloudTextEvent m_CloudTextEvent;
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
            anim.SetFloat("Speed", 2f);
            TellTextCloud(noMoney);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // anim.SetBool("WaveArmsTF", false);
            StartCoroutine(StopWavingAfterXSeconds(stopWavingAfterXSeconds));
            StartCoroutine(StopWalkingAfterXSeconds(stopWalkingAfterXSeconds));
        }
    }
    private void OnFootstep(AnimationEvent animationEvent)
    {
       
       // Debug.Log("PlayerClone recvd a Footstep Event " + transform.position + "  event clip info  " +  animationEvent.animatorClipInfo.clip);
        var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z -  .1f);
        transform.position = newPosition;
    }
    IEnumerator StopWavingAfterXSeconds(float x)
    {
        yield return new WaitForSeconds (x);
        anim.SetBool("WaveArmsTF", false);
    }
    IEnumerator StopWalkingAfterXSeconds(float x)
    {
        yield return new WaitForSeconds(x);
        anim.SetFloat("Speed", 0f);
    }
    public void TellTextCloud(string caption)
    {
        m_CloudTextEvent.Invoke(5, 4, caption);
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
