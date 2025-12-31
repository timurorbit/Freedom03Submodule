using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;
using UnityEngine.AI;

public abstract class GoToActionBase : Action
{
    [SerializeField] protected SharedVariable<float> ArriveDistance = .5f;

    protected NavMeshAgent Agent;

    public override void OnAwake()
    {
        base.OnAwake();
        Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        Vector3? destination = GetDestinationPosition();
        if (!destination.HasValue)
            return;
        Agent.isStopped = false;
        Agent.SetDestination(destination.Value);
    }

    protected bool HasArrived(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition, Agent.transform.position);
        return distance < ArriveDistance.Value;
    }

    public override TaskStatus OnUpdate()
    {
        Vector3? destination = GetDestinationPosition();
        if (!destination.HasValue)
            return TaskStatus.Failure;

        if (Agent.destination != destination.Value)
        {
            Agent.SetDestination(destination.Value);
        }

        if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return TaskStatus.Failure;
        }

        if (HasArrived(destination.Value))
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

    protected abstract Vector3? GetDestinationPosition();
}
