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

        public void UpdateView()
        {
            //TODO: update currentCard in main table
        }


        private void SetHeroApproval(bool approved)
        {
            if (_mainTable.currentHeroBehaviour == null)
            {
                Debug.LogWarning("No hero selected");
                return;
            }
            var behaviour = _mainTable.currentHeroBehaviour;
            behaviour.Approved = approved;
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