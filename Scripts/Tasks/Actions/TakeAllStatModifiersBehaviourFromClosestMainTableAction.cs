using DG.Tweening;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class TakeAllStatModifiersBehaviourFromClosestMainTableAction : Action
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
            Debug.LogWarning("TakeAllStatModifiersBehaviourFromClosestMainTableAction: HeroBehaviour is null");
            return TaskStatus.Failure;
        }

        MainTable mainTable = GuildRepository.Instance.GetClosestMainTable();
        
        if (mainTable == null)
        {
            Debug.LogWarning("TakeAllStatModifiersBehaviourFromClosestMainTableAction: No MainTable found");
            return TaskStatus.Failure;
        }

        if (mainTable.inventorySlots == null || mainTable.inventorySlots.Count == 0)
        {
            Debug.LogWarning("TakeAllStatModifiersBehaviourFromClosestMainTableAction: No inventory slots found in MainTable");
            return TaskStatus.Failure;
        }

        Transform statModifiersParent = heroBehaviour.statModifiersParent;
        
        if (statModifiersParent == null)
        {
            Debug.LogWarning("TakeAllStatModifiersBehaviourFromClosestMainTableAction: statModifiersParent is null in HeroBehaviour");
            return TaskStatus.Failure;
        }

        foreach (InventorySlot slot in mainTable.inventorySlots)
        {
            if (slot == null)
            {
                continue;
            }

            GameObject takenItem = slot.Take();
            
            if (takenItem == null)
            {
                continue;
            }

            StatModifierBehaviour statModifier = takenItem.GetComponent<StatModifierBehaviour>();
            
            if (statModifier == null)
            {
                Debug.LogWarning("TakeAllStatModifiersBehaviourFromClosestMainTableAction: Taken item does not have StatModifierBehaviour component");
                continue;
            }

            statModifier.transform.SetParent(statModifiersParent);
            TweenToPosition(statModifier.transform);
            heroBehaviour.statModifiers.Add(statModifier);
        }

        return TaskStatus.Success;
    }

    private void TweenToPosition(Transform statModifierTransform)
    {
        if (statModifierTransform == null)
        {
            Debug.LogWarning("TakeAllStatModifiersBehaviourFromClosestMainTableAction: Attempted to tween null transform");
            return;
        }

        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        Vector3 targetPosition = Vector3.zero;
        Vector3 targetRotation = Vector3.zero;

        currentSequence = DOTween.Sequence();
        currentSequence.Append(statModifierTransform.DOLocalMove(targetPosition, tweenDuration).SetEase(tweenEase));
        currentSequence.Join(statModifierTransform.DOLocalRotate(targetRotation, tweenDuration).SetEase(tweenEase));
        currentSequence.SetAutoKill(true);
    }
}
