using DG.Tweening;
using Unity.Cinemachine;
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
    
    [Header("Dot Sequence")] [SerializeField]
    private GameObject dotPrefab;
    
    [SerializeField] private CinemachineCamera resultTableCamera;
    [SerializeField] private float zoomInFOV = 30f;
    [SerializeField] private float zoomDuration = 1f;
    [SerializeField] private float dotMoveDuration = 3f;
    
    private GameObject currentDotInstance;
    private float originalFOV;

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
                meshCoveragePercentage = (actualStatsMeshArea / heroMeshArea) * 100f;
            }
            else
            {
                meshCoveragePercentage = 0f;
            }

            Debug.Log($"Mesh Coverage Percentage: {meshCoveragePercentage}%");
        });

        sequence.SetAutoKill(true);
    }
    
    public void ShowDotSequence()
    {
        if (currentActualStatsBehaviour == null)
        {
            Debug.LogWarning("QuestResultTable: currentActualStatsBehaviour is null");
            return;
        }

        var chart = currentActualStatsBehaviour.GetChart();
        if (chart == null)
        {
            Debug.LogWarning("QuestResultTable: chart is null");
            return;
        }

        var radarRenderer = chart.radarMeshCanvasRenderer;
        if (radarRenderer == null)
        {
            Debug.LogWarning("QuestResultTable: radarRenderer is null");
            return;
        }

        if (dotPrefab == null)
        {
            Debug.LogWarning("QuestResultTable: dotPrefab is null");
            return;
        }

        if (resultTableCamera == null)
        {
            Debug.LogWarning("QuestResultTable: resultTableCamera is null");
            return;
        }

        Sequence mainSequence = DOTween.Sequence();

        Vector3 chartCenter = radarRenderer.transform.position;
        currentDotInstance = Instantiate(dotPrefab, chartCenter, Quaternion.identity, radarRenderer.transform);

        RectTransform dotRect = currentDotInstance.GetComponent<RectTransform>();
        if (dotRect != null)
        {
            dotRect.anchoredPosition = Vector2.zero;
        }

        originalFOV = resultTableCamera.Lens.FieldOfView;

        mainSequence.Append(DOTween.To(() => resultTableCamera.Lens.FieldOfView,
            x => resultTableCamera.Lens.FieldOfView = x,
            zoomInFOV,
            zoomDuration));

        mainSequence.AppendCallback(() =>
        {
            AnimateDotMovement();
        });

        mainSequence.AppendInterval(dotMoveDuration);

        mainSequence.AppendCallback(() =>
        {
            if (currentDotInstance != null)
            {
                DOTween.Kill(currentDotInstance.transform);
            }
        });

        mainSequence.Append(DOTween.To(() => resultTableCamera.Lens.FieldOfView,
            x => resultTableCamera.Lens.FieldOfView = x,
            originalFOV,
            zoomDuration));

        mainSequence.OnComplete(() =>
        {
            if (currentDotInstance != null)
            {
                Destroy(currentDotInstance);
                currentDotInstance = null;
            }
        });

        mainSequence.SetAutoKill(true);
    }

    private void AnimateDotMovement()
    {
        if (currentDotInstance == null || currentActualStatsBehaviour == null)
            return;

        var chart = currentActualStatsBehaviour.GetChart();
        if (chart == null)
            return;

        RectTransform dotRect = currentDotInstance.GetComponent<RectTransform>();
        if (dotRect == null)
            return;

        float radarChartSize = 169f;
        
        Sequence moveSequence = DOTween.Sequence();
        
        int numBounces = Mathf.CeilToInt(dotMoveDuration * 2);
        
        for (int i = 0; i < numBounces; i++)
        {
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            float randomDistance = Random.Range(radarChartSize * 0.3f, radarChartSize * 0.9f);
            Vector2 targetPosition = randomDirection * randomDistance;
            
            float moveDuration = dotMoveDuration / numBounces;
            moveSequence.Append(dotRect.DOAnchorPos(targetPosition, moveDuration).SetEase(Ease.InOutQuad));
        }
        
        moveSequence.SetTarget(currentDotInstance.transform);
    }
}