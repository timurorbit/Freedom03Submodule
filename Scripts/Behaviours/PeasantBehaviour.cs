using UnityEngine;

namespace _Game.Scripts.Behaviours
{
    public class PeasantBehaviour : MonoBehaviour
    {
        [SerializeField]
        public QuestResultBehaviour questResultBehaviour;
        
        public QuestTable GetClosestQuestTable()
        {
            return GuildRepository.Instance.GetClosestQuestTable();
        }
        
        public void PutQuestResultInQuestPile()
        {
            questResultBehaviour.SwitchState(QuestResultState.Opened);
            
            var questTable = GetClosestQuestTable();
            if (questTable != null)
            {
                questTable.AddToQuests(questResultBehaviour);
            }
        }
    }
}