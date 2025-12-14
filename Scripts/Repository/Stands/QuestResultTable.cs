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
    
    [SerializeField]
    private QuestResultTableCanvas questResultTableCanvas;


    [SerializeField] private Ease tweenEase = Ease.OutQuad;

    [Header("Coverage")] [SerializeField]
    private float meshCoveragePercentage;
    
    [Header("Dot Sequence")] [SerializeField]
    private GameObject dotPrefab;
    
    [SerializeField] private CinemachineCamera resultTableCamera;
    [SerializeField] private float zoomInFOV = 30f;
    [SerializeField] private float zoomDuration = 1f;
    [SerializeField] private float dotMoveDuration = 3f;
    [SerializeField] private float forcePower = 35f;
    
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
    
    
    
    public void isTouching()
    {
        Mesh mesh = currentHeroBehaviour.heroCard.GetChart().radarMeshCanvasRenderer.GetMesh();
        Renderer dotRenderer = currentDotInstance.GetComponentInChildren<Renderer>();
        Vector3 objectCenterWorld = dotRenderer.bounds.center;
        Transform chartTransform = currentHeroBehaviour.heroCard.GetChart().radarMeshCanvasRenderer.transform;
        Vector3 localCenter = chartTransform.InverseTransformPoint(objectCenterWorld);
        bool isInsideBounds = IsPointInMesh(mesh, localCenter);
        Debug.LogError("Dot is inside mesh: " + isInsideBounds);
    }
    
    private bool IsPointInMesh(Mesh mesh, Vector3 localPoint) {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
    
        // Project to 2D (XY plane) for UI meshes
        Vector2 point = new Vector2(localPoint.x, localPoint.y);
    
        // Simple raycast horizontally from the point and count intersections
        int intersectionCount = 0;
        for (int i = 0; i < triangles.Length; i += 3) {
            Vector2 a = new Vector2(vertices[triangles[i]].x, vertices[triangles[i]].y);
            Vector2 b = new Vector2(vertices[triangles[i + 1]].x, vertices[triangles[i + 1]].y);
            Vector2 c = new Vector2(vertices[triangles[i + 2]].x, vertices[triangles[i + 2]].y);
        
            // Check each edge of the triangle
            intersectionCount += CheckEdgeIntersection(point, a, b) ? 1 : 0;
            intersectionCount += CheckEdgeIntersection(point, b, c) ? 1 : 0;
            intersectionCount += CheckEdgeIntersection(point, c, a) ? 1 : 0;
        }
    
        // Even-odd rule: odd intersections mean inside
        return (intersectionCount % 2) == 1;
    }

    private bool CheckEdgeIntersection(Vector2 point, Vector2 start, Vector2 end) {
        // Horizontal ray from point to infinity
        if ((start.y > point.y) != (end.y > point.y) &&
            (point.x < start.x + (end.x - start.x) * (point.y - start.y) / (end.y - start.y))) {
            return true;
        }
        return false;
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

        var currentQuestRadarRenderer = actualStatsChart.radarMeshCanvasRenderer;
        var actualDotsParent = actualStatsChart.dotsParent;

        if (currentQuestRadarRenderer == null)
        {
            Debug.LogWarning("QuestResultTable: actualRadarRenderer is null");
            return;
        }

        Sequence sequence = DOTween.Sequence();

        currentQuestRadarRenderer.transform.SetParent(heroChart.transform);
        sequence.Append(currentQuestRadarRenderer.transform.DOLocalMove(Vector3.zero, tweenDuration).SetEase(tweenEase));
        sequence.Join(currentQuestRadarRenderer.transform.DOLocalRotate(Vector3.zero, tweenDuration).SetEase(tweenEase));

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
            ShowDotSequence();
            Debug.Log($"Mesh Coverage Percentage: {meshCoveragePercentage}%");
        });
        sequence.SetAutoKill(true);
    }
    
    private void ShowDotSequence()
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
        currentDotInstance.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
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
       //         Destroy(currentDotInstance);
      //          currentDotInstance = null;
                  isTouching();
            }
            questResultTableCanvas.UpdateView(QuestResultTableCanvas.QuestResultCanvasStage.Calculated);
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
        
        Rigidbody rb = currentDotInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 forceDirection = new Vector3(Random.Range(-.7f,.7f),0, Random.Range(-.7f,.7f));  // Example: push right; or Random.insideUnitCircle for random
            rb.AddForce(forceDirection * forcePower, ForceMode.Impulse);  // Impulse for instant push
        }
    }
        
        
    public float GetMeshCoveragePercentage()
    {
        return meshCoveragePercentage;
    }
}