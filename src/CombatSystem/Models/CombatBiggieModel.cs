public class CombatBiggieModel : AbstractCombatParticipantModel
{
    public CombatBiggieModel() { }
    public CombatBiggieModel(double health)
    {
        TotalHealth = health;
        CurrentHealth = health;
    }


    public override double TotalHealth { get; set; }
    public override double CurrentHealth { get; set; }
}