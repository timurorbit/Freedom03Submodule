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

    /// <summary>
    /// Takes a GameObject into the inventory, making it a child of this inventory.
    /// First positions it at the start transform, then tweens it to the final transform.
    /// </summary>
    /// <param name="item">The GameObject to take into inventory</param>
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

        // Store the item reference
        inventoryItem = item;

        // Make the item a child of this inventory
        item.transform.SetParent(transform);

        // Set initial position to start transform
        if (inventoryObjectStartTransform != null)
        {
            item.transform.position = inventoryObjectStartTransform.position;
            item.transform.rotation = inventoryObjectStartTransform.rotation;
        }

        // Tween to final inventory position
        if (inventoryObjectTransform != null)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(item.transform.DOMove(inventoryObjectTransform.position, tweenDuration).SetEase(tweenEase));
            sequence.Join(item.transform.DORotate(inventoryObjectTransform.eulerAngles, tweenDuration).SetEase(tweenEase));
            sequence.SetAutoKill(true);
        }
    }

    /// <summary>
    /// Retrieves the item from the inventory and clears the inventory slot.
    /// </summary>
    /// <returns>The GameObject that was in the inventory, or null if empty</returns>
    public GameObject GetFromInventory()
    {
        GameObject item = inventoryItem;
        inventoryItem = null;
        return item;
    }

    /// <summary>
    /// Checks if the inventory currently holds an item.
    /// </summary>
    /// <returns>True if inventory has an item, false otherwise</returns>
    public bool HasItem()
    {
        return inventoryItem != null;
    }

    /// <summary>
    /// Gets the item currently in the inventory without removing it.
    /// </summary>
    /// <returns>The GameObject in the inventory, or null if empty</returns>
    public GameObject PeekItem()
    {
        return inventoryItem;
    }
}
