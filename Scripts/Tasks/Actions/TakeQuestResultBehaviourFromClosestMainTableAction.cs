using DG.Tweening;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class TakeQuestResultBehaviourFromClosestMainTableAction : Action
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
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestMainTableAction: HeroBehaviour is null");
            return TaskStatus.Failure;
        }

        MainTable mainTable = GuildRepository.Instance.GetClosestMainTable();
        
        if (mainTable == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestMainTableAction: No MainTable found");
            return TaskStatus.Failure;
        }

        QuestResultBehaviour questResult = mainTable.currentQuestResultBehaviour;
        
        if (questResult == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestMainTableAction: currentQuestResultBehaviour is null in MainTable");
            return TaskStatus.Failure;
        }

        Transform questPosition = heroBehaviour.questPosition;
        
        if (questPosition == null)
        {
            Debug.LogWarning("TakeQuestResultBehaviourFromClosestMainTableAction: questPosition is null in HeroBehaviour");
            return TaskStatus.Failure;
        }
        
        questResult.transform.SetParent(heroBehaviour.transform);
        if (heroBehaviour.Approved)
        {
            questResult.SwitchState(QuestResultState.Assigned);   
        }
        else
        {
            questResult.SwitchState(QuestResultState.Closed);
        }
        questResult.transform.SetParent(heroBehaviour.questPosition);
        TweenToPosition(questResult.transform);

        heroBehaviour.currentQuestResultBehaviour = questResult;
        mainTable.currentQuestResultBehaviour = null;

        return TaskStatus.Success;
    }

    private void TweenToPosition(Transform questTransform)
    {
        if (questTransform == null)
        {
            Debug.LogWarning("GetQuestFromBoardAction: Attempted to tween null transform");
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
