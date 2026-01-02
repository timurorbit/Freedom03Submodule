using UnityEngine;

public class QuestTakingCanvas : MonoBehaviour
{
    [SerializeField]
    private QuestTakingTable questTakingTable;
    
    
    public void AcceptQuest()
    {
        questTakingTable.FinishTakingQuest();
    }
}