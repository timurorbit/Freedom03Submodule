using System.Collections.Generic;
using UnityEngine;

class Board : Table
{
    private List<QuestResultBehaviour> questsToTake;

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
        item.transform.SetParent(transform);
        addQuestToBoard(item.GetComponent<QuestResultBehaviour>());
        base.Interact();
    }
}