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


    public void PlaceHeroCardQuestResultActualStatsInResultTable()
    {
        QuestResultTable questResultTable = GuildRepository.Instance.GetClosestTable<QuestResultTable>();

        if (questResultTable == null)
        {
            Debug.LogWarning("HeroBehaviour: No QuestResultTable found");
            return;
        }

        ActualStatsBehaviour actualStats = null;
        actualStats = currentQuestResultBehaviour.getActualStatsView().GetComponent<ActualStatsBehaviour>();


        questResultTable.PlaceHeroCardAndQuestResultAndQuestStats(heroCard, currentQuestResultBehaviour, actualStats);
        questResultTable.currentHeroBehaviour = this;
    }

    public void HandleHeroInteraction()
    {
        QuestResultState state = currentQuestResultBehaviour.getQuestResult().state;
        if (state == QuestResultState.Taken)
        {
            return;
        }

        if (state == QuestResultState.Assigned)
        {
            Debug.LogError("Hero lifecycle has ended, going to die");
        }
    }
}