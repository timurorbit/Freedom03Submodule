using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class ReleaseSpotAction : Action
{
    [SerializeField] SharedVariable<GameObject> Spot;

    private CharacterBehaviour characterBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        characterBehaviour = gameObject.GetComponent<CharacterBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (characterBehaviour == null || Spot == null || Spot.Value == null)
            return TaskStatus.Failure;

        characterBehaviour.ReleaseSpot(Spot.Value);
        return TaskStatus.Success;
    }
}
