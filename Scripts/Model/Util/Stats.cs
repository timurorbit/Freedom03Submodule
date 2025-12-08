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
}