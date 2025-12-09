using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class FindClosestEntranceAction : Action
{
    [SerializeField] SharedVariable<GameObject> Result;

    private CharacterBehaviour characterBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        characterBehaviour = gameObject.GetComponent<CharacterBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (characterBehaviour == null)
            return TaskStatus.Failure;

        var entrance = characterBehaviour.FindClosestEntrance();
        
        if (entrance == null)
            return TaskStatus.Failure;

        if (Result != null)
            Result.Value = entrance;

        return TaskStatus.Success;
    }
}
