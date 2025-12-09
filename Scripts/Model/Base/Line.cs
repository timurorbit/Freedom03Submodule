using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a queue line with multiple spots for characters.
/// Usage for People Movement Behavior:
/// 1. Attach this component to a GameObject that represents the line anchor point
/// 2. When a character wants to join the line, call GetLastAvailableSpot() to get the next free position
/// 3. Move the character to that spot's Transform position using NavMeshAgent or similar
/// 4. Mark the spot as taken by storing the character reference
/// 5. When the character leaves, clear the spot reference to make it available again
/// 6. Characters should move forward in the line when front spots become available
/// </summary>
public class Line : MonoBehaviour
{
    [Header("Line Configuration")]
    [SerializeField] private float spotSpacing = 1.5f;
    [SerializeField] private int spotCount = 5;
    
    [Header("Spot GameObjects (Optional - Auto-created if empty)")]
    [SerializeField] private List<GameObject> spotGameObjects = new List<GameObject>();
    
    private LinkedList<LineSpot> spots;

    private void Start()
    {
        InitializeSpots();
    }

    private void InitializeSpots()
    {
        spots = new LinkedList<LineSpot>();
        
        if (spotGameObjects == null || spotGameObjects.Count == 0)
        {
            CreateSpots();
        }
        else
        {
            SetupExistingSpots();
        }
    }

    private void CreateSpots()
    {
        spotGameObjects = new List<GameObject>();
        
        for (int i = 0; i < spotCount; i++)
        {
            GameObject spotObject = new GameObject($"LineSpot_{i}");
            spotObject.transform.SetParent(transform);
            spotObject.transform.localPosition = new Vector3(0, 0, i * spotSpacing);
            spotObject.transform.localRotation = Quaternion.identity;
            
            spotGameObjects.Add(spotObject);
            
            LineSpot spot = new LineSpot(i, spotObject, null);
            spots.AddLast(spot);
        }
    }

    private void SetupExistingSpots()
    {
        for (int i = 0; i < spotGameObjects.Count; i++)
        {
            if (spotGameObjects[i] != null)
            {
                LineSpot spot = new LineSpot(i, spotGameObjects[i], null);
                spots.AddLast(spot);
            }
        }
    }

    public GameObject GetLastAvailableSpot()
    {
        if (spots == null) return null;
        
        foreach (var spot in spots)
        {
            if (!spot.IsTaken())
            {
                return spot.spotGameObject;
            }
        }
        
        return null;
    }

    public bool IsSlotTaken(int index)
    {
        if (spots == null || index < 0) return false;
        
        int currentIndex = 0;
        foreach (var spot in spots)
        {
            if (currentIndex == index)
            {
                return spot.IsTaken();
            }
            currentIndex++;
        }
        
        return false;
    }

    public void OccupySpot(int index, GameObject occupant)
    {
        if (spots == null || index < 0) return;
        
        int currentIndex = 0;
        var node = spots.First;
        while (node != null)
        {
            if (currentIndex == index)
            {
                node.Value.occupant = occupant;
                return;
            }
            currentIndex++;
            node = node.Next;
        }
    }

    public void ReleaseSpot(int index)
    {
        if (spots == null || index < 0) return;
        
        int currentIndex = 0;
        var node = spots.First;
        while (node != null)
        {
            if (currentIndex == index)
            {
                node.Value.occupant = null;
                return;
            }
            currentIndex++;
            node = node.Next;
        }
    }

    public int GetSpotCount()
    {
        return spots?.Count ?? 0;
    }
}

[System.Serializable]
public struct LineSpot
{
    public int index;
    public GameObject spotGameObject;
    public GameObject occupant;

    public LineSpot(int index, GameObject spotGameObject, GameObject occupant)
    {
        this.index = index;
        this.spotGameObject = spotGameObject;
        this.occupant = occupant;
    }

    public bool IsTaken()
    {
        return occupant != null;
    }
}
