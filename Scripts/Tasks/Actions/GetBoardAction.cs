using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

public class GetBoardAction : Action
{
    [SerializeField] private SharedVariable<GameObject> Result;
    
    [SerializeField] private SharedVariable<GameObject> StayingTranform;

    public override TaskStatus OnUpdate()
    {
        if (GuildRepository.Instance == null)
        {
            Debug.LogWarning("GetBoardAction: GuildRepository.Instance is null");
            return TaskStatus.Failure;
        }

        Board board = GuildRepository.Instance.GetBoard();
        
        if (board == null)
        {
            Debug.LogWarning("GetBoardAction: Board not found in GuildRepository");
            return TaskStatus.Failure;
        }

        if (Result != null)
        {
            Result.Value = board.gameObject;
        }

        if (StayingTranform != null)
        {
            StayingTranform.Value = board.stayingTransforms[Random.Range(0, board.stayingTransforms.Count)].gameObject;
        }

        return TaskStatus.Success;
    }
}
