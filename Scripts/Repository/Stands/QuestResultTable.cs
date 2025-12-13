using UnityEngine;

public class QuestResultTable : Table
{
    [Header("Positions")]
    [SerializeField] private Transform currentQuestResultPosition;
    [SerializeField] private Transform currentHeroCardPosition;
    [SerializeField] private Transform currentActualQuestStatsBehaviourTransform;
    
    
    [Header("Items")]
    [SerializeField] public QuestResultBehaviour currentQuestResultBehaviour;
    [SerializeField] public HeroCardBehaviour currentHeroCardBehaviour;
    [SerializeField] public HeroBehaviour currentHeroBehaviour;
    [SerializeField] public ActualStatsBehaviour currentActualResultBehaviourScript;
    
    
}