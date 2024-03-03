public class CombatBiggieModel : AbstractCombatParticipantModel
{
	public CombatBiggieModel() { }
	public CombatBiggieModel(double health)
	{
		MaxHealth = health;
		CurrentHealth = health;
	}


	public override double MaxHealth { get; set; }
	public override double CurrentHealth { get; set; }
}
