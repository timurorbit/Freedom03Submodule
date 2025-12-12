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
            Debug.LogWarning("AddQuestResultOnBoardAction: currentQuestResultBehaviour is null");
            return TaskStatus.Failure;
        }

        // Find the Board
        Board board = Object.FindAnyObjectByType<Board>();
        if (board == null)
        {
            Debug.LogWarning("AddQuestResultOnBoardAction: No Board found in scene");
            return TaskStatus.Failure;
        }

        // Add the current quest result to the board
        board.AddItemToBoard(heroBehaviour.currentQuestResultBehaviour.gameObject);

        return TaskStatus.Success;
    }
}
