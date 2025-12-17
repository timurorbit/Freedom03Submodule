using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace _Game.Scripts.Tasks.Actions
{
    public class DestroyGameObjectAction : Action
    {
        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            if (gameObject == null)
            {
                Debug.LogWarning("DestroyGameObjectAction: gameObject is null");
                return TaskStatus.Failure;
            }

            GameObject.Destroy(gameObject);

            return TaskStatus.Success;
        }
    }
}
