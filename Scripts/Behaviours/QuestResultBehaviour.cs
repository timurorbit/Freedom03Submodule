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
    
    [SerializeField] private StatsView statsView; 

    public void setQuest(Quest quest)
    {
        result = new QuestResult(quest);
        UpdateCanvasView();
    }
    
    public void setPrediction(Stats prediction)
    {
        result.setPrediction(prediction);
        UpdateCanvasView();
    }

    public void UpdateCanvasView()
    {
        var questTemplate = result.GetQuest().template;
        titleText.text = questTemplate.questTitle;
        description.text = questTemplate.questDescription;
        backArt.sprite = questTemplate.background;
        if (result.GetPrediction() != null)
        {
            statsView.UpdateView(result.GetPrediction());
        }
    }
    
    public QuestResult getQuestResult()
    {
        return result;
    }

    public void SwitchState(QuestResultState state)
    {
        result.state = state;
        SwitchViewState(state);
    }

    private void SwitchViewState(QuestResultState state)
    {
        switch (state)
        {
            case QuestResultState.Closed :
                setActiveClosedView();
                break;
            case QuestResultState.Opened:
                setActiveOpenedView();
                break;
            case QuestResultState.Taken:
                setActiveClosedView();
                break;
            case QuestResultState.Assigned:
                setActiveClosedView();
                break;
        }
    }

    private void setActiveClosedView()
    {
        closedView.SetActive(true);
        openedView.SetActive(false);
        canvasView.gameObject.SetActive(false);
    }

    private void setActiveOpenedView()
    {
        closedView.SetActive(false);
        openedView.SetActive(true);
        canvasView.gameObject.SetActive(true);
    }
}