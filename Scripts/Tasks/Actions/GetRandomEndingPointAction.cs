using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace _Game.Scripts.Tasks.Actions
{
    public class GetRandomEndingPointAction : Action
    {
        [SerializeField] public SharedVariable<GameObject> StayingTransform;

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

            var endingPoint = GuildRepository.Instance.GetRandomEndingPoints();
        
            if (endingPoint == null)
            {
                Debug.LogWarning("GetRandomEndingPointAction: No ending point found");
                return TaskStatus.Failure;
            }

            if (StayingTransform != null)
                StayingTransform.Value = endingPoint[Random.Range(0, endingPoint.Count)].gameObject;

            return TaskStatus.Success;
        }
    }
}
