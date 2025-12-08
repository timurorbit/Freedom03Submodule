using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "RPG/Hero")]
public class HeroObject : CharacterObject
{
    public Stats stats;

    [Header("Visuals")]
    public string race;
    public string gender;
    public string heroClass;
}