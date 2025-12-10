using System.Collections.Generic;

public class QuestResult
{
    private Hero hero;
    private Quest quest;
    private Stats prediction;
    public QuestResultState state;
    
    public QuestResult(Hero hero, Quest quest, Stats prediction)
    {
        this.hero = hero;
        this.quest = quest;
        this.prediction = prediction;
    }

    public QuestResult(Quest quest, Stats prediction)
    {
        this.quest = quest;
        this.prediction = prediction;
        state = QuestResultState.Predicted;
    }

    public QuestResult(Quest quest)
    {
        this.quest = quest;
        // Default prediction
        prediction = new Stats(Rank.F, new List<SkillType>(), quest.template.stats.reward);
        state = QuestResultState.Closed;
    }
    
    public void setHero(Hero hero)
    {
        this.hero = hero;
    }
    
    public Hero GetHero()
    {
        return hero;
    }
    
    public Quest GetQuest()
    {
        return quest;
    }
    
    public Stats GetPrediction()
    {
        return prediction;
    }
}