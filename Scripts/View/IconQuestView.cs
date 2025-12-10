using UnityEngine;
using UnityEngine.UI;

internal class IconQuestView : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private Image IconBG;
    
    private Quest currentQuest;

    public void UpdateView()
    {
        if (Icon != null)
        {
            Icon.sprite = currentQuest.template.icon;
        }
    }

    public void setQuest(Quest quest)
    {
        currentQuest = quest;
        UpdateView();
    }
}