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
        if (quests.Count > 0)
        {
            return quests.Pop();
        }
        return null;
    }

    public QuestResultBehaviour Peek()
    {
        if (quests.Count > 0)
        {
            return quests.Peek();
        }
        return null;
    }

    public int Count => quests.Count;
}
