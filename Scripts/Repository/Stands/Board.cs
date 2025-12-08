using System.Collections.Generic;

class Board
{
    private List<QuestResult> questsToTake;

    public Board()
    {
        questsToTake = new List<QuestResult>();
    }

    public Board(List<QuestResult> questsToTake)
    {
        this.questsToTake = questsToTake;
    }

    public void addQuestToBoard(QuestResult quest)
    {
        questsToTake.Add(quest);
    }

    public QuestResult getQuestByStats(Stats stats)
    {
        //TODO implement better logic 
        return getFirstQuestFromBoard();
    }

    public QuestResult getFirstQuestFromBoard()
    {
        var toTake = questsToTake[0];
        toTake.state = QuestResultState.Taken;
        questsToTake.RemoveAt(0);
        return toTake;
    }
}