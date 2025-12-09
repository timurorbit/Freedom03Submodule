using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
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
        Vector2Int AgentPriorityRange = new Vector2Int(0, 99);
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

        public void SetRun()
        {
            agent.speed = runSpeed;
            characterState = CharacterState.MovingToTarget;
        }

        public void SetIdle()
        {
            characterState = CharacterState.Idle;
        }
        
    }

    public enum CharacterState
    {
        Idle,
        MovingToTarget,
    }
}