using System.Collections.Generic;

public class QuestTable : Table
{
    private Stack<Quest> quests = new();
    private Stack<QuestResult> results = new();


    public void AddToQuests(Quest quest)
    {
        quests.Push(quest);
    }

    public QuestResult TakeFromResult()
    {
       return results.Pop();
    }

    public void inspectQuest(Stats prediction)
    {
        var quest = quests.Pop();
        var result = new QuestResult(quest, prediction);
        results.Push(result);
    }
}