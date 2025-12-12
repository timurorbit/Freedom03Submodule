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

        // Check if this is a HeroBehaviour with heroCard and QuestResultBehaviour in Taken state
        if (heroBehaviour != null && 
            heroBehaviour.heroCard != null && 
            heroBehaviour.currentQuestResultBehaviour != null)
        {
            var questResult = heroBehaviour.currentQuestResultBehaviour.getQuestResult();
            if (questResult != null && questResult.state == QuestResultState.Taken)
            {
                // Place hero card and quest result in main table
                heroBehaviour.PlaceHeroCardQuestResultInMainTable();
            }
        }

        characterBehaviour.SetCanInteract(canInteractValue);
        return TaskStatus.Success;
    }
}
