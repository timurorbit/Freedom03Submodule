using DG.Tweening;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class TakeQuestActualStatsBehaviourFromClosestResultTableAction : Action
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
            Debug.LogWarning("TakeQuestActualStatsBehaviourFromClosestResultTableAction: HeroBehaviour is null");
            return TaskStatus.Failure;
        }

        QuestResultTable questResultTable = GuildRepository.Instance.GetClosestTable<QuestResultTable>();
        
        if (questResultTable == null)
        {
            Debug.LogWarning("TakeQuestActualStatsBehaviourFromClosestResultTableAction: No QuestResultTable found");
            return TaskStatus.Failure;
        }

        ActualStatsBehaviour actualStats = questResultTable.currentActualStatsBehaviour;
        
        if (actualStats == null)
        {
            Debug.LogWarning("TakeQuestActualStatsBehaviourFromClosestResultTableAction: currentActualStatsBehaviour is null in QuestResultTable");
            return TaskStatus.Failure;
        }

        Transform questPosition = heroBehaviour.questPosition;
        
        if (questPosition == null)
        {
            Debug.LogWarning("TakeQuestActualStatsBehaviourFromClosestResultTableAction: questPosition is null in HeroBehaviour");
            return TaskStatus.Failure;
        }
        
        actualStats.transform.SetParent(heroBehaviour.questPosition);
        TweenToPosition(actualStats.transform, () => {
            actualStats.gameObject.SetActive(false);
        });

        questResultTable.currentActualStatsBehaviour = null;

        return TaskStatus.Success;
    }

    private void TweenToPosition(Transform questTransform, System.Action onComplete)
    {
        if (questTransform == null)
        {
            Debug.LogWarning("TakeQuestActualStatsBehaviourFromClosestResultTableAction: Attempted to tween null transform");
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
        currentSequence.OnComplete(() => onComplete?.Invoke());
        currentSequence.SetAutoKill(true);
    }
}
