using DG.Tweening;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class TakeQuestResultBehaviourFromClosestResultTableAction : Action
{
    private HeroBehaviour heroBehaviour;
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    private Sequence currentSequence;

    public override void OnAwake()
    {
        base.OnAwake();
        heroBehaviour = gameObject.GetComponent<HeroBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (heroBehaviour == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestResultTableAction: HeroBehaviour is null");
            return TaskStatus.Failure;
        }

        QuestResultTable questResultTable = GuildRepository.Instance.GetClosestTable<QuestResultTable>();
        
        if (questResultTable == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestResultTableAction: No QuestResultTable found");
            return TaskStatus.Failure;
        }

        QuestResultBehaviour questResult = questResultTable.currentQuestResultBehaviour;
        
        if (questResult == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestResultTableAction: currentQuestResultBehaviour is null in QuestResultTable");
            return TaskStatus.Failure;
        }

        Transform questPosition = heroBehaviour.questPosition;
        
        if (questPosition == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestResultTableAction: questPosition is null in HeroBehaviour");
            return TaskStatus.Failure;
        }
        
        questResult.transform.SetParent(heroBehaviour.transform);
        if (heroBehaviour.Approved)
        {
            questResult.SwitchState(QuestResultState.Assigned);   
        }
        else
        {
            questResult.SwitchState(QuestResultState.Declined);
        }
        questResult.transform.SetParent(heroBehaviour.questPosition);
        TweenToPosition(questResult.transform);

        heroBehaviour.currentQuestResultBehaviour = questResult;
        questResultTable.currentQuestResultBehaviour = null;

        return TaskStatus.Success;
    }

    private void TweenToPosition(Transform questTransform)
    {
        if (questTransform == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestResultTableAction: Attempted to tween null transform");
            return;
        }

        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        Vector3 targetPosition = Vector3.zero;
        Vector3 targetRotation = Vector3.zero;

        currentSequence = DOTween.Sequence();
        currentSequence.Append(questTransform.DOLocalMove(targetPosition, tweenDuration).SetEase(tweenEase));
        currentSequence.Join(questTransform.DOLocalRotate(targetRotation, tweenDuration).SetEase(tweenEase));
        currentSequence.SetAutoKill(true);
    }
}
