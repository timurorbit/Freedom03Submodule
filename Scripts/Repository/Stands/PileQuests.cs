using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Behaviours;

public class PileQuests : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Outline outline;

    private Stack<Quest> quests = new();

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
        Debug.LogError($"Interacting with PileQuests: {gameObject.name}");
    }

    public void SetCanInteract(bool value)
    {
        _canInteract = value;
    }

    public void Add(Quest quest)
    {
        quests.Push(quest);
    }

    public Quest Take()
    {
        return quests.Pop();
    }

    public int Count => quests.Count;
}
