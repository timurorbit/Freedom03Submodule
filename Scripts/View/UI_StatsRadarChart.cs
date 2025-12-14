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
        private Material radarMaterial;
        
        [SerializeField]
        private Texture2D radarTexture;
        
        private Stats stats;
        [SerializeField]
        public CanvasRenderer radarMeshCanvasRenderer;
        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        public void setStats(Stats stats)
        {
            this.stats = stats;
            UpdateStatsVisual();
        }

        private void Stats_OnStatsChanged(object sender, EventArgs e)
        {
            UpdateStatsVisual();
        }

        public void UpdateStatsVisual()
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
            PlaceStatDots(vertices);

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
        
        private void PlaceStatDots(Vector3[] vertices)
        {
            if (dotsParent != null)
                foreach (Transform child in dotsParent) Destroy(child.gameObject);


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
        
                dot.SetActive(true);
            }
        }
    }
}