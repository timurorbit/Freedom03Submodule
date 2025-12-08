using UnityEngine.Localization;

public class Hero
{
    private HeroObject template;
    private Stats stats;
    public LocalizedString displayName;

    public Hero(HeroObject template)
    {
        this.template = template;
        stats = new Stats(template.stats);
    }
    
    public Stats GetStats()
    {
        return stats;
    }
}