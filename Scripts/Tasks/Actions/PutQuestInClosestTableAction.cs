using _Game.Scripts.Behaviours;
using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class PutQuestInClosestTableAction : Action
{
    private PeasantBehaviour peasantBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        peasantBehaviour = gameObject.GetComponent<PeasantBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (peasantBehaviour == null)
            return TaskStatus.Failure;

        peasantBehaviour.PutQuestResultInQuestPile();
        return TaskStatus.Success;
    }
}
