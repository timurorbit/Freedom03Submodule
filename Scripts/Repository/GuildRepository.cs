using UnityEngine;

public class GuildRepository
{
    private Board board;
    private WelcomeTable heroWelcomeTable;
    private QuestTable questTable;
    private ResultTable questResultTable;
    private GuildHall hall;
}

public class GuildHall
{
    [SerializeField]
    private GameObject entrance;
}