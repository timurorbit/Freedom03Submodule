using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quest")]
public class QuestObject : ScriptableObject
{
    public int id;
    public string questName;
    
    public Stats stats;

    [Header("Translation")] public string questTitle;
    [TextArea] public string questDescription;

    [Header("Visual")]
    public Sprite icon;
    public Sprite background;
}