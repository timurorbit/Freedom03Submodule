using _Game.Scripts.Behaviours;
using DG.Tweening;
using UnityEngine;

public class QuestTakingTable : Table
{
    public QuestGiverBehaviour currentQuestGiver;
    
    [Header("Quest Result Interaction")]
    [SerializeField] private Transform questResultInteractionTransform;
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Ease tweenEase = Ease.OutQuad;


    public void TakingQuestInteraction()
    {
        GuildPlayerController.Instance.SwitchState(GuildPlayerState.QuestTaking);
        
        if (currentQuestGiver != null && currentQuestGiver.questResultBehaviour != null && questResultInteractionTransform != null)
        {
            currentQuestGiver.questResultBehaviour.transform.DOMove(questResultInteractionTransform.position, tweenDuration).SetEase(tweenEase);
            currentQuestGiver.questResultBehaviour.transform.DORotate(questResultInteractionTransform.eulerAngles, tweenDuration).SetEase(tweenEase);
        }
    }


    public void FinishTakingQuest()
    {
        if (currentQuestGiver == null)
        {
            Debug.LogWarning("QuestTakingTable: No current quest giver to take quest from.");
            return;
        }
        currentQuestGiver.questIsGiven = true;
        currentQuestGiver.Interact();
        currentQuestGiver = null;
    }
}