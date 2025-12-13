using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.BehaviorDesigner.Runtime.Utility;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace _Game.Scripts.Tasks.Actions
{
    public class CustomSharedWait : Action
    {
        [Tooltip("The amount of time to wait (in seconds).")] [SerializeField]
        SharedVariable<float> m_Duration = 1;

        [Tooltip("Should the wait duration be randomized?")] [SerializeField]
        SharedVariable<bool> m_RandomDuration;

        [Tooltip("The seed of the random number generator. Set to 0 to disable.")] [SerializeField]
        int m_Seed;

        [Tooltip("The minimum wait duration if random wait is enabled.")] [SerializeField]
        SharedVariable<RangeFloat> m_RandomDurationRange = new RangeFloat(1, 1);

        [Tooltip("The maximum wait duration if random wait is enabled.")] [SerializeField]
        SharedVariable<float> m_RandomDurationMax = 1;

        public SharedVariable<float> Duration
        {
            get => m_Duration;
            set => m_Duration = value;
        }

        public SharedVariable<bool> RandomDuration
        {
            get => m_RandomDuration;
            set => m_RandomDuration = value;
        }

        public int Seed
        {
            get => m_Seed;
            set => m_Seed = value;
        }

        public SharedVariable<RangeFloat> RandomDurationRange
        {
            get => m_RandomDurationRange;
            set => m_RandomDurationRange = value;
        }

        private float m_WaitDuration;
        private float m_StartTime;

        /// <summary>
        /// Callback when the task is initialized.
        /// </summary>
        public override void OnAwake()
        {
            if (m_Seed != 0)
            {
                Random.InitState(m_Seed);
            }
        }

        /// <summary>
        /// Callback when the task is started.
        /// </summary>
        public override void OnStart()
        {
            if (m_RandomDuration.Value)
            {
                m_WaitDuration =
                    Random.Range(m_RandomDurationRange.Value.Min, m_RandomDurationRange.Value.Max);
            }
            else
            {
                m_WaitDuration = m_Duration.Value;
            }

            m_StartTime = Time.time;
        }

        /// <summary>
        /// Executes the task logic.
        /// </summary>
        /// <returns>The status of the task.</returns>
        public override TaskStatus OnUpdate()
        {
            return m_StartTime + m_WaitDuration <= Time.time ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}