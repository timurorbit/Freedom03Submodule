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
    [SerializeField] private Transform rotationReference;
    
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
        AddItemToBoard(item);
        base.Interact();
    }

    public void AddItemToBoard(GameObject item)
    {
        item.transform.SetParent(transform);
        addQuestToBoard(item.GetComponent<QuestResultBehaviour>());
        TweenToRandomPosition(item.transform);
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
        

        // Ensure min/max are in correct order
        float minPosX = Mathf.Min(minX.localPosition.x, maxX.localPosition.x);
        float maxPosX = Mathf.Max(minX.localPosition.x, maxX.localPosition.x);
        float minPosY = Mathf.Min(minY.localPosition.y, maxY.localPosition.y);
        float maxPosY = Mathf.Max(minY.localPosition.y, maxY.localPosition.y);
        
        float randomX = Random.Range(minPosX, maxPosX);
        float randomY = Random.Range(minPosY, maxPosY);
        Vector3 targetPosition = new Vector3(randomX, randomY, rotationReference != null ? rotationReference.localPosition.z : 0f);
        
        // Use rotation reference if assigned, otherwise keep current rotation
        Vector3 targetRotation = rotationReference != null ? rotationReference.localEulerAngles : objectTransform.localEulerAngles;
        
        // Create tween sequence in local space
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