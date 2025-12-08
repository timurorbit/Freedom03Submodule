using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "RPG/Character")]
public class Character : ScriptableObject
{
    public int characterId;
    public string characterName;

    public GameObject characterPrefab;
}