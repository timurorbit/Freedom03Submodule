using _Game.Scripts.Behaviours;
using UnityEngine;

public class QuestTakingTable : Table
{
    public QuestGiverBehaviour currentQuestGiver;


    public void TakingQuestInteraction()
    {
        GuildPlayerController.Instance.SwitchState(GuildPlayerState.QuestTaking);
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