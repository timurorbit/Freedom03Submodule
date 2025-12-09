using System.Collections.Generic;
using UnityEngine;

public class GuildRepository
{
    private static GuildRepository instance;
    
    public static GuildRepository Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GuildRepository();
            }
            return instance;
        }
    }

    private Board board;
    private List<WelcomeTable> heroWelcomeTables;
    private List<QuestTable> questTables;
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