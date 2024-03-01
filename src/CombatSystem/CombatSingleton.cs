public class CombatSingleton
{
    public BiggieAttackProxy BiggiePhysicalAttackProxy { get; set; }
    public BiggieAttackProxy BiggieEmotionalAttackProxy { get; set; }
    public OpponentAttackProxy EnemyPhysicalAttackProxy { get; set; }

    private CombatBiggieModel BiggiePhysical { get; set; }
    private CombatParticipantModel EnemyEmotional { get; set; }
    private CombatParticipantModel EnemyPhysical { get; set; }

    public CombatSingleton()
    {
        BiggiePhysical = new CombatBiggieModel();
        EnemyEmotional = new CombatParticipantModel();
        EnemyPhysical = new CombatParticipantModel();

        BiggiePhysicalAttackProxy = new BiggieAttackProxy(BiggiePhysical, EnemyPhysical);
        BiggieEmotionalAttackProxy = new BiggieAttackProxy(BiggiePhysical, EnemyEmotional);
        OpponentPhysicalAttackProxy = new OpponentAttackProxy(EnemyPhysical, BiggiePhysical);
    }

    public void NewBattle(double totalBiggiePhysicalHealth, double totalEnemyEmotionalHealth, double totalEnemyPhysicalHealth)
    {
        BiggiePhysical = new CombatBiggieModel(totalBiggiePhysicalHealth);
        EnemyEmotional = new CombatParticipantModel(totalEnemyEmotionalHealth);
        EnemyPhysical = new CombatParticipantModel(totalEnemyPhysicalHealth);
    }
}