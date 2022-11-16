using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
//using UnityEngine.Animations;

public class IKReach : MonoBehaviour
{
    [SerializeField]
    Transform target = null;

    [SerializeField]
    AvatarIKGoal goal = AvatarIKGoal.RightHand;
    [SerializeField]
    [Range(0, 1)]
    float weight = 0.5f;
    Animator anim;
    AnimatorControllerLayer ACLayer;
    // Vector3 distanceToTarget;
    bool tooFarAway;
    public float distanceFromGoal = 9;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        int baseLayer = anim.GetLayerIndex("Base Layer");
        ACLayer = new AnimatorControllerLayer();

        ACLayer.iKPass = false;
      //  anim.SetIKPositionWeight
      //  var iBool = AnimatorControllerLayer.iKPass;
        
        //baseLayer.
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!tooFarAway)
        {
            //Debug.Log(" OnAnimatorIK called..."); //It works and moves ONLY the AvatarIKGoal (in this case right hand)
            anim.SetIKPosition(goal, target.position);

            anim.SetIKPositionWeight(goal, weight);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (distanceToTarget > distanceFromGoal)
        {
            tooFarAway = true;
        }
        else tooFarAway = false;
    }
}
