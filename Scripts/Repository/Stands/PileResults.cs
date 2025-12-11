using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Behaviours;

public class PileResults : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Outline outline;

    [SerializeField]
    private readonly Stack<QuestResultBehaviour> results = new();

    private bool _canInteract = false;
    public bool canInteract => _canInteract;

    private void Awake()
    {
        if (outline == null)
        {
            outline = GetComponent<Outline>();
        }
        
        _canInteract = results.Count > 0;
    }

    public void Interact()
    {
        Debug.Log($"Interacting with PileResults: {gameObject.name}");
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        if (playerController.CanTakeItem())
        {
            playerController.PutIntoInventory(Take().gameObject); 
        }
    }

    public void SetCanInteract(bool value)
    {
        _canInteract = value;
    }

    public void Add(QuestResultBehaviour result)
    {
        results.Push(result);
        _canInteract = true;
    }

    public QuestResultBehaviour Take()
    {
        if (results.Count == 0) return null;

        var item = results.Pop();
    
        _canInteract = results.Count > 0;

        return item;
    }

    public QuestResultBehaviour Peek()
    {
        if (results.Count > 0)
        {
            return results.Peek();
        }
        return null;
    }

    public int Count => results.Count;
}
