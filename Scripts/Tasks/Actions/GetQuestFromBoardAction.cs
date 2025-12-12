using DG.Tweening;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class GetQuestFromBoardAction : Action
{
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;

    private HeroBehaviour heroBehaviour;
    private Sequence currentSequence;

    public override void OnAwake()
    {
        base.OnAwake();
        heroBehaviour = gameObject.GetComponent<HeroBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (heroBehaviour == null)
            return TaskStatus.Failure;

        if (heroBehaviour.heroCard == null)
        {
            Debug.LogWarning("GetQuestFromBoardAction: HeroCard is null");
            return TaskStatus.Failure;
        }

        if (heroBehaviour.questPosition == null)
        {
            Debug.LogWarning("GetQuestFromBoardAction: questPosition is null");
            return TaskStatus.Failure;
        }

        Board board = GuildRepository.Instance.GetBoard();
        if (board == null)
        {
            Debug.LogWarning("GetQuestFromBoardAction: Board not found in GuildRepository");
            return TaskStatus.Failure;
        }

        Stats stats = heroBehaviour.heroCard.GetHero().GetStats();
        
        QuestResultBehaviour quest = null;
        try
        {
            quest = board.getQuestByStats(stats);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"GetQuestFromBoardAction: Error getting quest from board: {e.Message}");
            return TaskStatus.Failure;
        }
        
        if (quest == null)
        {
            Debug.LogWarning("GetQuestFromBoardAction: No quest retrieved from board");
            return TaskStatus.Failure;
        }

        heroBehaviour.currentQuestResultBehaviour = quest;

        quest.transform.SetParent(heroBehaviour.questPosition);

        TweenToQuestPosition(quest.transform);

        return TaskStatus.Success;
    }

    private void TweenToQuestPosition(Transform questTransform)
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

    public override void OnEnd()
    {
        base.OnEnd();
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }
    }
}
