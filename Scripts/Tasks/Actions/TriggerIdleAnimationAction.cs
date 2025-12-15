using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class TriggerIdleAnimationAction : Action
{
    [SerializeField] private int triggerIndex = 1;
    
    private CharacterBehaviour characterBehaviour;
    private Animator animator;

    public override void OnAwake()
    {
        base.OnAwake();
        characterBehaviour = gameObject.GetComponent<CharacterBehaviour>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on character");
            return TaskStatus.Failure;
        }

        string triggerName = triggerIndex == 1 ? Constants.ANIMATOR_IDLE_TRIGGER_1 : Constants.ANIMATOR_IDLE_TRIGGER_2;
        animator.SetTrigger(triggerName);
        
        return TaskStatus.Success;
    }
}
