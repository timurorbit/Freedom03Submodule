using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Game.Scripts.Behaviours
{
    public class PeasantBehaviour : MonoBehaviour
    {
        [SerializeField]
        public QuestResultBehaviour questResultBehaviour;
        
        [SerializeField]
        public MMF_Player questPutFeedback;
        
        public void PutQuestResultInQuestPile()
        {
            questPutFeedback?.PlayFeedbacks();
            var questTable = GuildRepository.Instance.GetClosestQuestTable();
            if (questTable != null)
            {
                questTable.AddToQuests(questResultBehaviour);
            }
        }
    }
}