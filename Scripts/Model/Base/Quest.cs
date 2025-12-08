using System;

[Serializable]
public class Quest
{
    public QuestObject template;

    public Quest(QuestObject template)
    {
        this.template = template;
    }
}