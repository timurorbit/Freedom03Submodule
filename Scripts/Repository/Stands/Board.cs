using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

class Board : Table
{
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    
    private List<QuestResultBehaviour> questsToTake;
    private Sequence currentSequence;

    protected override void Awake()
    {
        base.Awake();
    }

    public Board()
    {
        questsToTake = new List<QuestResultBehaviour>();
    }

    public Board(List<QuestResultBehaviour> questsToTake)
    {
        this.questsToTake = questsToTake;
    }

    public void addQuestToBoard(QuestResultBehaviour quest)
    {
        questsToTake.Add(quest);
    }

    public QuestResultBehaviour getQuestByStats(Stats stats)
    {
        //TODO implement better logic 
        return getFirstQuestFromBoard();
    }

    public QuestResultBehaviour getFirstQuestFromBoard()
    {
        var toTake = questsToTake[0];
        toTake.SwitchState(QuestResultState.Taken);
        questsToTake.RemoveAt(0);
        return toTake;
    }

    public override void Interact()
    {
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        if (!playerController.IsItemQuest())
        {
            return;
        }
        var item = playerController.GetFromInventory();
        AddItemToBoard(item);
        base.Interact();
    }

    public void AddItemToBoard(GameObject item)
    {
        item.transform.SetParent(transform);
        addQuestToBoard(item.GetComponent<QuestResultBehaviour>());
        TweenToRandomPosition(item.transform);
    }

    private void TweenToBoardPosition(Transform objectTransform)
    {
    }
    
    private void OnDestroy()
    {
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }
    }
}