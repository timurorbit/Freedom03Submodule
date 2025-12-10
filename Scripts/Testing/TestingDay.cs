using System;
using System.Collections;
using _Game.Scripts.Behaviours;
using UnityEngine;

public class TestingDay : MonoBehaviour
{
    [SerializeField] private TestQuestList testQuestList;

    [SerializeField] private GameObject peasantPrefab;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var questTemplates = testQuestList.questTemplates;
        foreach (var questTemplate in questTemplates)
        {
            yield return new WaitForSeconds(2f);
            var peasant = Instantiate(peasantPrefab, transform);
            var behaviour = peasant.GetComponent<PeasantBehaviour>();
            var quest = new Quest(questTemplate);
            behaviour.questResultBehaviour.setQuest(quest);
        }
    }
}