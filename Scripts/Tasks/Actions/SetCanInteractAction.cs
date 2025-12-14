using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class SetCanInteractAction : Action
{
    [Tooltip("The value to set for canInteract")]
    [SerializeField] private bool canInteractValue;

    private CharacterBehaviour characterBehaviour;
    private HeroBehaviour heroBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        characterBehaviour = gameObject.GetComponent<CharacterBehaviour>();
        heroBehaviour = gameObject.GetComponent<HeroBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (characterBehaviour == null)
            return TaskStatus.Failure;

        if (ShouldPlaceHeroItems())
        {
            var questResult = heroBehaviour.currentQuestResultBehaviour.getQuestResult();
            if (questResult.state == QuestResultState.Taken)
            {
                heroBehaviour.PlaceHeroCardQuestResultInMainTable();
            }
            else if (questResult.state == QuestResultState.Assigned)
            {
                heroBehaviour.PlaceHeroCardQuestResultActualStatsInResultTable();
            }
        }

        characterBehaviour.SetCanInteract(canInteractValue);
        return TaskStatus.Success;
    }
    
    private bool ShouldPlaceHeroItems()
    {
        if (heroBehaviour == null || 
            heroBehaviour.heroCard == null || 
            heroBehaviour.currentQuestResultBehaviour == null)
        {
            return false;
        }
        
        var questResult = heroBehaviour.currentQuestResultBehaviour.getQuestResult();
        return questResult != null && 
               (questResult.state == QuestResultState.Taken || questResult.state == QuestResultState.Assigned);
    }
}
