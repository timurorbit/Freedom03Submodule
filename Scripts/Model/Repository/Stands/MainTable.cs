public class MainTable : Table
{
    public QuestResult currentResult;

    public void Approve()
    {
        
    }
    
    public void Reject()
    {
        
    }

    public void GiveStat()
    {
        GiveStat(new StatModifier(StatModifierType.UpgradeRank));
    }
    
    public void GiveStat(StatModifier modifier)
    {
        if (currentResult == null) return;
        
        Hero hero = currentResult.GetHero();
        if (hero == null) return;
        
        Stats stats = hero.GetStats();
        if (stats == null) return;
        
        modifier.Apply(stats);
    }
    
    public void RemoveStat(StatModifier modifier)
    {
        if (currentResult == null) return;
        
        Hero hero = currentResult.GetHero();
        if (hero == null) return;
        
        Stats stats = hero.GetStats();
        if (stats == null) return;
        
        modifier.Apply(stats);
    }
}