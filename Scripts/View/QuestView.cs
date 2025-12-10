using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class QuestView : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent titleText;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private LocalizeStringEvent description;
    [SerializeField] private Image Background;
    [SerializeField] private IconQuestView iconQuestView;

    private Quest currentQuest;

    public void SetQuest(Quest quest)
    {
        currentQuest = quest;
        iconQuestView.setQuest(quest);
        UpdateView();
    }

    public void UpdateView()
    {
        if (currentQuest == null || currentQuest.template == null) return;

        var template = currentQuest.template;

        if (Background != null && template.background != null)
        {
            Background.sprite = template.background;
        }

        if (titleText != null && template.questTitle != null)
        {
            titleText.StringReference = new LocalizedString()
            {
                TableReference = Constants.TRANSLATION_QUESTS_COLLECTION_NAME,
                TableEntryReference = template.questTitle
            };
        }
        
        if (description != null && template.questDescription != null)
        {
            description.StringReference = new LocalizedString()
            {
                TableReference = Constants.TRANSLATION_QUESTS_COLLECTION_NAME,
                TableEntryReference = template.questDescription
            };
        }
    }
}