using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts
{
    public class UI_StatsRadarChart : MonoBehaviour
    {
        
        [Header("Dot Settings")]
        public GameObject dotPrefab;        // Assign a UI Image with circle sprite in the inspector
        public Transform dotsParent;        // Optional: a parent object under your canvas
        public float dotSize = 12f;         // Size in pixels (for RectTransform)

        [SerializeField]
        private PhysicsMaterial bouncyMaterial;
        
        
        [SerializeField]
        private Material radarMaterial;
        
        [SerializeField]
        private Texture2D radarTexture;
        
        private Stats stats;
        [SerializeField]
        public CanvasRenderer radarMeshCanvasRenderer;
        private MaterialPropertyBlock propertyBlock;

        public Stats GetStats()
        {
            return stats;
        }

        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        public void setStats(Stats stats, bool createCollider = false)
        {
            this.stats = stats;
            UpdateStatsVisual(createCollider);
        }

private void CreateCollider(Vector2[] points)
        {
            // Create a parent for the boundary walls
            GameObject boundaryParent = new GameObject("RadarBoundaries");
            boundaryParent.transform.parent = radarMeshCanvasRenderer.transform;
            boundaryParent.transform.localPosition = Vector3.zero;
            boundaryParent.transform.localRotation = Quaternion.identity;
            boundaryParent.transform.localScale = Vector3.one;

            float height = 35f; // Height of the walls in Z (adjust as needed for your scene scale)
            float thickness = 0.4f; // Thickness of the walls (small value for invisible walls)

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 p1 = points[i];
                Vector2 p2 = points[(i + 1) % points.Length];

                Vector2 dir = p2 - p1;
                float length = dir.magnitude;
                Vector2 dirNormalized = dir.normalized;

                Vector2 mid = (p1 + p2) / 2f;

                // Perp points to the "left" of the direction vector
                Vector2 perp = new Vector2(-dirNormalized.y, dirNormalized.x);

                // Determine outward normal using radial direction from center (0,0)
                Vector2 midDir = mid.normalized; // Outward radial direction
                float dotWithRadial = Vector2.Dot(perp, midDir);

                Vector2 outwardNormal = (dotWithRadial > 0) ? perp : -perp;

                // Shift position outward by thickness/2 so inner face aligns with boundary
                Vector3 shift = (Vector3)(outwardNormal * (thickness / 2f));

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                GameObject wall = new GameObject("Wall" + i);
                wall.transform.parent = boundaryParent.transform;
                wall.transform.localScale = new Vector3(1, 1, 1);
                wall.transform.localPosition = new Vector3(mid.x, mid.y, 0) + shift;
                wall.transform.localRotation = Quaternion.Euler(0, 0, angle);

                BoxCollider collider = wall.AddComponent<BoxCollider>();
                collider.material = bouncyMaterial;
                collider.size = new Vector3(length, thickness, height);
            }
        }

        private void Stats_OnStatsChanged(object sender, EventArgs e)
        {
            UpdateStatsVisual();
        }

        public void UpdateStatsVisual(bool createCollider = false)
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[6];
            Vector2[] uv = new Vector2[6];
            int[] triangles = new int[3 * 5];

            float angleIncrement = 360f / 5;
            float radarChartSize = 169f;
            
            Vector3 attackVertex = Quaternion.Euler(0,0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Attack);
            int attackVertexIndex = 1;
            Vector3 defenceVertex = Quaternion.Euler(0,0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Defense);
            int defenceVertexIndex = 2;
            Vector3 mobilityVertex = Quaternion.Euler(0,0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Mobility);
            int mobilityVertexIndex = 3;
            Vector3 CharismaVertex = Quaternion.Euler(0,0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Charisma);
            int charismaVertexIndex = 4;
            Vector3 IntelligenceVertex = Quaternion.Euler(0,0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Intelligence);
            int intelligenceVertexIndex = 5;
            
            uv[0] = Vector2.zero;
            uv[attackVertexIndex] = Vector2.one; 
            uv[defenceVertexIndex] = Vector2.one; 
            uv[mobilityVertexIndex] = Vector2.one; 
            uv[charismaVertexIndex] = Vector2.one; 
            uv[intelligenceVertexIndex] = Vector2.one; 
            
            vertices[0] = Vector3.zero;
            vertices[attackVertexIndex] = attackVertex;
            vertices[defenceVertexIndex] = defenceVertex;
            vertices[mobilityVertexIndex] = mobilityVertex;
            vertices[charismaVertexIndex] = CharismaVertex;
            vertices[intelligenceVertexIndex] = IntelligenceVertex;
            PlaceStatDots(vertices, createCollider);

            triangles[0] = 0;
            triangles[1] = attackVertexIndex;
            triangles[2] = defenceVertexIndex;
            
            triangles[3] = 0;
            triangles[4] = defenceVertexIndex;
            triangles[5] = mobilityVertexIndex;
            
            triangles[6] = 0;
            triangles[7] = mobilityVertexIndex;
            triangles[8] = charismaVertexIndex;
            
            triangles[9] = 0;
            triangles[10] = charismaVertexIndex;
            triangles[11] = intelligenceVertexIndex;
            
            triangles[12] = 0;
            triangles[13] = intelligenceVertexIndex;
            triangles[14] = attackVertexIndex;
            
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            
            radarMeshCanvasRenderer.SetMesh(mesh);
            radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture);
        }
        
        private void PlaceStatDots(Vector3[] vertices, bool createCollider = false)
        {
            if (dotsParent != null)
                foreach (Transform child in dotsParent) Destroy(child.gameObject);
            
            Vector2[] points = new Vector2[vertices.Length - 1];

            for (int i = 1; i < vertices.Length; i++)
            {
                GameObject dot = Instantiate(dotPrefab, dotsParent ? dotsParent : radarMeshCanvasRenderer.transform);
                RectTransform rt = dot.GetComponent<RectTransform>();
        
                // Important: Convert local mesh position to Canvas space
                rt.anchoredPosition = new Vector2(vertices[i].x, vertices[i].y);
                rt.sizeDelta = new Vector2(dotSize, dotSize);
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                points[i - 1] = rt.anchoredPosition;
                dot.SetActive(true);
            }
            
            if (createCollider)
            {
                CreateCollider(points);
            }
        }

        public float CalculateMeshArea()
        {
            if (stats == null)
                return 0f;

            float angleIncrement = 360f / 5;
            float radarChartSize = 169f;
            
            Vector3[] vertices = new Vector3[6];
            vertices[0] = Vector3.zero;
            vertices[1] = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Attack);
            vertices[2] = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Defense);
            vertices[3] = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Mobility);
            vertices[4] = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Charisma);
            vertices[5] = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatNormalized(SkillType.Intelligence);

            float totalArea = 0f;
            for (int i = 1; i < 6; i++)
            {
                Vector3 v1 = vertices[i];
                Vector3 v2 = vertices[i == 5 ? 1 : i + 1];
                totalArea += 0.5f * Mathf.Abs(v1.x * v2.y - v2.x * v1.y);
            }
            
            return totalArea;
        }
    }
}