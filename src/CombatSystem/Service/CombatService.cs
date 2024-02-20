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
        return Source.Health;
    }

    public double GetTargetHealth()
    {
        return Target.Health;
    }

    public void TakeDamage(double damage)
    {
        Source.Health -= damage;
    }

    public void GiveDamage(double damage)
    {
        Target.Health -= damage;
    }

    public bool SourceBelowZero()
    {
        return Source.Health > 0;
    }

    public bool TargetBelowZero()
    {
        return Target.Health > 0;
    }
}