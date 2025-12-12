using System;
using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{
    [SerializeField] public Transform questPosition;
    [SerializeField] public HeroCardBehaviour heroCard;
    [SerializeField] public QuestResultBehaviour currentQuestResultBehaviour;
    
    [Header("Only for testing purposes")]
    [SerializeField]
    public HeroObject baseHero;
    
    public void PlaceHeroCardQuestResultInMainTable()
    {
        // Get the closest main table from GuildInventory
        MainTable mainTable = GuildRepository.Instance.GetClosestMainTable();
        
        if (mainTable == null)
        {
            Debug.LogWarning("HeroBehaviour: No MainTable found");
            return;
        }
        
        // Place both hero card and quest result on the main table
        mainTable.PlaceHeroCardAndQuestResult(heroCard, currentQuestResultBehaviour);
    }
}
