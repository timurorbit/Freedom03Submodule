using System;
using System.Collections;
using _Game.Scripts.Behaviours;
using UnityEngine;
using UnityEngine.Serialization;

public class TestingDay : MonoBehaviour
{
    [Header("Peasants")]
    [SerializeField] private bool PopulatePeasants;
    [SerializeField] private TestQuestList testQuestList;
    [SerializeField] private GameObject peasantPrefab;

    [Header("Board")]
    [SerializeField] private bool PopulateBoard;
    [SerializeField] private GameObject questResultBehaviourPrefab;
    [SerializeField] private TestQuestList testBoardList;

    [Header("Heroes")]
    [SerializeField] private bool PopulateHeroes;
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private TestQuestList testHeroList;

    private void Start()
    {
        if (PopulatePeasants)
        {
            StartCoroutine(SpawnCoroutine());   
        }
        if (PopulateBoard)
        {
            StartCoroutine(populateBoard());
        }

        if (PopulateHeroes)
        {
            StartCoroutine(populateHeroes());
        }
    }

    private IEnumerator populateHeroes()
    {
        var heroTemplates = testHeroList.heroTemplates;
        foreach (var heroTemplate in heroTemplates)
        {
            yield return new WaitForSeconds(1f);
            var peasant = Instantiate(heroPrefab, transform);
            var behaviour = peasant.GetComponent<HeroBehaviour>();
            var hero = new Hero(heroTemplate);
            behaviour.heroCard.SetHero(hero);
            behaviour.heroCard.SwitchState(false);
        }
    }

    private IEnumerator populateBoard()
    {
        var questTemplates = testBoardList.questTemplates;
        Board board = FindAnyObjectByType<Board>();
        foreach (var questTemplate in questTemplates)
        {
            yield return new WaitForSeconds(1f);
            var prefab = Instantiate(questResultBehaviourPrefab, transform);
            var behaviour = prefab.GetComponent<QuestResultBehaviour>();
            var quest = new Quest(questTemplate);
            behaviour.setQuest(quest);
            behaviour.SwitchState(QuestResultState.Opened);
            board.AddItemToBoard(prefab);
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        var questTemplates = testQuestList.questTemplates;
        foreach (var questTemplate in questTemplates)
        {
            yield return new WaitForSeconds(1f);
            var peasant = Instantiate(peasantPrefab, transform);
            var behaviour = peasant.GetComponent<PeasantBehaviour>();
            var quest = new Quest(questTemplate);
            behaviour.questResultBehaviour.setQuest(quest);
        }
    }
}