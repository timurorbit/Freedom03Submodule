using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Behaviours;

public class PileResults : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Outline outline;

    private Stack<QuestResult> results = new();
    private Stack<QuestResultBehaviour> resultBehaviours = new();

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

    public void Add(QuestResultBehaviour resultBehaviour)
    {
        resultBehaviours.Push(resultBehaviour);
    }

    public QuestResult Take()
    {
        if (results.Count > 0)
        {
            return results.Pop();
        }
        return null;
    }

    public QuestResultBehaviour TakeBehaviour()
    {
        if (resultBehaviours.Count > 0)
        {
            return resultBehaviours.Pop();
        }
        return null;
    }

    public QuestResultBehaviour PeekBehaviour()
    {
        if (resultBehaviours.Count > 0)
        {
            return resultBehaviours.Peek();
        }
        return null;
    }

    public int Count => results.Count;
    public int BehaviourCount => resultBehaviours.Count;
}
