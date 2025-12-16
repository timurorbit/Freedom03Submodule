using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Behaviours
{
    public class MainTableCanvas : MonoBehaviour
    {
        [SerializeField]
        private MainTable _mainTable;
        
        [SerializeField]
        private Button sendButton;

        
        [SerializeField] private Button rejectButton;
        
        [Header("Feedbacks")]
        [SerializeField] private MMF_Player approveFeedback;
        [SerializeField] private MMF_Player rejectFeedback;

        public void UpdateView()
        {
            //TODO: update currentCard in main table
        }


        private void SetHeroApproval(bool approved)
        {
            // Feedback to approve/reject button press
            if (_mainTable.currentHeroBehaviour == null)
            {
                Debug.LogWarning("No hero selected");
                return;
            }
            var behaviour = _mainTable.currentHeroBehaviour;
            behaviour.Approved = approved;
            if (approved)
            {
                approveFeedback?.PlayFeedbacks();
            }
            else
            {
                rejectFeedback?.PlayFeedbacks();
            }
            _mainTable.Clear();
            behaviour.GetComponent<CharacterBehaviour>().Interact();
        }

        public void SendHeroToMission()
        {
            SetHeroApproval(true);
        }

        public void RejectHero()
        {
            SetHeroApproval(false);
        }
    }
}