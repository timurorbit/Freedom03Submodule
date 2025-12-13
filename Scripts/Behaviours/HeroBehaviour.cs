using System.Collections.Generic;
using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{
    [SerializeField] public Transform questPosition;
    [SerializeField] public Transform heroCardPosition;
    [SerializeField] public HeroCardBehaviour heroCard;
    [SerializeField] public QuestResultBehaviour currentQuestResultBehaviour;
    [SerializeField] public List<StatModifierBehaviour> statModifiers = new List<StatModifierBehaviour>();
    [SerializeField] public Transform statModifiersParent;
    [SerializeField] public bool Approved;
    
    [Header("Only for testing purposes")]
    [SerializeField]
    public HeroObject baseHero;
    
    public void PlaceHeroCardQuestResultInMainTable()
    {
        MainTable mainTable = GuildRepository.Instance.GetClosestTable<MainTable>();
        
        if (mainTable == null)
        {
            Debug.LogWarning("HeroBehaviour: No MainTable found");
            return;
        }
        
        mainTable.PlaceHeroCardAndQuestResult(heroCard, currentQuestResultBehaviour);
        mainTable.PlaceHero(this);
    }
}