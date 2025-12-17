using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Behaviours;
using UnityEngine;
using Random = UnityEngine.Random;

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
    
    
    [Header("Returning Heroes")]
    [SerializeField]
    private bool PopulateReturningHeroes;
    [SerializeField] private GameObject returningHeroPrefab;
    [SerializeField] private TestQuestList testReturningHeroList;
    [SerializeField] private TestQuestList returningHeroesQuests;
    
    [Header("Stat modifiers")]
    [SerializeField] private GameObject statModifierPrefab;
    
    
    [Header("Spawning")]
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        if (PopulatePeasants)
        {
            StartCoroutine(populatePeasants());   
        }
        if (PopulateBoard)
        {
            StartCoroutine(populateBoard());
        }

        if (PopulateHeroes)
        {
            StartCoroutine(populateHeroes());
        }

        if (PopulateReturningHeroes)
        {
            StartCoroutine(populateReturningHeroes());
        }
    }

    private IEnumerator populateReturningHeroes()
    {
        var heroTemplates = testReturningHeroList.heroTemplates;
        for (int i = 0; i < heroTemplates.Count; i++)
        {
            
            // Create hero
            var heroTemplate = heroTemplates[i];
            yield return new WaitForSeconds(1f);
            var peasant = Instantiate(returningHeroPrefab, transform);
            var characterBehaviour = peasant.GetComponent<CharacterBehaviour>();
            characterBehaviour.Initialize(heroTemplate);
            
            var behaviour = peasant.GetComponent<HeroBehaviour>();
            var hero = new Hero(heroTemplate);
            behaviour.heroCard.SetHero(hero);
            behaviour.heroCard.SwitchState(false);
            
            // Add stat modifier
            var statModifier = Instantiate(statModifierPrefab, behaviour.statModifiersParent.transform);
            var statModifierBehaviour = statModifier.GetComponent<StatModifierBehaviour>();
            behaviour.statModifiers.Add(statModifierBehaviour);
            statModifier.transform.localPosition = Vector3.zero;
            
            
            // Add quest
            var QuestPrefab = Instantiate(questResultBehaviourPrefab, behaviour.questPosition.transform);
            var resultBehaviour = QuestPrefab.GetComponent<QuestResultBehaviour>();
            var quest = new Quest(returningHeroesQuests.questTemplates[i]);
            resultBehaviour.setQuest(quest);
            resultBehaviour.SwitchState(QuestResultState.Assigned);
            behaviour.currentQuestResultBehaviour = resultBehaviour;
        }
    }

    private IEnumerator populateHeroes()
    {
        var heroTemplates = testHeroList.heroTemplates;
        foreach (var heroTemplate in heroTemplates)
        {
            yield return new WaitForSeconds(1f);
            var peasant = Instantiate(heroPrefab, transform);
            var characterBehaviour = peasant.GetComponent<CharacterBehaviour>();
            characterBehaviour.Initialize(heroTemplate);
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

    private IEnumerator populatePeasants()
    {
        var questTemplates = testQuestList.questTemplates;
        foreach (var questTemplate in questTemplates)
        {
            yield return new WaitForSeconds(1f);
            var peasant = Instantiate(peasantPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity, transform);
            var characterBehaviour = peasant.GetComponent<CharacterBehaviour>();
            characterBehaviour.Initialize(testQuestList.characterTemplates[Random.Range(0, testQuestList.characterTemplates.Count - 1)]);
            var behaviour = peasant.GetComponent<PeasantBehaviour>();
            var quest = new Quest(questTemplate);
            behaviour.questResultBehaviour.setQuest(quest);
        }
    }
}