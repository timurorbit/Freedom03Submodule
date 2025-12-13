using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainTable : Table
{
    [SerializeField] private Transform currentQuestResultPosition;
    [SerializeField] private Transform currentHeroCardPosition;
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    
    [SerializeField] public QuestResultBehaviour currentQuestResultBehaviour;
    [SerializeField] public HeroCardBehaviour currentHeroCardBehaviour;
    [SerializeField] public HeroBehaviour currentHeroBehaviour;

    [SerializeField] public List<InventorySlot> inventorySlots;

    public void Approve()
    {
        
    }
    
    public void Reject()
    {
        
    }

    public void UpdateCanvasView()
    {
        currentHeroCardBehaviour.UpdateCanvasView();
    }
    
    
    
    public void PlaceHeroCardAndQuestResult(HeroCardBehaviour heroCard, QuestResultBehaviour questResult)
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
            foreach (var slot in inventorySlots)
            {
                slot.GetHeroFromMainTable();
                slot.ApplyStatModifierIfNeeded(slot.GetItem());
            }
            currentQuestResultBehaviour = questResult;
            questResult.transform.SetParent(transform);
            questResult.SwitchState(QuestResultState.Opened);
            
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
        
        sequence.SetAutoKill(true);
    }

    public override void Interact()
    {
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        if (playerController != null)
        {
            playerController.SwitchState(GuildPlayerState.MainTable);
        }
        base.Interact();
    }

    public void PlaceHero(HeroBehaviour heroBehaviour)
    {
        currentHeroBehaviour = heroBehaviour;
    }

    public void Clear()
    {
        currentHeroBehaviour = null;
    }
}