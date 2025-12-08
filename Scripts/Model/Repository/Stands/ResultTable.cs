public class ResultTable : Table
{
    public QuestResult activeQuestResult;
    public float activeQuestResultChance;
    
    
    public void SetActiveQuestResult(QuestResult questResult)
    {
        activeQuestResult = questResult;
    }
}