using DG.Tweening;
using UnityEngine;

public class QuestResultTable : Table
{
    [Header("Positions")] [SerializeField] private Transform currentQuestResultPosition;
    [SerializeField] private Transform currentHeroCardPosition;
    [SerializeField] private Transform currentActualQuestStatsBehaviourTransform;


    [Header("Items")] [SerializeField] public QuestResultBehaviour currentQuestResultBehaviour;
    [SerializeField] public HeroCardBehaviour currentHeroCardBehaviour;
    [SerializeField] public HeroBehaviour currentHeroBehaviour;
    [SerializeField] public ActualStatsBehaviour currentActualStatsBehaviour;

    [Header("Tween Settings")] [SerializeField]
    private float tweenDuration = 0.5f;

    [SerializeField] private Ease tweenEase = Ease.OutQuad;

    public override void Interact()
    {
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        if (playerController != null)
        {
            playerController.SwitchState(GuildPlayerState.QuestResultTable);
        }

        base.Interact();
    }

    public void PlaceHeroCardAndQuestResultAndQuestStats(HeroCardBehaviour heroCard, QuestResultBehaviour questResult, ActualStatsBehaviour actualStats)
    {
        Sequence sequence = DOTween.Sequence();
        bool heroCardAnimated = false;
        
        if (heroCard != null && currentHeroCardPosition != null)
        {
            currentHeroCardBehaviour = heroCard;
            heroCard.transform.SetParent(transform);
            heroCard.SwitchState(true);
            
            sequence.Append(heroCard.transform.DOMove(currentHeroCardPosition.position, tweenDuration).SetEase(tweenEase));
            sequence.Join(heroCard.transform.DORotate(currentHeroCardPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
            heroCardAnimated = true;
        }
        if (questResult != null && currentQuestResultPosition != null)
        {
            currentQuestResultBehaviour = questResult;
            questResult.transform.SetParent(transform);
            questResult.SwitchState(QuestResultState.Completed);
            
            if (heroCardAnimated)
            {
                sequence.Join(questResult.transform.DOMove(currentQuestResultPosition.position, tweenDuration).SetEase(tweenEase));
                sequence.Join(questResult.transform.DORotate(currentQuestResultPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
            }
            else
            {
                sequence.Append(questResult.transform.DOMove(currentQuestResultPosition.position, tweenDuration).SetEase(tweenEase));
                sequence.Join(questResult.transform.DORotate(currentQuestResultPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
            }
        }

        if (actualStats != null)
        {
            currentActualStatsBehaviour = actualStats;
            actualStats.transform.SetParent(transform);
            sequence.Append(actualStats.transform.DOMove(currentActualQuestStatsBehaviourTransform.position, tweenDuration).SetEase(tweenEase));
            sequence.Join(actualStats.transform.DORotate(currentActualQuestStatsBehaviourTransform.eulerAngles, tweenDuration).SetEase(tweenEase));
        }
        
        sequence.SetAutoKill(true);
    }

    public void Clear()
    {
        currentHeroBehaviour = null;
    }
}