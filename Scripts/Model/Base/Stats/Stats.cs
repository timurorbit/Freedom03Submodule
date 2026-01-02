using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stats
{
    public Rank rank;
    [SerializeField] private SingleStat attackStat;
    [SerializeField] private SingleStat defenseStat;
    [SerializeField] private SingleStat mobilityStat;
    [SerializeField] private SingleStat charismaStat;
    [SerializeField] private SingleStat intelligenceStat;
    public event EventHandler OnStatsChanged;
    [SerializeField] public int reward;

    public Stats(Rank rank, int attackStatAmount, int defenceStatAmount, int mobilityStatAmount, int charismaStatAmount, int intelligenceStatAmount, int reward)
    {
        this.rank = rank;
        attackStat = new SingleStat(attackStatAmount);
        defenseStat = new SingleStat(defenceStatAmount);
        mobilityStat = new SingleStat(mobilityStatAmount);
        charismaStat = new SingleStat(charismaStatAmount);
        intelligenceStat = new SingleStat(intelligenceStatAmount);
        this.reward = reward;
    }

    public Stats(Stats stats)
    {
        rank = stats.rank;
        attackStat = new SingleStat(stats.attackStat.GetStatAmount());
        defenseStat = new SingleStat(stats.defenseStat.GetStatAmount());
        mobilityStat = new SingleStat(stats.mobilityStat.GetStatAmount());
        charismaStat = new SingleStat(stats.charismaStat.GetStatAmount());
        intelligenceStat = new SingleStat(stats.intelligenceStat.GetStatAmount());
        reward = stats.reward;
    }

    public Stats()
    {
        rank = Rank.None;
        attackStat = new SingleStat(0);
        defenseStat = new SingleStat(0);
        mobilityStat = new SingleStat(0);
        charismaStat = new SingleStat(0);
        intelligenceStat = new SingleStat(0);
        reward = 0;
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
    
    public void AddSkill(SkillType skillType, int amount)
    {
        switch (skillType)
        {
            case SkillType.Attack:
                attackStat.SetStatAmount(attackStat.GetStatAmount() + amount);
                break;
            case SkillType.Defense:
                defenseStat.SetStatAmount(defenseStat.GetStatAmount() + amount);
                break;
            case SkillType.Mobility:
                mobilityStat.SetStatAmount(mobilityStat.GetStatAmount() + amount);
                break;
            case SkillType.Charisma:
                charismaStat.SetStatAmount(charismaStat.GetStatAmount() + amount);
                break;
            case SkillType.Intelligence:
                intelligenceStat.SetStatAmount(intelligenceStat.GetStatAmount() + amount);
                break;
        }
    }
    
    public void RemoveSkill(SkillType skillType, int amount)
    {
        switch (skillType)
        {
            case SkillType.Attack:
                attackStat.SetStatAmount(attackStat.GetStatAmount() - amount);
                break;
            case SkillType.Defense:
                defenseStat.SetStatAmount(defenseStat.GetStatAmount() - amount);
                break;
            case SkillType.Mobility:
                mobilityStat.SetStatAmount(mobilityStat.GetStatAmount() - amount);
                break;
            case SkillType.Charisma:
                charismaStat.SetStatAmount(charismaStat.GetStatAmount() - amount);
                break;
            case SkillType.Intelligence:
                intelligenceStat.SetStatAmount(intelligenceStat.GetStatAmount() - amount);
                break;
        }
    }
    
    private SingleStat GetSingleStat(SkillType statType)
    {
        switch (statType)
        {
            default:
            case SkillType.Attack: return attackStat;
            case SkillType.Defense: return defenseStat;
            case SkillType.Mobility: return mobilityStat;
            case SkillType.Charisma: return charismaStat;
            case SkillType.Intelligence: return intelligenceStat;
        }
    }
    
    
    public int GetStatAmount(SkillType statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }
    
    public void SetStatAmount(SkillType statType, int value)
    {
        GetSingleStat(statType).SetStatAmount(value);
        if (OnStatsChanged != null)
            OnStatsChanged(this, EventArgs.Empty);
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
    
    
    // For UI only. better to keep if more than max for applying buffs/debuffs
    public float GetStatNormalized(SkillType statType)
    {
        return GetSingleStat(statType).GetStatNormalized();
    }
    

    [Serializable]
    private class SingleStat
    {
        [SerializeField]
        private int stat;

        public SingleStat(int statAmount)
        {
            SetStatAmount(statAmount);
        }

        public void SetStatAmount(int value)
        {
            stat = value;
        }

        public int GetStatAmount()
        {
            return stat;
        }
        
        public float GetStatNormalized()
        {
            if (stat < 0)
            {
                return 0f;
            }
            return (float)(stat) / STAT_MAX;
        }

        
        //TODO remove or change (for now used only in UI in normalized)
        public const int STAT_MAX = 20;
    }
}