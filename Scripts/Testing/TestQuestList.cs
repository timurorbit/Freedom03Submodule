using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Testing/Test Quest List")]
public class TestQuestList : ScriptableObject
{
    public List<QuestObject> questTemplates = new List<QuestObject>();
    
    public List<HeroObject> heroTemplates = new List<HeroObject>();
}