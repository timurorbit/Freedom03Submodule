using UnityEngine;

namespace _Game.Scripts.Behaviours
{
    public class PeasantBehaviour : MonoBehaviour
    {
        [SerializeField]
        public QuestResultBehaviour questResultBehaviour;
        
        public void PutQuestResultInQuestPile()
        {
            var questTable = GuildRepository.Instance.GetClosestQuestTable();
            if (questTable != null)
            {
                questTable.AddToQuests(questResultBehaviour);
            }
        }
    }
}