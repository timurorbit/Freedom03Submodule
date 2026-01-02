using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Behaviours
{
    public class CharacterBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField]
        public Transform modelTransform;

        [Header("Components")] [SerializeField]
        private Animator animator;

        [SerializeField] NavMeshAgent agent;

        [Header("Locomotion")]
        [SerializeField] private float runSpeed = 1.5f;

        Vector2Int AgentPriorityRange = new(0, 99);

        public CharacterState characterState;

        private float speedVelocity;

        [Header("Idle Animation")]
        [SerializeField] private float minIdleTimeBeforeAnimation = 3f;

        [SerializeField] private float maxIdleTimeBeforeAnimation = 8f;

        private float idleTimer = 0f;

        private float nextIdleAnimationTime;

        [SerializeField]
        private Outline outline;

        private bool _canInteract = false;
        public bool canInteract => _canInteract;
        
        [Header("Only for testing purposes, keep null for prefab")] [SerializeField]
        public CharacterObject baseCharacter;

        private void Awake()
        {
            if (baseCharacter != null)
            {
                Initialize(baseCharacter);
            }
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }

            if (outline == null)
            {
                outline = GetComponent<Outline>();
            }
        }

        public void Initialize(CharacterObject characterObject)
        {
            Instantiate(characterObject.characterPrefab, modelTransform);
            animator = GetComponentInChildren<Animator>();
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }

            if (outline == null)
            {
                outline = GetComponent<Outline>();
            }

            outline.RefreshOutlines();
        }
        
        private void Start()
        {
            agent.avoidancePriority = Random.Range(AgentPriorityRange.x, AgentPriorityRange.y + 1);
            ResetIdleTimer();
        }

        public void Update()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            switch (characterState)
            {
                case CharacterState.Created:
                    break;
                case CharacterState.Idle:
                    animator.SetFloat(Constants.ANIMATOR_SPEED, 0, 0.15f, Time.deltaTime);
                    UpdateIdleAnimation();
                    break;
                case CharacterState.Interactable:  
                    animator.SetFloat(Constants.ANIMATOR_SPEED, 0, 0.15f, Time.deltaTime);
                    UpdateInteractableAnimation();
                    RotateTowardsPlayerCamera();
                    break;
                case CharacterState.MovingToTarget:
                    animator.SetFloat(Constants.ANIMATOR_SPEED, 1, 0.15f, Time.deltaTime);
                    agent.speed = Mathf.SmoothDamp(
                        agent.speed,
                        runSpeed,
                        ref speedVelocity,
                        0.15f
                    );
                    ResetIdleTimer();
                    break;
            }
        }

        private void UpdateInteractableAnimation()
        {
            UpdateIdleAnimation();
        }

        private void RotateTowardsPlayerCamera()
        {
            
        }

        public GameObject FindClosestEntrance()
        {
           var entrance = FindObjectsByType<Entrance>(FindObjectsSortMode.None);
           return entrance[Random.Range(0, entrance.Length)].gameObject;
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

        public GameObject GetClosestFreeMainTableSpot()
        {
            return GuildRepository.Instance.GetClosestFreeMainTableSpot();
        }

        public QuestTable GetClosestQuestTable()
        {
            return GuildRepository.Instance.GetClosestQuestTable();
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

        public virtual void Interact()
        {
            Debug.Log($"Interacting with character: {gameObject.name}");
            characterState = CharacterState.Interacted;
            _canInteract = false;
        }

        public virtual void SetCanInteract(bool value)
        {
            var component = GetComponent<HeroBehaviour>();
            if (component != null)
            {
                component.HandleHeroInteraction();
                return;
            }
            _canInteract = value;
        }

        private void UpdateIdleAnimation()
        {
            idleTimer += Time.deltaTime;
            
            if (idleTimer >= nextIdleAnimationTime)
            {
                TriggerRandomIdleAnimation();
                ResetIdleTimer();
            }
        }

        private void TriggerRandomIdleAnimation()
        {
            if (animator == null) return;
            
            int randomTrigger = Random.Range(0, 2);
            if (randomTrigger == 0)
            {
                animator.SetTrigger(Constants.ANIMATOR_IDLE_TRIGGER_1);
            }
            else
            {
                animator.SetTrigger(Constants.ANIMATOR_IDLE_TRIGGER_2);
            }
        }

        private void ResetIdleTimer()
        {
            idleTimer = 0f;
            nextIdleAnimationTime = Random.Range(minIdleTimeBeforeAnimation, maxIdleTimeBeforeAnimation);
        }
        
    }

    [System.Serializable]
    public enum CharacterState
    {
        Created,
        Idle,
        MovingToTarget,
        Interacted,
        SentToMission,
        Reading,
        Interactable,
    }
}