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
        
        TweenQuestResultToPosition(questResult, questPosition);

        heroBehaviour.currentQuestResultBehaviour = questResult;
        mainTable.currentQuestResultBehaviour = null;

        return TaskStatus.Success;
    }

    private void TweenQuestResultToPosition(QuestResultBehaviour questResult, Transform targetPosition)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(questResult.transform.DOMove(targetPosition.position, tweenDuration).SetEase(tweenEase));
        sequence.Join(questResult.transform.DORotate(targetPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
        sequence.SetAutoKill(true);
    }
}
