using DG.Tweening;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class TakeHeroCardFromClosestMainTableAction : Action
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
            Debug.LogWarning("TakeHeroCardFromClosestMainTableAction: HeroBehaviour is null");
            return TaskStatus.Failure;
        }

        MainTable mainTable = GuildRepository.Instance.GetClosestMainTable();
        
        if (mainTable == null)
        {
            Debug.LogWarning("TakeHeroCardFromClosestMainTableAction: No MainTable found");
            return TaskStatus.Failure;
        }

        HeroCardBehaviour heroCard = mainTable.currentHeroCardBehaviour;
        
        if (heroCard == null)
        {
            Debug.LogWarning("TakeHeroCardFromClosestMainTableAction: currentHeroCardBehaviour is null in MainTable");
            return TaskStatus.Failure;
        }

        heroCard.transform.SetParent(heroBehaviour.heroCardPosition);
        heroCard.SwitchState(false);
        TweenToPosition(heroCard.transform);

        heroBehaviour.heroCard = heroCard;
        mainTable.currentHeroCardBehaviour = null;

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
