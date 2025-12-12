using DG.Tweening;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class GetQuestFromBoardAction : Action
{
    [Tooltip("The tweening duration for moving to quest position")]
    [SerializeField] private float tweenDuration = 0.5f;
    
    [Tooltip("The easing function for the tween")]
    [SerializeField] private Ease tweenEase = Ease.OutQuad;

    [SerializeField] private SharedVariable<Board> BoardVariable;

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

        // Get the Board from SharedVariable
        Board board = BoardVariable?.Value;
        if (board == null)
        {
            Debug.LogWarning("GetQuestFromBoardAction: Board not found in SharedVariable");
            return TaskStatus.Failure;
        }

        // Get stats from hero card
        Stats stats = heroBehaviour.heroCard.GetHero().GetStats();
        
        // Get quest from board by stats
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

        // Store the quest in HeroBehaviour
        heroBehaviour.currentQuestResultBehaviour = quest;

        // Set parent to questPosition
        quest.transform.SetParent(heroBehaviour.questPosition);

        // Tween to quest position (local position zero since it's parented)
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

        // Kill any active sequence before creating a new one
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        // Target is local zero position since we're parented to questPosition
        Vector3 targetPosition = Vector3.zero;
        Vector3 targetRotation = Vector3.zero;

        // Create tween sequence
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
