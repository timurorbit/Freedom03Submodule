using DG.Tweening;
using UnityEngine;

public class QuestResultTable : Table
{
    [Header("Positions")] [SerializeField] private Transform currentQuestResultPosition;
    [SerializeField] private Transform currentHeroCardPosition;
    [SerializeField] private Transform currentActualQuestStatsBehaviourTransform;


    [Header("Items")] [SerializeField] public QuestResultBehaviour currentQuestResultBehaviour;
    [SerializeField] public HeroCardBehaviour currentHeroCardBehaviour;
    [SerializeField] public HeroBehaviour currentHeroBehaviour;
    [SerializeField] public ActualStatsBehaviour currentActualStatsBehaviour;


    [Header("Tween Settings")] [SerializeField]
    private float tweenDuration = 0.5f;


    [SerializeField] private Ease tweenEase = Ease.OutQuad;

    [Header("Coverage")] [SerializeField]
    private float meshCoveragePercentage;

    [SerializeField] public QuestResultTableCanvas questResultTableCanvas;

    public override void Interact()
    {
        var playerController = FindAnyObjectByType<GuildPlayerController>();
        if (playerController != null)
        {
            playerController.SwitchState(GuildPlayerState.QuestResultTable);
        }

        base.Interact();
    }

    public void PlaceHeroCardAndQuestResultAndQuestStats(HeroCardBehaviour heroCard, QuestResultBehaviour questResult, ActualStatsBehaviour actualStats)
    {
        Sequence sequence = DOTween.Sequence();
        bool heroCardAnimated = false;
        
        if (heroCard != null && currentHeroCardPosition != null)
        {
            currentHeroCardBehaviour = heroCard;
            heroCard.transform.SetParent(transform);
            heroCard.SwitchState(true);
            
            sequence.Append(heroCard.transform.DOMove(currentHeroCardPosition.position, tweenDuration).SetEase(tweenEase));
            sequence.Join(heroCard.transform.DORotate(currentHeroCardPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
            heroCardAnimated = true;
        }
        if (questResult != null && currentQuestResultPosition != null)
        {
            currentQuestResultBehaviour = questResult;
            questResult.transform.SetParent(transform);
            questResult.SwitchState(QuestResultState.Completed);
            
            if (heroCardAnimated)
            {
                sequence.Join(questResult.transform.DOMove(currentQuestResultPosition.position, tweenDuration).SetEase(tweenEase));
                sequence.Join(questResult.transform.DORotate(currentQuestResultPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
            }
            else
            {
                sequence.Append(questResult.transform.DOMove(currentQuestResultPosition.position, tweenDuration).SetEase(tweenEase));
                sequence.Join(questResult.transform.DORotate(currentQuestResultPosition.eulerAngles, tweenDuration).SetEase(tweenEase));
            }
        }

        if (actualStats != null)
        {
            currentActualStatsBehaviour = actualStats;
            actualStats.transform.SetParent(transform);
            sequence.Append(actualStats.transform.DOMove(currentActualQuestStatsBehaviourTransform.position, tweenDuration).SetEase(tweenEase));
            sequence.Join(actualStats.transform.DORotate(currentActualQuestStatsBehaviourTransform.eulerAngles, tweenDuration).SetEase(tweenEase));
        }
        
        questResultTableCanvas.UpdateView(QuestResultTableCanvas.QuestResultCanvasStage.StatsShow);
        sequence.SetAutoKill(true);
    }

    public void Clear()
    {
        currentHeroBehaviour = null;
    }

    public void MoveChartComponentsAndCalculateCoverage()
    {
        if (currentActualStatsBehaviour == null)
        {
            Debug.LogWarning("QuestResultTable: currentActualStatsBehaviour is null");
            return;
        }

        if (currentHeroCardBehaviour == null)
        {
            Debug.LogWarning("QuestResultTable: currentHeroCardBehaviour is null");
            return;
        }

        var actualStatsChart = currentActualStatsBehaviour.GetChart();
        var heroChart = currentHeroCardBehaviour.GetChart();

        if (actualStatsChart == null)
        {
            Debug.LogWarning("QuestResultTable: actualStatsChart is null");
            return;
        }

        if (heroChart == null)
        {
            Debug.LogWarning("QuestResultTable: heroChart is null");
            return;
        }

        var actualRadarRenderer = actualStatsChart.radarMeshCanvasRenderer;
        var actualDotsParent = actualStatsChart.dotsParent;

        if (actualRadarRenderer == null)
        {
            Debug.LogWarning("QuestResultTable: actualRadarRenderer is null");
            return;
        }

        Sequence sequence = DOTween.Sequence();

        actualRadarRenderer.transform.SetParent(heroChart.transform);
        sequence.Append(actualRadarRenderer.transform.DOLocalMove(Vector3.zero, tweenDuration).SetEase(tweenEase));
        sequence.Join(actualRadarRenderer.transform.DOLocalRotate(Vector3.zero, tweenDuration).SetEase(tweenEase));

        if (actualDotsParent != null)
        {
            actualDotsParent.SetParent(heroChart.transform);
            sequence.Join(actualDotsParent.DOLocalMove(Vector3.zero, tweenDuration).SetEase(tweenEase));
            sequence.Join(actualDotsParent.DOLocalRotate(Vector3.zero, tweenDuration).SetEase(tweenEase));
        }

        sequence.OnComplete(() =>
        {
            float heroMeshArea = heroChart.CalculateMeshArea();
            float actualStatsMeshArea = actualStatsChart.CalculateMeshArea();

            if (heroMeshArea > 0f)
            {
                meshCoveragePercentage = (heroMeshArea / actualStatsMeshArea) * 100f;
                meshCoveragePercentage = Mathf.Clamp(meshCoveragePercentage, 0f, 100f);
            }
            else
            {
                meshCoveragePercentage = 0f;
            }

            questResultTableCanvas.UpdatePercentText();
            Debug.Log($"Mesh Coverage Percentage: {meshCoveragePercentage}%");
        });
        sequence.SetAutoKill(true);
    }

    public void ShowDotSequence()
    {
        //
    }
    
    public float GetCoveragePercentage()
    {
        return meshCoveragePercentage;
    }
}