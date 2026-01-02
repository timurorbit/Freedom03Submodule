using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class SetCharacterStateAction : Action
{
    private CharacterBehaviour characterBehaviour;
    
    [SerializeField]
    protected SharedVariable<CharacterState> characterState;

    public override void OnAwake()
    {
        base.OnAwake();
        characterBehaviour = gameObject.GetComponent<CharacterBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (characterBehaviour == null)
            return TaskStatus.Failure;

        characterBehaviour.SetCharacterState(characterState.Value);
        return TaskStatus.Success;
    }
}