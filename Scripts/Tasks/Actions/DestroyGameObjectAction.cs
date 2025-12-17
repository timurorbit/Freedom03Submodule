using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace _Game.Scripts.Tasks.Actions
{
    public class DestroyGameObjectAction : Action
    {
        [SerializeField] SharedVariable<GameObject> Target;

        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            if (Target == null || Target.Value == null)
            {
                Debug.LogWarning("DestroyGameObjectAction: Target is null");
                return TaskStatus.Failure;
            }

            GameObject.Destroy(Target.Value);

            return TaskStatus.Success;
        }
    }
}
