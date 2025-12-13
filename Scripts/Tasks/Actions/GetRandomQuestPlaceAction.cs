using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace _Game.Scripts.Tasks.Actions
{
    public class GetRandomQuestPlaceAction : Action
    {
        [SerializeField] SharedVariable<GameObject> Result;

        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            var exit = GuildRepository.Instance.getRandomQuestPlace();
        
            if (exit == null)
                return TaskStatus.Failure;

            if (Result != null)
                Result.Value = exit;

            return TaskStatus.Success;
        }
    }
}