using System;

[Serializable]
public class StatModifier
{
    public StatModifierType type;
    public SkillType skillType;
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
                stats.AddSkill(skillType);
                break;
            case StatModifierType.RemoveSkill:
                stats.RemoveSkill(skillType);
                break;
            case StatModifierType.IncreaseReward:
                stats.IncreaseReward(rewardAmount);
                break;
            case StatModifierType.DecreaseReward:
                stats.DecreaseReward(rewardAmount);


                break;
        }
    }
}