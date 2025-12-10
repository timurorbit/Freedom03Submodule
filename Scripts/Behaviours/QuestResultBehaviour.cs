using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestResultBehaviour : MonoBehaviour
{
    private QuestResult result;
    
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image backArt;

    [SerializeField] private GameObject openedView;
    [SerializeField] private GameObject closedView;
    [SerializeField] private Canvas canvasView;

    public void setQuest(Quest quest)
    {
        result = new QuestResult(quest);
        UpdateCanvasView();
    }

    private void UpdateCanvasView()
    {
        var questTemplate = result.GetQuest().template;
        titleText.text = questTemplate.questTitle;
        description.text = questTemplate.questDescription;
        backArt.sprite = questTemplate.background;
    }
    
    public QuestResult getQuestResult()
    {
        return result;
    }

    public void SwitchView(QuestResultState state)
    {
        switch (state)
        {
            case QuestResultState.Closed :
                setActiveClosedView();
                break;
            case QuestResultState.Opened:
                setActiveOpenedView();
                break;
            
        }
    }

    private void setActiveClosedView()
    {
        closedView.SetActive(true);
    }

    private void setActiveOpenedView()
    {
        closedView.SetActive(false);
        openedView.SetActive(true);
        canvasView.gameObject.SetActive(true);
    }
}
