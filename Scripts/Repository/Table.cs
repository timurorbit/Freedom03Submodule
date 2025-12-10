using System;
using _Game.Scripts.Behaviours;
using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Line line;
    
    [SerializeField]
    private Outline outline;

    protected bool _canInteract = true;
    public bool canInteract => _canInteract;

    protected virtual void Awake()
    {
        if (line == null)
        {
            line = GetComponentInChildren<Line>();
        }
        if (outline == null)
        {
            outline = GetComponent<Outline>();
        }
    }

    public GameObject GetClosestFreeSpot()
    {
        if (line != null)
        {
            return line.GetLastAvailableSpot();
        }
        return null;
    }
    public virtual void Interact()
    {
        outline.enabled = false;
    }
}