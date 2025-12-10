using System.Collections.Generic;
using UnityEngine;

public class GuildRepository : MonoBehaviour
{
    private static GuildRepository instance;
    
    public static GuildRepository Instance => instance;

    private void Awake()
    {
        //TODO refactor
        if (instance == null)
        {
            instance = this;
        }
    }

    private Board board;
    [SerializeField]
    private List<WelcomeTable> heroWelcomeTables;
    [SerializeField]
    private List<QuestTable> questTables;
    [SerializeField]
    private List<ResultTable> questResultTables;
    private GuildHall hall;

    private GuildRepository()
    {
        heroWelcomeTables = new List<WelcomeTable>();
        questTables = new List<QuestTable>();
        questResultTables = new List<ResultTable>();
    }

    public GameObject GetClosestFreeQuestTableSpot()
    {
        return GetClosestFreeSpot<QuestTable>(questTables);
    }

    public GameObject GetClosestFreeWelcomeTableSpot()
    {
        return GetClosestFreeSpot<WelcomeTable>(heroWelcomeTables);
    }

    public GameObject GetClosestFreeResultTableSpot()
    {
        return GetClosestFreeSpot<ResultTable>(questResultTables);
    }

    public QuestTable GetClosestQuestTable()
    {
        if (questTables == null || questTables.Count == 0)
        {
            return null;
        }

        foreach (var table in questTables)
        {
            if (table != null)
            {
                return table;
            }
        }

        return null;
    }

    private GameObject GetClosestFreeSpot<T>(List<T> tables) where T : Table
    {
        if (tables == null || tables.Count == 0)
        {
            return null;
        }

        foreach (var table in tables)
        {
            if (table != null)
            {
                GameObject spot = table.GetClosestFreeSpot();
                if (spot != null)
                {
                    return spot;
                }
            }
        }

        return null;
    }

    public void AddWelcomeTable(WelcomeTable table)
    {
        if (table != null && !heroWelcomeTables.Contains(table))
        {
            heroWelcomeTables.Add(table);
        }
    }

    public void AddQuestTable(QuestTable table)
    {
        if (table != null && !questTables.Contains(table))
        {
            questTables.Add(table);
        }
    }

    public void AddResultTable(ResultTable table)
    {
        if (table != null && !questResultTables.Contains(table))
        {
            questResultTables.Add(table);
        }
    }
}

public class GuildHall
{
    [SerializeField]
    private GameObject entrance;
}