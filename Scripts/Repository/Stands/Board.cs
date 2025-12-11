using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

class Board : Table
{
    [Header("Boundary Transforms")]
    [SerializeField] private Transform maxX;
    [SerializeField] private Transform maxY;
    [SerializeField] private Transform minX;
    [SerializeField] private Transform minY;
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;
    
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
        item.transform.SetParent(transform);
        addQuestToBoard(item.GetComponent<QuestResultBehaviour>());
        TweenToRandomPosition(item.transform);
        base.Interact();
    }
    
    private void TweenToRandomPosition(Transform objectTransform)
    {
        if (maxX == null || maxY == null || minX == null || minY == null)
        {
            Debug.LogWarning("Board: Boundary transforms are not assigned. Cannot tween to random position.");
            return;
        }
        
        // Kill any existing sequence to prevent memory leaks
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }
        
        // Calculate random position between boundaries
        float randomX = Random.Range(minX.position.x, maxX.position.x);
        float randomY = Random.Range(minY.position.y, maxY.position.y);
        Vector3 targetPosition = new Vector3(randomX, randomY, objectTransform.position.z);
        
        // Use maxX rotation as target rotation, keep size unchanged
        Vector3 targetRotation = maxX.eulerAngles;
        
        // Create tween sequence
        currentSequence = DOTween.Sequence();
        currentSequence.Append(objectTransform.DOMove(targetPosition, tweenDuration).SetEase(tweenEase));
        currentSequence.Join(objectTransform.DORotate(targetRotation, tweenDuration).SetEase(tweenEase));
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