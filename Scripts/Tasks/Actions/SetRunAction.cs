using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class SetRunAction : Action
{
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

        characterBehaviour.SetRun();
        return TaskStatus.Success;
    }
}
