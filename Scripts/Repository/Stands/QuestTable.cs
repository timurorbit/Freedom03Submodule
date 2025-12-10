using UnityEngine;

public class QuestTable : Table
{
    [SerializeField]
    private PileQuests pileQuests;
    
    [SerializeField]
    private PileResults pileResults;

    private void Awake()
    {
        if (pileQuests == null)
        {
            pileQuests = GetComponentInChildren<PileQuests>();
        }
        if (pileResults == null)
        {
            pileResults = GetComponentInChildren<PileResults>();
        }
    }

    public void AddToQuests(Quest quest)
    {
        pileQuests.Add(quest);
    }

    public QuestResult TakeFromResult()
    {
       return pileResults.Take();
    }

    public void inspectQuest(Stats prediction)
    {
        var quest = pileQuests.Take();
        var result = new QuestResult(quest, prediction);
        pileResults.Add(result);
    }
}