using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWeight : MonoBehaviour
{
    public Animation robotArmsUp;
    float animWeight;
    // Start is called before the first frame update
    void Start()
    {
        animWeight = robotArmsUp["RobotArmsUp"].weight; // = 0.5f; anim["Walk"].weight = 0.5f;
        Debug.Log("AnimationWeight reports weight of " + animWeight);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
