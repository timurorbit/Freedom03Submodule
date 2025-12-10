using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Behaviours;

public class PileResults : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Outline outline;

    private Stack<QuestResult> results = new();

    private bool _canInteract = true;
    public bool canInteract => _canInteract;

    private void Awake()
    {
        if (outline == null)
        {
            outline = GetComponent<Outline>();
        }
    }

    public void Interact()
    {
        Debug.Log($"Interacting with PileResults: {gameObject.name}");
        _canInteract = false;
    }

    public void SetCanInteract(bool value)
    {
        _canInteract = value;
    }

    public void Add(QuestResult result)
    {
        results.Push(result);
    }

    public QuestResult Take()
    {
        return results.Pop();
    }

    public int Count => results.Count;
}
