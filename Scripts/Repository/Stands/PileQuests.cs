using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Behaviours;

public class PileQuests : MonoBehaviour
{
    private Stack<Quest> quests = new();

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
