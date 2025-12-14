using _Game.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HeroCardBehaviour : MonoBehaviour
{
    private Hero hero;
    
    [FormerlySerializedAs("statsView")] [SerializeField] private QuestPredictionView questPredictionView;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI heroName;
    [SerializeField] private GameObject openedView;
    [SerializeField] private GameObject closedView;
    [SerializeField] private GameObject canvasView;
    [SerializeField] private UI_StatsRadarChart chart;
    
    public UI_StatsRadarChart GetChart()
    {
        return chart;
    }
    
    public void SetHero(Hero hero)
    {
        this.hero = hero;
    }
    
    public Hero GetHero()
    {
        return hero;
    }
    
    public void UpdateCanvasView()
    {
        portrait.sprite = hero.GetTemplate().portrait;
        heroName.text = hero.GetTemplate().name;
        chart.setStats(hero.GetStats());
        questPredictionView.UpdateViewRankOnly(hero.GetStats());
    }
    
    public void SwitchState(bool opened)
    {
        SwitchViewState(opened);
    }

    private void SwitchViewState(bool opened)
    {
        if (!opened)
        {
            setActiveClosedView();
        }
        else
        {
            setActiveOpenedView();
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
        UpdateCanvasView();
        closedView.SetActive(false);
        openedView.SetActive(true);
        canvasView.gameObject.SetActive(true);
    }
}
