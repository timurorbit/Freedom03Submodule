using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestResultBehaviour : MonoBehaviour
{
    private QuestResult result;
    
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image backArt;
    [SerializeField] private TextMeshProUGUI goldRewardText;

    [SerializeField] private GameObject openedView;
    [SerializeField] private GameObject closedView;
    [SerializeField] private GameObject actualStatsView;
    [SerializeField] private Canvas canvasView;

    [FormerlySerializedAs("statsView")] [SerializeField] private QuestPredictionView questPredictionView; 

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

    public GameObject getActualStatsView()
    {
        return actualStatsView;
    }
    
    public int getReward()
    {
        return result.GetQuest().template.stats.reward;
    }

    public void UpdateCanvasView()
    {
        var questTemplate = result.GetQuest().template;
        titleText.text = questTemplate.questTitle;
        description.text = questTemplate.questDescription;
        backArt.sprite = questTemplate.background;
        goldRewardText.text = questTemplate.stats.reward.ToString();
        if (result.GetPrediction() != null)
        {
            questPredictionView.UpdateView(result.GetPrediction());
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
            case QuestResultState.Declined:
                setActiveClosedView();
                break;
            case QuestResultState.Predicted:
                setActiveOpenedView();
                break;
            case QuestResultState.Completed:
                setActiveOpenedView();
                setActiveActualStats();
                break;
        }
    }

    private void setActiveActualStats()
    {
        actualStatsView.GetComponent<ActualStatsBehaviour>().UpdateView(result.GetQuest());
        actualStatsView.SetActive(true);
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