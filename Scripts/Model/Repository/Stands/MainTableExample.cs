using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example class demonstrating MainTable GiveStat() and RemoveStat() usage
/// This is for reference and testing purposes
/// </summary>
public class MainTableExample : MonoBehaviour
{
    public void ExampleUsage()
    {
        // Create a mock setup for demonstration
        MainTable mainTable = new MainTable();
        
        // Create a hero with initial stats
        HeroObject heroTemplate = ScriptableObject.CreateInstance<HeroObject>();
        heroTemplate.stats = new Stats(
            Rank.B, 
            new List<SkillType> { SkillType.Attack, SkillType.Defense }, 
            100
        );
        Hero hero = new Hero(heroTemplate);
        
        // Create a quest
        QuestObject questTemplate = ScriptableObject.CreateInstance<QuestObject>();
        Quest quest = new Quest(questTemplate);
        
        // Create a quest result and assign to mainTable
        mainTable.currentResult = new QuestResult(hero, quest, heroTemplate.stats);
        
        Debug.Log("=== Initial Stats ===");
        LogStats(hero);
        
        // Example 1: Upgrade rank
        Debug.Log("\n=== Example 1: Upgrade Rank ===");
        mainTable.GiveStat(new StatModifier(StatModifierType.UpgradeRank));
        Debug.Log("After upgrading rank:");
        LogStats(hero);
        
        // Example 2: Add a new skill
        Debug.Log("\n=== Example 2: Add Charisma Skill ===");
        mainTable.GiveStat(new StatModifier(StatModifierType.AddSkill, SkillType.Charisma));
        Debug.Log("After adding Charisma skill:");
        LogStats(hero);
        
        // Example 3: Increase reward
        Debug.Log("\n=== Example 3: Increase Reward ===");
        mainTable.GiveStat(new StatModifier(StatModifierType.IncreaseReward, 50));
        Debug.Log("After increasing reward by 50:");
        LogStats(hero);
        
        // Example 4: Remove a skill
        Debug.Log("\n=== Example 4: Remove Defense Skill ===");
        mainTable.RemoveStat(new StatModifier(StatModifierType.RemoveSkill, SkillType.Defense));
        Debug.Log("After removing Defense skill:");
        LogStats(hero);
        
        // Example 5: Downgrade rank
        Debug.Log("\n=== Example 5: Downgrade Rank ===");
        mainTable.RemoveStat(new StatModifier(StatModifierType.DowngradeRank));
        Debug.Log("After downgrading rank:");
        LogStats(hero);
        
        // Example 6: Decrease reward
        Debug.Log("\n=== Example 6: Decrease Reward ===");
        mainTable.RemoveStat(new StatModifier(StatModifierType.DecreaseReward, 30));
        Debug.Log("After decreasing reward by 30:");
        LogStats(hero);
    }
    
    private void LogStats(Hero hero)
    {
        Stats stats = hero.GetStats();
        if (stats != null)
        {
            Debug.Log($"Rank: {stats.rank}");
            Debug.Log($"Skills: {string.Join(", ", stats.skills)}");
            Debug.Log($"Reward: {stats.reward}");
        }
    }
}
