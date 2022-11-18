// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    Animator anim;

    public Transform goal;
    NavMeshAgent agent;
    enum Mode
    {
        AgentControlsPosition,
        AnimatorControlsPosition
    }
    [SerializeField] Mode mode = Mode.AnimatorControlsPosition;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 2f);
        agent = GetComponent<NavMeshAgent>();
      //  agent.
        agent.destination = goal.position;
        var dest = agent.pathEndPosition;
        Debug.Log("the dest is " + dest + "  The Goal is " + goal.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NavDest1"))
        {
            Debug.Log("Player hit NavDest1 ---set anim speed to 0 ");
            anim.SetFloat("Speed", 0f);
        }
    }
    //private void update()
    //{
    //    if (Vector3.Distance(transform.position, goal.position) < 1f)
    //    {
    //        anim.SetFloat("Speed", 0f);
    //    }
    //}
    //private void OnAnimatorMove()
    //{
    //    switch (mode)
    //    {
    //        case Mode.AgentControlsPosition:
    //            transform.position = agent.nextPosition;
    //            break;
    //        case Mode.AnimatorControlsPosition:
    //            Vector3 position = anim.rootPosition;
    //            position.y = agent.nextPosition.y;
    //            transform.position = position;
    //            break;

    //    }
    //}
}
