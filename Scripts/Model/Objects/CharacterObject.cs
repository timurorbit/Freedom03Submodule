using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "RPG/Character")]
public class CharacterObject : ScriptableObject
{
    public int characterId;
    public string characterName;

    public GameObject characterPrefab;
}