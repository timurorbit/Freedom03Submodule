using System.Collections.Generic;
using MoreMountains.Feedbacks;
using NUnit.Framework;
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
    
    [SerializeField] private DropdownRankOptions rankDropdown;
    
    
    [Header("Feedbacks")]
    /// a MMF_Player to play when the Hero starts jumping
    public MMF_Player AddFeedback;
    /// a MMF_Player to play when the Hero lands after a jump
    public MMF_Player nextScrollFeedback;

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
        rankDropdown.SetSelectedRank(stats.rank);
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

    private void UpdateQuestPrediction(QuestResultBehaviour currentQuest, QuestResultState newState)
    {
        var attack = attackToggle.isOn ? 1 : 0;
        var defense = defenseToggle.isOn ? 1 : 0;
        var mobility = mobilityToggle.isOn ? 1 : 0;
        var intelligence = magicToggle.isOn ? 1 : 0;
        var charisma = charismaToggle.isOn ? 1 : 0;
        var stats = new Stats(rankDropdown.GetSelectedRank(),
            attack,
            defense,
            mobility,
            charisma,
            intelligence,
            0);
        currentQuest.setPrediction(stats);
        currentQuest.SwitchState(newState);
    }
}
