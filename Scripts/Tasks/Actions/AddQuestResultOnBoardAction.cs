using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using UnityEngine;

public class AddQuestResultOnBoardAction : Action
{
    private HeroBehaviour heroBehaviour;

    public override void OnAwake()
    {
        base.OnAwake();
        heroBehaviour = gameObject.GetComponent<HeroBehaviour>();
    }

    public override TaskStatus OnUpdate()
    {
        if (heroBehaviour == null)
            return TaskStatus.Failure;

        if (heroBehaviour.currentQuestResultBehaviour == null)
        {
            return TaskStatus.Failure;
        }

        Board board = GuildRepository.Instance.GetBoard();
        if (board == null)
        {
            Debug.LogWarning("AddQuestResultOnBoardAction: Board not found in GuildRepository");
            return TaskStatus.Failure;
        }

        board.AddItemToBoard(heroBehaviour.currentQuestResultBehaviour.gameObject);
        
        heroBehaviour.currentQuestResultBehaviour = null;

        return TaskStatus.Success;
    }
}
