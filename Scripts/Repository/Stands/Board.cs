using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

class Board : Table
{
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    
    [Header("Board Position Settings")]
    [SerializeField] private Transform boardReferenceTransform;
    
    [Header("Board Boundaries")]
    [SerializeField] private float minX = -0.5f;
    [SerializeField] private float maxX = 0.5f;
    [SerializeField] private float minY = -0.5f;
    [SerializeField] private float maxY = 0.5f;
    
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
        questsToTake.Add(quest);
    }

    public QuestResultBehaviour getQuestByStats(Stats stats)
    {
        //TODO implement better logic 
        return getFirstQuestFromBoard();
    }

    public QuestResultBehaviour getFirstQuestFromBoard()
    {
        var toTake = questsToTake[0];
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
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        
        // Use Z position and rotation from reference transform if available
        float targetZ = 0f;
        Vector3 targetRotation = Vector3.zero;
        
        if (boardReferenceTransform != null)
        {
            targetZ = boardReferenceTransform.localPosition.z;
            targetRotation = boardReferenceTransform.localEulerAngles;
        }
        
        Vector3 targetPosition = new Vector3(randomX, randomY, targetZ);
        
        // Create tween sequence
        currentSequence = DOTween.Sequence();
        currentSequence.Append(objectTransform.DOLocalMove(targetPosition, tweenDuration).SetEase(tweenEase));
        currentSequence.Join(objectTransform.DOLocalRotate(targetRotation, tweenDuration).SetEase(tweenEase));
        currentSequence.SetAutoKill(true);
    }
    
    private void OnDestroy()
    {
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }
    }
}