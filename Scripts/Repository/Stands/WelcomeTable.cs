/*
 * should launch signal to BehaviourTree when hero is approved or rejected
 */
public class WelcomeTable : Table
{
    private Hero activeHero;
    
    public void SetActiveHero(Hero hero)
    {
        activeHero = hero;
    }
    
    public Hero GetActiveHero()
    {
        return activeHero;
    }
}