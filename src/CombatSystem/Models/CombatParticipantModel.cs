public class CombatParticipantModel : AbstractCombatParticipantModel
{
    public CombatParticipantModel() { }
    public CombatParticipantModel(double health)
    {
        TotalHealth = health;
        CurrentHealth = health;
    }

    public override double TotalHealth { get; set; }
    public override double CurrentHealth { get; set; }
}