public class CombatService : ICombatService
{
    public CombatService() { }

    public CombatService(AbstractCombatParticipantModel source, AbstractCombatParticipantModel target)
    {
        Source = source;
        Target = target;
    }

    private AbstractCombatParticipantModel Source { get; set; }
    private AbstractCombatParticipantModel Target { get; set; }

    public double GetSourceHealth()
    {
        return Source.CurrentHealth;
    }

    public double GetTargetHealth()
    {
        return Target.CurrentHealth;
    }

    public void TakeDamage(double damage)
    {
        Source.CurrentHealth -= damage;
    }

    public void GiveDamage(double damage)
    {
        Target.CurrentHealth -= damage;
    }

    public bool SourceBelowZero()
    {
        return Source.CurrentHealth > 0;
    }

    public bool TargetBelowZero()
    {
        return Target.CurrentHealth > 0;
    }
}