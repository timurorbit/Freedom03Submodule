using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using UnityEngine;

public class CheckCharacterStateConditional : Conditional
{
    [Tooltip("The character state to check for")]
    [SerializeField] private CharacterState targetState;

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

        return characterBehaviour.characterState == targetState ? TaskStatus.Success : TaskStatus.Failure;
    }
}
