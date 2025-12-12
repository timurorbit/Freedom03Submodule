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

        heroCard.transform.SetParent(heroBehaviour.transform);
        
        TweenHeroCardToPosition(heroCard, heroBehaviour.transform);

        heroBehaviour.heroCard = heroCard;
        mainTable.currentHeroCardBehaviour = null;

        return TaskStatus.Success;
    }

    private void TweenHeroCardToPosition(HeroCardBehaviour heroCard, Transform targetPosition)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(heroCard.transform.DOMove(targetPosition.position, tweenDuration).SetEase(tweenEase));
        sequence.Join(heroCard.transform.DORotate(targetPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
        sequence.SetAutoKill(true);
    }
}
