using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class QuestTableCanvas : MonoBehaviour
{
    [SerializeField]
    private QuestTable questTable;

    [SerializeField] private Toggle attackToggle;
    [SerializeField] private Toggle defenseToggle;
    [SerializeField] private Toggle mobilityToggle;
    [SerializeField] private Toggle magicToggle;
    [SerializeField] private Toggle charismaToggle;
    
    [SerializeField] private Rank currentRank;
    
    public MMF_Player nextScrollFeedback;
    
    
    [Header("Rank buttons")]
    [SerializeField]
    private RankButton rankSButton;
    [SerializeField]
    private RankButton rankAButton;
    [SerializeField]
    private RankButton rankBButton;
    [SerializeField]
    private RankButton rankCButton;
    [SerializeField]
    private RankButton rankDButton;
    [SerializeField]
    private RankButton rankEButton;
    

    private void Start()
    {
        
    }

    private void OnDestroy()
    {

    }

    private void UpdateCurrentQuestStats()
    {
        // Update the current quest in inspection
        if (questTable != null)
        {
            var currentQuest = questTable.GetCurrentInspection();
            if (currentQuest != null)
            {
                var stats = currentQuest.getQuestResult().GetPrediction();
                if (stats != null)
                {
                    
                }
            }
        }
    }
    
    public void Left()
    {
        if (questTable == null)
            return;

        var currentQuest = questTable.TakeFromInspection();
        nextScrollFeedback?.PlayFeedbacks();
        
        if (currentQuest != null)
        {
            UpdateQuestPrediction(currentQuest, QuestResultState.Predicted);
            questTable.AddToQuests(currentQuest);
        }
        
        var topFromResults = questTable.TakeFromResult();
        if (topFromResults != null)
        {
            UpdateView(topFromResults);
            questTable.MoveToInspection(topFromResults);
        }
    }

    private void UpdateView(QuestResultBehaviour quest)
    {
        var stats = quest.getQuestResult().GetPrediction();
        attackToggle.isOn = stats.GetStatAmount(SkillType.Attack) >= 1;
        defenseToggle.isOn = stats.GetStatAmount(SkillType.Defense) >= 1;
        mobilityToggle.isOn = stats.GetStatAmount(SkillType.Mobility) >= 1;
        magicToggle.isOn = stats.GetStatAmount(SkillType.Intelligence) >= 1;
        charismaToggle.isOn = stats.GetStatAmount(SkillType.Charisma) >= 1;
        UpdateRankButtonsView(stats.rank);
    }

    private void UpdateRankButtonsView(Rank rank) {
        rankSButton.setInactiveColor();
        rankAButton.setInactiveColor();
        rankBButton.setInactiveColor();
        rankCButton.setInactiveColor();
        rankDButton.setInactiveColor();
        rankEButton.setInactiveColor();

        switch (rank) {
            case Rank.S:
                rankSButton.setActiveColor();
                break;
            case Rank.A:
                rankAButton.setActiveColor();
                break;
            case Rank.B:
                rankBButton.setActiveColor();
                break;
            case Rank.C:
                rankCButton.setActiveColor();
                break;
            case Rank.D:
                rankDButton.setActiveColor();
                break;
            case Rank.E:
                rankEButton.setActiveColor();
                break;
        }
    }

    public void Right()
    {
        if (questTable == null)
            return;
        nextScrollFeedback?.PlayFeedbacks();

        var currentQuest = questTable.TakeFromInspection();
        
        if (currentQuest != null)
        {
            UpdateQuestPrediction(currentQuest, QuestResultState.Opened);
            questTable.AddToResults(currentQuest);
        }
        
        var topFromQuests = questTable.TakeFromQuests();
        if (topFromQuests != null)
        {
            questTable.MoveToInspection(topFromQuests);
            UpdateView(topFromQuests);
        }
    }
    
    public void UpdateQuestRank(Rank rank)
    {
        var currentQuest = questTable.GetCurrentInspection();
        UpdateRankButtonsView(rank);
        if (currentQuest == null)
        {
            return;
        }
        currentQuest.getQuestResult().GetPrediction().rank = rank;
        UpdateQuestPrediction(currentQuest, currentQuest.getQuestResult().state);
    }
    
    public void UpdateCurrentQuestViewInTable()
    {
        var currentQuest = questTable.GetCurrentInspection();
        if (currentQuest == null)
        {
            return;
        }
        UpdateQuestPrediction(currentQuest, currentQuest.getQuestResult().state);
    }

    private void UpdateQuestPrediction(QuestResultBehaviour currentQuest, QuestResultState newState)
    {
        var attack = attackToggle.isOn ? 1 : 0;
        var defense = defenseToggle.isOn ? 1 : 0;
        var mobility = mobilityToggle.isOn ? 1 : 0;
        var intelligence = magicToggle.isOn ? 1 : 0;
        var charisma = charismaToggle.isOn ? 1 : 0;
        var stats = new Stats(currentQuest.getQuestResult().GetPrediction().rank,
            attack,
            defense,
            mobility,
            charisma,
            intelligence,
            0);
        currentQuest.setPrediction(stats);
        currentQuest.SwitchState(newState);
    }
    
    public void SetRankS()
    {
        UpdateQuestRank(Rank.S);
    }
    
    public void SetRankA()
    {
        UpdateQuestRank(Rank.A);
    }
    
    public void SetRankB()
    {
        UpdateQuestRank(Rank.B);
    }
    
    public void SetRankC()
    {
        UpdateQuestRank(Rank.C);
    }
    
    public void SetRankD()
    {
        UpdateQuestRank(Rank.D);
    }
    
    public void SetRankE()
    {
        UpdateQuestRank(Rank.E);
    }
}