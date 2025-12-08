using System;

[Serializable]
public class StatModifier
{
    public StatModifierType type;
    public SkillType? skillType;
    public int rewardAmount;
    
    public StatModifier(StatModifierType type)
    {
        this.type = type;
    }
    
    public StatModifier(StatModifierType type, SkillType skillType)
    {
        this.type = type;
        this.skillType = skillType;
    }
    
    public StatModifier(StatModifierType type, int rewardAmount)
    {
        this.type = type;
        this.rewardAmount = rewardAmount;
    }
    
    // Apply this modifier to the given stats
    public void Apply(Stats stats)
    {
        if (stats == null) return;
        
        switch (type)
        {
            case StatModifierType.UpgradeRank:
                stats.UpgradeRank();
                break;
            case StatModifierType.DowngradeRank:
                stats.DowngradeRank();
                break;
            case StatModifierType.AddSkill:
                if (skillType.HasValue)
                {
                    stats.AddSkill(skillType.Value);
                }
                break;
            case StatModifierType.RemoveSkill:
                if (skillType.HasValue)
                {
                    stats.RemoveSkill(skillType.Value);
                }
                break;
            case StatModifierType.IncreaseReward:
                if (rewardAmount > 0)
                {
                    stats.IncreaseReward(rewardAmount);
                }
                break;
            case StatModifierType.DecreaseReward:
                if (rewardAmount > 0)
                {
                    stats.DecreaseReward(rewardAmount);
                }
                break;
        }
    }
}
