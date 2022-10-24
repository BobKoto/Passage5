using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimButtons : MonoBehaviour     //Find this on PlayerArmature Game Object 
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }
    public void OnPressPressed()
    {
        anim.SetTrigger("Punch");
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
