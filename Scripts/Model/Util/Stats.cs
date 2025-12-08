using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{
    public Rank rank;
    [SerializeField] public List<SkillType> skills;
    public int baseReward;
}