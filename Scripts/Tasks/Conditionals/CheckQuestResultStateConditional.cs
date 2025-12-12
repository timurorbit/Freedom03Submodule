using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using UnityEngine;

public class CheckQuestResultStateConditional : Conditional
{
    [Tooltip("The quest result state to check for")]
    [SerializeField] private QuestResultState targetState;

    private HeroBehaviour heroBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        heroBehaviour = gameObject.GetComponent<HeroBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (heroBehaviour == null)
        {
            Debug.LogWarning("CheckQuestResultStateConditional: HeroBehaviour is null");
            return TaskStatus.Failure;
        }

        if (heroBehaviour.currentQuestResultBehaviour == null)
        {
            Debug.LogWarning("CheckQuestResultStateConditional: currentQuestResultBehaviour is null");
            return TaskStatus.Failure;
        }

        QuestResult questResult = heroBehaviour.currentQuestResultBehaviour.getQuestResult();
        
        if (questResult == null)
        {
            Debug.LogWarning("CheckQuestResultStateConditional: QuestResult is null");
            return TaskStatus.Failure;
        }

        return questResult.state == targetState ? TaskStatus.Success : TaskStatus.Failure;
    }
}
