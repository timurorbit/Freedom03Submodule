using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class AddQuestResultOnBoardAction : Action
{
    [SerializeField] private SharedVariable<Board> BoardVariable;

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

        // Get the Board from SharedVariable
        Board board = BoardVariable?.Value;
        if (board == null)
        {
            Debug.LogWarning("AddQuestResultOnBoardAction: Board not found in SharedVariable");
            return TaskStatus.Failure;
        }

        // Add the current quest result to the board
        board.AddItemToBoard(heroBehaviour.currentQuestResultBehaviour.gameObject);
        
        // Clear the reference to prevent stale references
        heroBehaviour.currentQuestResultBehaviour = null;

        return TaskStatus.Success;
    }
}
