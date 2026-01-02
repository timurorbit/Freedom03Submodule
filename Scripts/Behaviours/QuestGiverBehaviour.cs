using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Behaviours
{
    public class QuestGiverBehaviour : CharacterBehaviour
    {
        
        [Header("Quest Giver")]
        [SerializeField]
        public QuestResultBehaviour questResultBehaviour;
        
        [SerializeField]
        public MMF_Player questPutFeedback;
        
        private QuestTakingTable _questTakingTable;
        
        public bool questIsGiven = false;
        
        public void PutQuestResultInQuestPile()
        {
            questPutFeedback?.PlayFeedbacks();
            var questTable = GuildRepository.Instance.GetClosestQuestTable();
            if (questTable != null)
            {
                questTable.AddToQuests(questResultBehaviour);
            }
        }

        public override void SetCanInteract(bool value)
        {
            _questTakingTable = GuildRepository.Instance.GetClosestQuestTakingTable();
            _questTakingTable.currentQuestGiver = this;
            base.SetCanInteract(value);
        }

        public override void Interact()
        {
            if (!questIsGiven)
            {
                GuildRepository.Instance.GetClosestQuestTakingTable().TakingQuestInteraction();
                SetCharacterState(CharacterState.Interacting);
            }
            else
            {
                base.Interact(); 
            }
        }
    }
}