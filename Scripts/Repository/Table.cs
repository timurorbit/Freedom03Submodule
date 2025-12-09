using System;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    private Line line;

    private void Awake()
    {
        if (line == null)
        {
            line = GetComponentInChildren<Line>();
        }
    }
}