using System.Collections.Generic;
using UnityEngine;

public class PileQuests : MonoBehaviour
{
    private Stack<QuestResultBehaviour> quests = new();

    public void Add(QuestResultBehaviour quest)
    {
        quests.Push(quest);
    }

    public QuestResultBehaviour Take()
    {
        return quests.Pop();
    }

    public int Count => quests.Count;
}
