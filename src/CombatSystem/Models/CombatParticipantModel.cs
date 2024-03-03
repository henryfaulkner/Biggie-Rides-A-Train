public class CombatParticipantModel : AbstractCombatParticipantModel
{
	public CombatParticipantModel() { }
	public CombatParticipantModel(double health)
	{
		MaxHealth = health;
		CurrentHealth = health;
	}

	public override double MaxHealth { get; set; }
	public override double CurrentHealth { get; set; }
}
