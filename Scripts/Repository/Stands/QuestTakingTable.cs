using _Game.Scripts.Behaviours;
using UnityEngine;

public class QuestTakingTable : Table
{
    public QuestGiverBehaviour currentQuestGiver;

    [SerializeField] public Transform questResultInteractionTransform;

    public void TakingQuestInteraction()
    {
        GuildPlayerController.Instance.SwitchState(GuildPlayerState.QuestTaking);
        // currentQuestGiver.questResultBehaviour move by dotween to questResultInteractionTransform position and rotation
        
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