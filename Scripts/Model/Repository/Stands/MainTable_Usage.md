# MainTable GiveStat() and RemoveStat() Usage Guide

## Overview

The `MainTable` class now includes `GiveStat()` and `RemoveStat()` methods that allow you to modify hero statistics dynamically. These methods can:
- Increase or decrease rank (e.g., from A to S, or S to A)
- Add or remove SkillTypes
- Increase or decrease rewards

## Basic Usage

### Simple Usage (Default Behavior)

```csharp
// Upgrade hero's rank by one level (e.g., A -> S)
mainTable.GiveStat();

// Downgrade hero's rank by one level (e.g., S -> A)
mainTable.RemoveStat();
```

### Advanced Usage with StatModifier

The methods accept a `StatModifier` parameter for more control:

#### Upgrade/Downgrade Rank

```csharp
// Upgrade rank
mainTable.GiveStat(new StatModifier(StatModifierType.UpgradeRank));

// Downgrade rank
mainTable.RemoveStat(new StatModifier(StatModifierType.DowngradeRank));
```

#### Add/Remove Skills

```csharp
// Add Attack skill to hero
mainTable.GiveStat(new StatModifier(StatModifierType.AddSkill, SkillType.Attack));

// Remove Defense skill from hero
mainTable.RemoveStat(new StatModifier(StatModifierType.RemoveSkill, SkillType.Defense));
```

#### Increase/Decrease Rewards

```csharp
// Increase reward by 100 gold
mainTable.GiveStat(new StatModifier(StatModifierType.IncreaseReward, 100));

// Decrease reward by 50 gold
mainTable.RemoveStat(new StatModifier(StatModifierType.DecreaseReward, 50));
```

## StatModifierType Enum

The following modification types are available:

- `UpgradeRank` - Improves rank by one level (E -> F -> D -> C -> B -> A -> S)
- `DowngradeRank` - Decreases rank by one level (S -> A -> B -> C -> D -> F -> E)
- `AddSkill` - Adds a specific skill if not already present
- `RemoveSkill` - Removes a specific skill if present
- `IncreaseReward` - Increases reward by specified amount
- `DecreaseReward` - Decreases reward by specified amount (minimum 0)

## SkillType Enum

Available skill types:
- `Attack`
- `Defense`
- `Mobility`
- `Charisma`
- `Intelligence`

## Rank Enum

Ranks from highest to lowest:
- `S` (highest)
- `A`
- `B`
- `C`
- `D`
- `F`
- `E` (lowest)

## Implementation Details

The methods work on the `currentResult` field of the `MainTable` class. They:
1. Check if `currentResult` is set
2. Get the hero from the quest result
3. Get the stats from the hero
4. Apply the modifier to the stats

If any of these objects are null, the methods return early without making changes.

## Example Scenarios

### Scenario 1: Reward a Hero for Excellent Performance

```csharp
// Upgrade rank
mainTable.GiveStat(new StatModifier(StatModifierType.UpgradeRank));

// Add Charisma skill
mainTable.GiveStat(new StatModifier(StatModifierType.AddSkill, SkillType.Charisma));

// Increase reward
mainTable.GiveStat(new StatModifier(StatModifierType.IncreaseReward, 200));
```

### Scenario 2: Penalize a Hero for Poor Performance

```csharp
// Downgrade rank
mainTable.RemoveStat(new StatModifier(StatModifierType.DowngradeRank));

// Remove a skill
mainTable.RemoveStat(new StatModifier(StatModifierType.RemoveSkill, SkillType.Mobility));

// Decrease reward
mainTable.RemoveStat(new StatModifier(StatModifierType.DecreaseReward, 50));
```

### Scenario 3: Adjust Quest Rewards Based on Difficulty

```csharp
// For hard quests, increase rewards
if (quest.difficulty == QuestDifficulty.Hard)
{
    mainTable.GiveStat(new StatModifier(StatModifierType.IncreaseReward, 150));
}

// For easy quests, decrease rewards
if (quest.difficulty == QuestDifficulty.Easy)
{
    mainTable.RemoveStat(new StatModifier(StatModifierType.DecreaseReward, 30));
}
```

## Notes

- Rank upgrades stop at `S` (highest rank)
- Rank downgrades stop at `E` (lowest rank)
- Skills can only be added if not already present
- Skills can be removed multiple times safely (no error if skill doesn't exist)
- Rewards cannot go below 0
