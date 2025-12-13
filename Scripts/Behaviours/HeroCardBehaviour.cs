using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardBehaviour : MonoBehaviour
{
    private Hero hero;
    
    [SerializeField] private StatsView statsView;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI heroName;
    [SerializeField] private GameObject openedView;
    [SerializeField] private GameObject closedView;
    [SerializeField] private GameObject canvasView;
    
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
        statsView.UpdateView(hero.GetStats());
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
