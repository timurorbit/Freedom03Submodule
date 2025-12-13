using DG.Tweening;
using UnityEngine;

public class InventorySlot : Table
{
    [SerializeField]
    private GameObject item;

    [SerializeField] private Transform objectPosition;

    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    private Sequence currentSequence;

    public bool Add(GameObject itemToAdd)
    {
        if (item != null)
        {
            return false;
        }

        item = itemToAdd;
        item.transform.SetParent(objectPosition);
        TweenToPosition(item.transform);
        return true;
    }

    public GameObject Take()
    {
        if (item == null)
        {
            return null;
        }

        GameObject takenItem = item;
        takenItem.transform.SetParent(null);
        item = null;
        return takenItem;
    }

    public override void Interact()
    {
        var playerController = GuildPlayerController.Instance;
        if (playerController != null)
        {
            GameObject inventoryItem = playerController.GetFromInventory();
            if (inventoryItem != null)
            {
                Add(inventoryItem);
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
}