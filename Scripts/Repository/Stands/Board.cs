using System.Collections.Generic;
using System.Linq;
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
        return getQuestByHeroStats(stats);
    }

    private QuestResultBehaviour getQuestByHeroStats(Stats stats)
    {
        if (questsToTake == null || questsToTake.Count == 0)
        {
            return null;
        }
        
        // Get hero's two strongest stats
        var strongestStats = GetTwoStrongestStats(stats);
        
        // Start with exact rank match
        Rank heroRank = stats.rank;
        int rankOffset = 0;
        int minRank = System.Enum.GetValues(typeof(Rank)).Cast<int>().Min();
        int maxRank = System.Enum.GetValues(typeof(Rank)).Cast<int>().Max();
        
        // Try to find quest with matching rank and strongest stats
        while (true)
        {
            // Try current rank + offset
            int upperRank = (int)heroRank - rankOffset;
            if (upperRank >= minRank && upperRank <= maxRank)
            {
                QuestResultBehaviour quest = FindQuestByRankAndStats((Rank)upperRank, strongestStats, rankOffset == 0);
                if (quest != null)
                {
                    return TakeQuestFromBoard(quest);
                }
            }
            
            // Try current rank - offset (only if offset > 0 to avoid checking same rank twice)
            if (rankOffset > 0)
            {
                int lowerRank = (int)heroRank + rankOffset;
                if (lowerRank >= minRank && lowerRank <= maxRank)
                {
                    QuestResultBehaviour quest = FindQuestByRankAndStats((Rank)lowerRank, strongestStats, false);
                    if (quest != null)
                    {
                        return TakeQuestFromBoard(quest);
                    }
                }
            }
            
            // Check if we've exhausted all possible ranks
            int nextUpperRank = (int)heroRank - (rankOffset + 1);
            int nextLowerRank = (int)heroRank + (rankOffset + 1);
            
            if (nextUpperRank < minRank && nextLowerRank > maxRank)
            {
                break; // We've checked all possible ranks
            }
            
            rankOffset++;
        }
        
        // Fallback to first quest from board
        return getFirstQuestFromBoard();
    }
    
    private SkillType[] GetTwoStrongestStats(Stats stats)
    {
        SkillType[] allStats = (SkillType[])System.Enum.GetValues(typeof(SkillType));
        
        if (allStats.Length < 2)
        {
            Debug.LogWarning("Board: SkillType enum has fewer than 2 values. Cannot determine two strongest stats.");
            return allStats;
        }
        
        // Sort stats by amount in descending order
        System.Array.Sort(allStats, (a, b) => stats.GetStatAmount(b).CompareTo(stats.GetStatAmount(a)));
        
        // Return top 2
        return new SkillType[] { allStats[0], allStats[1] };
    }
    
    private QuestResultBehaviour FindQuestByRankAndStats(Rank rank, SkillType[] strongestStats, bool requireStrongStats)
    {
        foreach (var quest in questsToTake)
        {
            var prediction = quest.getQuestResult().GetPrediction();
            if (prediction != null && prediction.rank == rank)
            {
                if (requireStrongStats)
                {
                    // Check if prediction has value 1 in any of the strongest stats
                    if (HasValueOneInStats(prediction, strongestStats))
                    {
                        return quest;
                    }
                }
                else
                {
                    // For expanded search, just return quest with matching rank
                    return quest;
                }
            }
        }
        return null;
    }
    
    private bool HasValueOneInStats(Stats prediction, SkillType[] stats)
    {
        foreach (var stat in stats)
        {
            if (prediction.GetStatAmount(stat) == 1)
            {
                return true;
            }
        }
        return false;
    }
    
    private QuestResultBehaviour TakeQuestFromBoard(QuestResultBehaviour quest)
    {
        RemovePosition(new Vector2(quest.transform.localPosition.x, quest.transform.localPosition.y));
        quest.SwitchState(QuestResultState.Taken);
        questsToTake.Remove(quest);
        return quest;
    }

    public QuestResultBehaviour getFirstQuestFromBoard()
    {
        if (questsToTake == null || questsToTake.Count == 0)
        {
            return null;
        }
        return TakeQuestFromBoard(questsToTake[0]);
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