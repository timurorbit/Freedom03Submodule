using _Game.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActualStatsBehaviour : MonoBehaviour
{
    [SerializeField] private UI_StatsRadarChart chart;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image background;

    public UI_StatsRadarChart GetChart()
    {
        return chart;
    }

    public void UpdateView(Quest quest)
    {
        chart.setStats(quest.template.stats, true);
        title.text = quest.template.questTitle;
        background.sprite = quest.template.background;
    }
}
