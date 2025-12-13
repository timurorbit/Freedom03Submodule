using System.Collections.Generic;
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
        
        if (currentQuest != null)
        {
            UpdateQuestStats(currentQuest, QuestResultState.Predicted);
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
        attackToggle.isOn = stats.skills.Contains(SkillType.Attack);
        defenseToggle.isOn = stats.skills.Contains(SkillType.Defense);
        mobilityToggle.isOn = stats.skills.Contains(SkillType.Mobility);
        magicToggle.isOn = stats.skills.Contains(SkillType.Intelligence);
        charismaToggle.isOn = stats.skills.Contains(SkillType.Charisma);
        rankDropdown.SetSelectedRank(stats.rank);
    }

    public void Right()
    {
        if (questTable == null)
            return;

        var currentQuest = questTable.TakeFromInspection();
        
        if (currentQuest != null)
        {
            UpdateQuestStats(currentQuest, QuestResultState.Opened);
            questTable.AddToResults(currentQuest);
        }
        
        var topFromQuests = questTable.TakeFromQuests();
        if (topFromQuests != null)
        {
            questTable.MoveToInspection(topFromQuests);
            UpdateView(topFromQuests);
        }
    }

    private void UpdateQuestStats(QuestResultBehaviour currentQuest, QuestResultState newState)
    {
        
        var stats = new Stats(rankDropdown.GetSelectedRank(), getSkillListFromUI(), 0);
        currentQuest.setPrediction(stats);
        currentQuest.SwitchState(newState);
    }

    private List<SkillType> getSkillListFromUI()
    {
        var list = new List<SkillType>();
        if (attackToggle.isOn)
            list.Add(SkillType.Attack);
        if (defenseToggle.isOn)
            list.Add(SkillType.Defense);
        if (mobilityToggle.isOn)
            list.Add(SkillType.Mobility);
        if (magicToggle.isOn)
            list.Add(SkillType.Intelligence);
        if (charismaToggle.isOn)
            list.Add(SkillType.Charisma);
        return list;
    }
}
