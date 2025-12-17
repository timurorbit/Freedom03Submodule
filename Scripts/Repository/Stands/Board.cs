using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Board : Table
{
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    [SerializeField] private MMF_Player pinningFeedback;
    
    [Header("Board Position Settings")]
    [SerializeField] private Transform boardReferenceTransform;

    [SerializeField] public List<Transform> stayingTransforms;
    
    [Header("Board Boundaries")]
    [SerializeField] private float minX = -0.5f;
    [SerializeField] private float maxX = 0.5f;
    [SerializeField] private float minY = -0.5f;
    [SerializeField] private float maxY = 0.5f;
    
    private List<Vector2> previousPositions = new List<Vector2>();
    public float minSeparationDistance = 5f;  // Adjust this value based on your board scale (e.g., units in world space)
    private const int maxRegenerationAttempts = 10;  // Prevent infinite loops if the board is too crowded
    
    private List<QuestResultBehaviour> questsToTake;
    private Sequence currentSequence;

    protected override void Awake()
    {
        base.Awake();
    }

    public Board()
    {
        questsToTake = new List<QuestResultBehaviour>();
    }

    public Board(List<QuestResultBehaviour> questsToTake)
    {
        this.questsToTake = questsToTake;
    }

    public void addQuestToBoard(QuestResultBehaviour quest)
    {
        quest.SwitchState(QuestResultState.Predicted);
        questsToTake.Add(quest);
    }

    public QuestResultBehaviour getQuestByStats(Stats stats)
    {
        getQuestByHeroStats();
        return getFirstQuestFromBoard();
    }

    private void getQuestByHeroStats()
    {
        
    }

    public QuestResultBehaviour getFirstQuestFromBoard()
    {
        var toTake = questsToTake[0];
        RemovePosition(new Vector2(toTake.transform.localPosition.x, toTake.transform.localPosition.y));
        toTake.SwitchState(QuestResultState.Taken);
        questsToTake.RemoveAt(0);
        return toTake;
    }

    public override void Interact()
    {
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        if (!playerController.IsItemQuest())
        {
            return;
        }
        pinningFeedback?.PlayFeedbacks();
        var item = playerController.GetFromInventory();
        AddItemToBoard(item);
        base.Interact();
    }

    public void AddItemToBoard(GameObject item)
    {
        item.transform.SetParent(transform);
        addQuestToBoard(item.GetComponent<QuestResultBehaviour>());
        TweenToBoardPosition(item.transform);
    }

    private void TweenToBoardPosition(Transform objectTransform)
    {
        if (objectTransform == null)
        {
            Debug.LogWarning("Board: Attempted to tween null transform");
            return;
        }

        // Kill any active sequence before creating a new one
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        // Generate random X and Y positions within boundaries
        Vector2 newPos  = GetSeparatedRandomPosition();
        
        // Use Z position and rotation from reference transform if available
        float targetZ = Random.Range(0f, -0.1f);
        Vector3 targetRotation = Vector3.zero;
        
        if (boardReferenceTransform != null)
        {
            targetZ = boardReferenceTransform.localPosition.z;
            targetRotation = boardReferenceTransform.localEulerAngles;
        }
        
        Vector3 targetPosition = new Vector3(newPos.x, newPos.y, targetZ);
        
        // Create tween sequence
        currentSequence = DOTween.Sequence();
        currentSequence.Append(objectTransform.DOLocalMove(targetPosition, tweenDuration).SetEase(tweenEase));
        currentSequence.Join(objectTransform.DOLocalRotate(targetRotation, tweenDuration).SetEase(tweenEase));
        currentSequence.SetAutoKill(true);
    }
    
    private Vector2 GetSeparatedRandomPosition()
    {
        // Generate random X and Y positions within boundaries, ensuring separation from previous positions
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector2 newPos = new Vector2(randomX, randomY);

        bool tooClose = true;
        int attempts = 0;

        while (tooClose && attempts < maxRegenerationAttempts)
        {
            tooClose = false;
            foreach (var prevPos in previousPositions)
            {
                if (Vector2.Distance(newPos, prevPos) < minSeparationDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
            {
                randomX = Random.Range(minX, maxX);
                randomY = Random.Range(minY, maxY);
                newPos = new Vector2(randomX, randomY);
                attempts++;
            }
        }

        if (attempts >= maxRegenerationAttempts)
        {
            Debug.LogWarning("Board: Could not find a separated position after max attempts. Using potentially close position.");
        }
        previousPositions.Add(newPos);

        return newPos;
    }
    
    private void RemovePosition(Vector2 position)
    {
        const float tolerance = 0.0001f; // Adjust if needed for your precision

        for (int i = 0; i < previousPositions.Count; i++)
        {
            if (Vector2.Distance(previousPositions[i], position) < tolerance)
            {
                previousPositions.RemoveAt(i);
                return;
            }
        }

        Debug.LogWarning("Board: Position not found in history.");
    }
    
    private void OnDestroy()
    {
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }
    }
}