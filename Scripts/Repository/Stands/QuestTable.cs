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
        if (pileQuests != null)
        {
            pileQuests.Add(quest);
        }
    }

    public QuestResult TakeFromResult()
    {
        if (pileResults != null)
        {
            return pileResults.Take();
        }
        return null;
    }

    public void inspectQuest(Stats prediction)
    {
        if (pileQuests != null && pileResults != null)
        {
            var quest = pileQuests.Take();
            var result = new QuestResult(quest, prediction);
            pileResults.Add(result);
        }
    }
}