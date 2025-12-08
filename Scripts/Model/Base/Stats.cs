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
    
    public void UpgradeRank()
    {
        if (rank != Rank.S)
        {
            rank = (Rank)((int)rank - 1);
        }
    }
    
    public void DowngradeRank()
    {
        if (rank != Rank.E)
        {
            rank = (Rank)((int)rank + 1);
        }
    }
    
    public void AddSkill(SkillType skillType)
    {
        if (!skills.Contains(skillType))
        {
            skills.Add(skillType);
        }
    }
    
    public void RemoveSkill(SkillType skillType)
    {
        skills.Remove(skillType);
    }
    
    public void IncreaseReward(int amount)
    {
        if (amount > 0)
        {
            reward += amount;
        }
    }
    
    public void DecreaseReward(int amount)
    {
        if (amount > 0)
        {
            reward = Mathf.Max(0, reward - amount);
        }
    }
}