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
        MainTable mainTable = GuildRepository.Instance.GetClosestTable<MainTable>();
        
        if (mainTable == null)
        {
            Debug.LogWarning("HeroBehaviour: No MainTable found");
            return;
        }
        
        mainTable.PlaceHeroCardAndQuestResult(heroCard, currentQuestResultBehaviour);
    }
}
