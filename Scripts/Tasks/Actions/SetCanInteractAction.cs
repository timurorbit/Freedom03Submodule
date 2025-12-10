using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class SetCanInteractAction : Action
{
    [Tooltip("The value to set for canInteract")]
    [SerializeField] private bool canInteractValue;

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

        characterBehaviour.SetCanInteract(canInteractValue);
        return TaskStatus.Success;
    }
}
