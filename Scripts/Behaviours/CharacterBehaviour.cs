using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Behaviours
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Animator animator;
        
        [SerializeField] NavMeshAgent agent;
        
        [Header("Locomotion")]
        [SerializeField] private float runSpeed = 1.5f;
        Vector2Int AgentPriorityRange = new(0, 99);
        public CharacterState characterState;
        private float speedVelocity;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
        }
        
        private void Start()
        {
            agent.avoidancePriority = Random.Range(AgentPriorityRange.x, AgentPriorityRange.y + 1);
        }

        public void Update()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            switch (characterState)
            {
                case CharacterState.Idle:
                    animator.SetFloat("Speed", 0, 0.15f, Time.deltaTime);
                    break;
                case CharacterState.MovingToTarget:
                    animator.SetFloat("Speed", 1, 0.15f, Time.deltaTime);
                    agent.speed = Mathf.SmoothDamp(
                        agent.speed,
                        runSpeed,
                        ref speedVelocity,
                        0.15f
                    );
                    break;
            }
        }

        public GameObject FindClosestEntrance()
        {
           var entrance = FindAnyObjectByType<Entrance>().gameObject;
           return entrance;
        }

        public GameObject GetClosestFreeQuestTableSpot()
        {
            return GuildRepository.Instance.GetClosestFreeQuestTableSpot();
        }

        public GameObject GetClosestFreeWelcomeTableSpot()
        {
            return GuildRepository.Instance.GetClosestFreeWelcomeTableSpot();
        }

        public GameObject GetClosestFreeResultTableSpot()
        {
            return GuildRepository.Instance.GetClosestFreeResultTableSpot();
        }

        public void SetRun()
        {
            agent.speed = runSpeed;
            characterState = CharacterState.MovingToTarget;
        }

        public void SetIdle()
        {
            characterState = CharacterState.Idle;
        }

        public void OccupySpot(GameObject spot)
        {
            var line = spot.GetComponentInParent<Line>();
            if (line != null)
            {
                int index = line.GetSpotIndex(spot);
                if (index >= 0)
                {
                    line.OccupySpot(index, this.gameObject);
                }
            }
        }

        public void ReleaseSpot(GameObject spot)
        {
            var line = spot.GetComponentInParent<Line>();
            if (line != null)
            {
                int index = line.GetSpotIndex(spot);
                if (index >= 0)
                {
                    line.ReleaseSpot(index);
                }
            }
        }
        
    }

    public enum CharacterState
    {
        Idle,
        MovingToTarget,
    }
}