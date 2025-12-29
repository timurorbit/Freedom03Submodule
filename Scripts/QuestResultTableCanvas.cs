using System;
using _Game.Scripts.Behaviours;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestResultTableCanvas : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestResultTable _tableRef;
    [SerializeField]
    private TextMeshProUGUI _percentageText;
    [SerializeField] private Button _calculateButton;
    [SerializeField] private GameObject _payWindow;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI reputationText;
    [SerializeField] private UpdateTextValue rewardAmountText;
    [SerializeField] private GameObject reputation;

    
    [Header("Texts")]
    [SerializeField] private Image reputationPlus;
    [SerializeField] private Image reputationMinus;
    [SerializeField] private Material plusMaterial;
    [SerializeField] private Material minusMaterial;
    
    
    
    [Header("Values")]
    [SerializeField] private int rewardAmount;
    [SerializeField] private float rewardPercentMax = 1f;
    [SerializeField] private float rewardPercentMin = .7f;
    
    [Header("Feedbacks")]
    [SerializeField] MMF_Player paymentFeedback;
    

    private void Awake()
    {
        slider.onValueChanged.AddListener(UpdateText);
    }
    
    private void UpdateText(float sliderValue)
    {
        int repValue;
        if (_tableRef.successfulMission)
        {
            repValue = Mathf.RoundToInt(sliderValue * 0.3f);
            reputationText.text = repValue.ToString();
        }
        else
        {
            float normalized = (sliderValue - slider.minValue) / (slider.maxValue - slider.minValue);
            float percent = Mathf.Lerp(0.1f, 0f, normalized);
            repValue = Mathf.RoundToInt(percent * rewardAmount);
            reputationText.text = (-repValue).ToString();
        }
    }
    

    public void NextHero()
    {
        if (_tableRef.currentHeroBehaviour == null)
        {
            Debug.LogWarning("No hero selected");
            return;
        }
        
        paymentFeedback?.PlayFeedbacks();
        var behaviour = _tableRef.currentHeroBehaviour;
        behaviour.GetComponent<CharacterBehaviour>().Interact();
        //Feedback give money
        GuildRepository.Instance.Gold -= Mathf.RoundToInt(slider.value);
        if (_tableRef.successfulMission)
        {
            GuildRepository.Instance.Reputation += Convert.ToInt32(reputationText.text);   
        }
        else
        {
            GuildRepository.Instance.Reputation += Convert.ToInt32(reputationText.text);   
        }
        
        _tableRef.Clear();
        UpdateView(QuestResultCanvasStage.Disabled);
    }

    public void CalculateResult()
    {
        _calculateButton.gameObject.SetActive(false);
        _tableRef.MoveChartComponentsAndCalculateCoverage();
    }
    
    

    public void UpdateView(QuestResultCanvasStage canvasStage)
    {
        switch (canvasStage)
        {
            case QuestResultCanvasStage.StatsShow:
                _percentageText.gameObject.SetActive(false);
                _calculateButton.gameObject.SetActive(true);
                _payWindow.gameObject.SetActive(false);
                break;
            case QuestResultCanvasStage.Calculated:
                _percentageText.gameObject.SetActive(true);
                _calculateButton.gameObject.SetActive(false);
                _payWindow.gameObject.SetActive(true);
                SetRewardSlider();
                break;
            case QuestResultCanvasStage.Disabled:
                _percentageText.gameObject.SetActive(false);
                _calculateButton.gameObject.SetActive(false);
                _payWindow.gameObject.SetActive(false);
                break;
        }
    }

    private void SetRewardSlider()
    {
        if (_tableRef.successfulMission)
        {
            reputationText.fontMaterial = plusMaterial;
            reputationPlus.gameObject.SetActive(true);
            reputationMinus.gameObject.SetActive(false);
            slider.minValue = (int) (rewardAmount * rewardPercentMin);
            slider.maxValue = (int) (rewardAmount * rewardPercentMax);  
        }
        else
        {
            reputationText.fontMaterial = minusMaterial;
            reputationMinus.gameObject.SetActive(true);
            reputationPlus.gameObject.SetActive(false);
            slider.minValue = (int) (rewardAmount * (1 - rewardPercentMax));
            slider.maxValue = (int) (rewardAmount * (1 - rewardPercentMin));  
        }
    }


    public enum QuestResultCanvasStage
    {
        StatsShow,
        Calculated,
        Disabled
    }

    public void UpdatePercentText()
    {
        _percentageText.text = "Success Chance: " + _tableRef.GetMeshCoveragePercentage().ToString("F2") + "%";
    }

    public void setReward(int reward)
    {
        rewardAmount = reward;
    }
}