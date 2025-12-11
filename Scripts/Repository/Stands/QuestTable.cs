using DG.Tweening;
using UnityEngine;

public class QuestTable : Table
{
    [SerializeField]
    private PileQuests pileQuests;
    
    [SerializeField]
    private PileResults pileResults;

    [Header("Quest Stacking Settings")]
    [SerializeField]
    private float stackOffsetZ = -0.075f;
    
    [SerializeField]
    private float rotationOffsetMin = -25f;
    
    [SerializeField]
    private float rotationOffsetMax = 25f;
    
    [SerializeField]
    private float tweenDuration = 0.5f;
    
    [SerializeField]
    private Ease tweenEase = Ease.OutQuad;

    protected override void Awake()
    {
        base.Awake();
        if (pileQuests == null)
        {
            pileQuests = GetComponentInChildren<PileQuests>();
        }
        if (pileResults == null)
        {
            pileResults = GetComponentInChildren<PileResults>();
        }
    }

    public void AddToQuests(QuestResultBehaviour quest)
    {
        if (pileQuests != null && quest != null)
        {
            // Change the state of questResult to Opened
            quest.SwitchState(QuestResultState.Opened);
            
            // Parent the quest to PileQuests GameObject
            quest.transform.SetParent(pileQuests.transform);
            
            // Add to the pile
            pileQuests.Add(quest);
            
            // Calculate new position (Z offset based on pile count)
            int pileCount = pileQuests.Count;
            Vector3 targetPosition = new Vector3(0, 0, stackOffsetZ * pileCount);
            
            // Calculate new rotation (PileQuests rotation + random offset)
            float randomRotationOffset = Random.Range(rotationOffsetMin, rotationOffsetMax);
            Vector3 targetRotation = new Vector3(
                pileQuests.transform.localEulerAngles.x,
                pileQuests.transform.localEulerAngles.y,
                pileQuests.transform.localEulerAngles.z + randomRotationOffset
            );
            
            // Apply smooth tweening for position and rotation
            TweenQuestToPosition(quest.transform, targetPosition, targetRotation);
        }

        if (pileQuests != null && pileQuests.Count > 0)
        {
            _canInteract = true;
        }
    }
    
    private void TweenQuestToPosition(Transform questTransform, Vector3 targetLocalPosition, Vector3 targetLocalRotation)
    {
        questTransform.DOLocalMove(targetLocalPosition, tweenDuration).SetEase(tweenEase);
        questTransform.DOLocalRotate(targetLocalRotation, tweenDuration).SetEase(tweenEase);
    }

    public QuestResult TakeFromResult()
    {
        if (pileResults != null)
        {
            return pileResults.Take();
        }
        return null;
    }

    public void inspectQuest(Stats prediction)
    {
        if (pileQuests != null && pileResults != null)
        {
            var quest = pileQuests.Take();
            if (pileQuests.Count <= 0)
            {
                _canInteract = false;
            }
            var result = new QuestResult(quest.getQuestResult().GetQuest(), prediction);
            pileResults.Add(result);
        }
    }

    public override void Interact()
    {
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        playerController.SetActiveQuestCamera(true);
        base.Interact();
    }
}