using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class PutQuestInClosestTableAction : Action
{
    private QuestGiverBehaviour questGiverBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        questGiverBehaviour = gameObject.GetComponent<QuestGiverBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (questGiverBehaviour == null)
            return TaskStatus.Failure;

        questGiverBehaviour.PutQuestResultInQuestPile();
        return TaskStatus.Success;
    }
}
