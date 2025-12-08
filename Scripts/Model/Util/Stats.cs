using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stats
{
    public Rank rank;
    [SerializeField] public List<SkillType> skills;
    [SerializeField] public int reward;

    public Stats(Rank rank, List<SkillType> skills, int reward)
    {
        this.rank = rank;
        this.skills = skills;
        this.reward = reward;
    }

    public Stats(Stats stats)
    {
        rank = stats.rank;
        skills = new List<SkillType>(stats.skills);
        reward = stats.reward;
    }
    
    // Upgrade rank (move from lower to higher rank, e.g., A -> S)
    public void UpgradeRank()
    {
        if (rank != Rank.S)
        {
            rank = (Rank)((int)rank - 1);
        }
    }
    
    // Downgrade rank (move from higher to lower rank, e.g., S -> A)
    public void DowngradeRank()
    {
        if (rank != Rank.E)
        {
            rank = (Rank)((int)rank + 1);
        }
    }
    
    // Add a skill type if not already present
    public void AddSkill(SkillType skillType)
    {
        if (!skills.Contains(skillType))
        {
            skills.Add(skillType);
        }
    }
    
    // Remove a skill type if present
    public void RemoveSkill(SkillType skillType)
    {
        skills.Remove(skillType);
    }
    
    // Increase reward by specified amount
    public void IncreaseReward(int amount)
    {
        if (amount > 0)
        {
            reward += amount;
        }
    }
    
    // Decrease reward by specified amount
    public void DecreaseReward(int amount)
    {
        if (amount > 0)
        {
            reward = Mathf.Max(0, reward - amount);
        }
    }
}