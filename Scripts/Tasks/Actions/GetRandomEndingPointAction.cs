using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace _Game.Scripts.Tasks.Actions
{
    public class GetRandomEndingPointAction : Action
    {
        [SerializeField] SharedVariable<Transform> Result;

        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            if (GuildRepository.Instance == null)
            {
                Debug.LogWarning("GetRandomEndingPointAction: GuildRepository.Instance is null");
                return TaskStatus.Failure;
            }

            var endingPoint = GuildRepository.Instance.GetRandomEndingPoint();
        
            if (endingPoint == null)
            {
                Debug.LogWarning("GetRandomEndingPointAction: No ending point found");
                return TaskStatus.Failure;
            }

            if (Result != null)
                Result.Value = endingPoint;

            return TaskStatus.Success;
        }
    }
}
