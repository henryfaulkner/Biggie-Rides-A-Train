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

	public double GetSourceCurrentHealth()
	{
		return Source.CurrentHealth;
	}

	public double GetSourceMaxHealth()
	{
		return Source.MaxHealth;
	}

	public double GetTargetCurrentHealth()
	{
		return Target.CurrentHealth;
	}

	public double GetTargetMaxHealth()
	{
		return Target.MaxHealth;
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
