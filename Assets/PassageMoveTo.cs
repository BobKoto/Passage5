// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
<<<<<<< HEAD
namespace PassageMoveToNameSpace
{


    public class PassageMoveTo : MonoBehaviour
    {
        Animator anim;

        //  public Transform goal;
        public Transform[] agentGoals;
        public bool agentUpdatePosition = true; //the Default I think
        NavMeshAgent agent;
        enum Mode
        {
            AgentControlsPosition,
            AnimatorControlsPosition
        }
        [SerializeField] Mode mode = Mode.AnimatorControlsPosition;
        [SerializeField] string forwardSpeedParmeterName = "Speed";

        Vector2 smoothDeltaPosition = Vector2.zero;
        bool robotReachedDestination;
        int agentGoalIndex = 0;
        void Start()
        {
            anim = GetComponent<Animator>();
            anim.SetFloat(forwardSpeedParmeterName, 2f);
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = agentUpdatePosition;  //book recipe says set to false (so default true?)

            //    agentGoals = new Transform[3];
            agent.destination = agentGoals[0].position;
            var dest = agent.pathEndPosition;
            Debug.Log("the dest is " + dest + "  The Goal is " + agentGoals[agentGoalIndex].position);
        }
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.CompareTag("NavDest1"))
        //    {
        //        robotReachedDestination = true;
        //        Debug.Log("Robot hit NavDest1 ---set anim speed to 0 ");
        //        anim.SetFloat(forwardSpeedParmeterName, 0f);
        //    }
        //}
        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.CompareTag("NavDest1"))
        //    {
        //        robotReachedDestination = false;
        //        Debug.Log("Robot hit NavDest1 ---set anim speed to 0 ");
        //        anim.SetFloat(forwardSpeedParmeterName, 0f);
        //    }
        //}
        private void Update()
        {
            Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

            float xMovement = Vector3.Dot(transform.right, worldDeltaPosition);
            float zMovement = Vector3.Dot(transform.forward, worldDeltaPosition);

            Vector2 localDeltaPosition = new Vector2(xMovement, zMovement);
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, localDeltaPosition, smooth);

            var velocity = smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
            ////////////////////// here test destination change /////////////
            //if (!robotReachedDestination)
            if (agent.remainingDistance <= agent.radius)
            {
                SetNextDestination();
            }

            //////////////////////end test /////////////////////////////////           
            anim.SetFloat(forwardSpeedParmeterName, velocity.y);

            if (mode == Mode.AnimatorControlsPosition)
            {
                if (worldDeltaPosition.magnitude > agent.radius)
                {
                    transform.position = Vector3.Lerp(transform.position, agent.nextPosition, Time.deltaTime / 0.15f);
                }
            }
        }
        private void OnAnimatorMove()
        {
            switch (mode)
            {
                case Mode.AgentControlsPosition:
                    transform.position = agent.nextPosition;
                    break;
                case Mode.AnimatorControlsPosition:
                    Vector3 position = anim.rootPosition;
                    position.y = agent.nextPosition.y;
                    transform.position = position;
                    break;

            }
        }
        void SetNextDestination()
        {
            // Debug.Log("SetNext - agentGoalIndex is " + agentGoalIndex);

            agentGoalIndex++;
            if (agentGoalIndex >= agentGoals.Length) agentGoalIndex = 0;
            agent.destination = agentGoals[agentGoalIndex].position;
        }
    }
=======

public class PassageMoveTo : MonoBehaviour
{
    Animator anim;

    //  public Transform goal;
    public Transform[] agentGoals;
    public bool agentUpdatePosition = true; //the Default I think
    NavMeshAgent agent;
    enum Mode
    {
        AgentControlsPosition,
        AnimatorControlsPosition
    }
    [SerializeField] Mode mode = Mode.AnimatorControlsPosition;
    [SerializeField] string forwardSpeedParmeterName = "Speed";

    Vector2 smoothDeltaPosition = Vector2.zero;
    bool robotReachedDestination;
    int agentGoalIndex = 0;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat(forwardSpeedParmeterName, 2f);
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = agentUpdatePosition;  //book recipe says set to false (so default true?)

        //    agentGoals = new Transform[3];
        agent.destination = agentGoals[0].position;
        var dest = agent.pathEndPosition;
        Debug.Log("the dest is " + dest + "  The Goal is " + agentGoals[agentGoalIndex].position);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("NavDest1"))
    //    {
    //        robotReachedDestination = true;
    //        Debug.Log("Robot hit NavDest1 ---set anim speed to 0 ");
    //        anim.SetFloat(forwardSpeedParmeterName, 0f);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("NavDest1"))
    //    {
    //        robotReachedDestination = false;
    //        Debug.Log("Robot hit NavDest1 ---set anim speed to 0 ");
    //        anim.SetFloat(forwardSpeedParmeterName, 0f);
    //    }
    //}
    private void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        float xMovement = Vector3.Dot(transform.right, worldDeltaPosition);
        float zMovement = Vector3.Dot(transform.forward, worldDeltaPosition);

        Vector2 localDeltaPosition = new Vector2(xMovement, zMovement);
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, localDeltaPosition, smooth);

        var velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        ////////////////////// here test destination change /////////////
        //if (!robotReachedDestination)
        if (agent.remainingDistance <= agent.radius)
        {
            SetNextDestination();
        }

        //////////////////////end test /////////////////////////////////           
        anim.SetFloat(forwardSpeedParmeterName, velocity.y);

        if (mode == Mode.AnimatorControlsPosition)
        {
            if (worldDeltaPosition.magnitude > agent.radius)
            {
                transform.position = Vector3.Lerp(transform.position, agent.nextPosition, Time.deltaTime / 0.15f);
            }
        }
    }
    private void OnAnimatorMove()
    {
        switch (mode)
        {
            case Mode.AgentControlsPosition:
                transform.position = agent.nextPosition;
                break;
            case Mode.AnimatorControlsPosition:
                Vector3 position = anim.rootPosition;
                position.y = agent.nextPosition.y;
                transform.position = position;
                break;

        }
    }
    void SetNextDestination()
    {
        // Debug.Log("SetNext - agentGoalIndex is " + agentGoalIndex);

        agentGoalIndex++;
        if (agentGoalIndex >= agentGoals.Length) agentGoalIndex = 0;
        agent.destination = agentGoals[agentGoalIndex].position;
    }
>>>>>>> Work-on-scene-with-3rd-person-input
}
