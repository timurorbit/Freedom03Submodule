using UnityEngine;

public class QuestTable : Table
{
    [SerializeField]
    private PileQuests pileQuests;
    
    [SerializeField]
    private PileResults pileResults;

    protected override void Awake()
    {
        base.Awake();
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

        if (pileQuests.Count > 0)
        {
            _canInteract = true;
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
            if (pileQuests.Count <= 0)
            {
                _canInteract = false;
            }
            var result = new QuestResult(quest, prediction);
            pileResults.Add(result);
        }
    }

    public override void Interact()
    {
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        playerController.SetActiveQuestCamera(true);
        base.Interact();
    }
}