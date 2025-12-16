using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

public class InventorySlot : Table
{
    [SerializeField]
    private GameObject item;

    [SerializeField] private Transform objectPosition;

    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    private Sequence currentSequence;
    private MainTable mainTable;
    
    
    [Header("Feedbacks")]
    [SerializeField]
    private MMF_Player addPotionFeedback;
    [SerializeField]
    private MMF_Player takePotionFeedback;

    public bool Add(GameObject itemToAdd)
    {
        if (item != null)
        {
            return false;
        }

        item = itemToAdd;
        item.transform.SetParent(objectPosition);
        TweenToPosition(item.transform);
        if (item.GetComponent<StatModifierBehaviour>())
        {
            addPotionFeedback?.PlayFeedbacks();
        }
        
        ApplyStatModifierIfNeeded(itemToAdd);
        
        return true;
    }

    public GameObject Take()
    {
        if (item == null)
        {
            return null;
        }

        GameObject takenItem = item;
        if (item.GetComponent<StatModifierBehaviour>())
        {
            takePotionFeedback?.PlayFeedbacks();
        }
        
        UnapplyStatModifierIfNeeded(takenItem);
        
        takenItem.transform.SetParent(null);
        item = null;
        return takenItem;
    }

    public GameObject GetItem()
    {
        return item;
    }

    public override void Interact()
    {
        var playerController = GuildPlayerController.Instance;
        if (playerController != null)
        {
            GameObject inventoryItem = playerController.GetFromInventory();
            if (inventoryItem != null)
            {
                if (!Add(inventoryItem))
                {
                    playerController.PutIntoInventory(inventoryItem);
                }
            }
            else if (playerController.CanTakeItem() && item != null)
            {
                GameObject takenItem = Take();
                playerController.PutIntoInventory(takenItem);
            }
        }
        base.Interact();
    }

    private void TweenToPosition(Transform itemTransform)
    {
        if (itemTransform == null)
        {
            return;
        }

        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        Vector3 targetPosition = Vector3.zero;
        Vector3 targetRotation = Vector3.zero;

        currentSequence = DOTween.Sequence();
        currentSequence.Append(itemTransform.DOLocalMove(targetPosition, tweenDuration).SetEase(tweenEase));
        currentSequence.Join(itemTransform.DOLocalRotate(targetRotation, tweenDuration).SetEase(tweenEase));
        currentSequence.SetAutoKill(true);
    }

    private void OnDestroy()
    {
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }
    }

    public void ApplyStatModifierIfNeeded(GameObject itemObject)
    {
        StatModifierBehaviour statModifierBehaviour = GetStatModifierBehaviour(itemObject);
        if (statModifierBehaviour == null)
        {
            return;
        }

        Hero hero = GetHeroFromMainTable();
        if (hero == null)
        {
            return;
        }

        
        statModifierBehaviour.statModifier.Apply(hero.GetStats());
        mainTable.UpdateCanvasView();
    }

    private void UnapplyStatModifierIfNeeded(GameObject itemObject)
    {
        StatModifierBehaviour statModifierBehaviour = GetStatModifierBehaviour(itemObject);
        if (statModifierBehaviour == null)
        {
            return;
        }

        Hero hero = GetHeroFromMainTable();
        if (hero == null)
        {
            return;
        }

        statModifierBehaviour.statModifier.Unapply(hero.GetStats());
        mainTable.UpdateCanvasView();
    }

    private StatModifierBehaviour GetStatModifierBehaviour(GameObject itemObject)
    {
        if (itemObject == null)
        {
            return null;
        }

        StatModifierBehaviour statModifierBehaviour = itemObject.GetComponent<StatModifierBehaviour>();
        if (statModifierBehaviour == null || statModifierBehaviour.statModifier == null)
        {
            return null;
        }

        return statModifierBehaviour;
    }

    public Hero GetHeroFromMainTable()
    {
        mainTable = GetComponentInParent<MainTable>();
        if (mainTable == null || mainTable.currentHeroCardBehaviour == null)
        {
            return null;
        }

        return mainTable.currentHeroCardBehaviour.GetHero();
    }
}