using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;
using UnityEngine.AI;

public class GoToObject : Action
{
    [SerializeField] SharedVariable<GameObject> Destination;

    [SerializeField] SharedVariable<float> ArriveDistance = .5f;


    NavMeshAgent Agent;

    public override void OnAwake()
    {
        base.OnAwake();

        Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        Debug.LogError(Destination.Name);
        if (Destination == null || Destination.Value == null)
            return;
        Agent.isStopped = false;
        Agent.SetDestination(Destination.Value.transform.position);
    }

    bool HasArrived()
    {
        float distance = Vector3.Distance(Destination.Value.transform.position, Agent.transform.position);
        return distance < ArriveDistance.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if (Destination == null || Destination.Value == null)
            return TaskStatus.Failure;

        if (Agent.destination != Destination.Value.transform.position)
        {
            Agent.SetDestination(Destination.Value.transform.position);
        }

        if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return TaskStatus.Failure;
        }

        if (HasArrived())
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (Agent.isOnNavMesh)
        {
            Agent.isStopped = true;
        }
    }
}