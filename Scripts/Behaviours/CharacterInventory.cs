using DG.Tweening;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField] private Transform inventoryObjectStartTransform;
    [SerializeField] private Transform inventoryObjectTransform;
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    
    private GameObject inventoryItem;
    private Sequence currentSequence;

    public void TakeToInventory(GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("CharacterInventory: Attempted to take null item into inventory");
            return;
        }

        if (inventoryItem != null)
        {
            Debug.LogWarning("CharacterInventory: Inventory already contains an item. Cannot take another item.");
            return;
        }

        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        inventoryItem = item;

        item.transform.SetParent(transform);

        PositionAndAnimateItem(item);
    }

    private void PositionAndAnimateItem(GameObject item)
    {
        if (inventoryObjectStartTransform != null)
        {
            item.transform.position = inventoryObjectStartTransform.position;
            item.transform.rotation = inventoryObjectStartTransform.rotation;
        }

        if (inventoryObjectTransform != null)
        {
            currentSequence = DOTween.Sequence();
            currentSequence.Append(item.transform.DOMove(inventoryObjectTransform.position, tweenDuration).SetEase(tweenEase));
            currentSequence.Join(item.transform.DORotate(inventoryObjectTransform.eulerAngles, tweenDuration).SetEase(tweenEase));
            currentSequence.SetAutoKill(true);
        }
        else
        {
            Debug.LogWarning("CharacterInventory: inventoryObjectTransform is not assigned. Item will not animate to final position.");
        }
    }

    public GameObject GetFromInventory()
    {
        GameObject item = inventoryItem;
        inventoryItem = null;
        
        if (item != null)
        {
            item.transform.SetParent(null);
        }
        
        return item;
    }

    public bool HasItem()
    {
        return inventoryItem != null;
    }

    public GameObject PeekItem()
    {
        return inventoryItem;
    }

    private void OnDestroy()
    {
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }
    }
}
