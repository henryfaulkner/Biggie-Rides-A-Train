using Godot;
using System;

public partial class CombatSingleton : Node
{
	public CombatStateMachineService CombatStateMachineService { get; set; }

	public BiggieAttackProxy BiggiePhysicalAttackProxy { get; set; }
	public BiggieAttackProxy BiggieEmotionalAttackProxy { get; set; }
	public OpponentAttackProxy EnemyPhysicalAttackProxy { get; set; }

	private CombatBiggieModel BiggiePhysical { get; set; }
	private CombatParticipantModel EnemyEmotional { get; set; }
	private CombatParticipantModel EnemyPhysical { get; set; }

	public CombatSingleton()
	{
		CombatStateMachineService = new CombatStateMachineService();
		BiggiePhysical = null;
		EnemyEmotional = null;
		EnemyPhysical = null;
		BiggiePhysicalAttackProxy = null;
		BiggieEmotionalAttackProxy = null;
		EnemyPhysicalAttackProxy = null;
	}

	public void NewBattle(double totalBiggiePhysicalHealth, double totalEnemyPhysicalHealth, double totalEnemyEmotionalHealth)
	{
		//GD.Print("Start NewBattle");
		CombatStateMachineService.Reset();
		BiggiePhysical = new CombatBiggieModel(totalBiggiePhysicalHealth);
		EnemyPhysical = new CombatParticipantModel(totalEnemyPhysicalHealth);
		EnemyEmotional = new CombatParticipantModel(totalEnemyEmotionalHealth);
		BiggiePhysicalAttackProxy = new BiggieAttackProxy(BiggiePhysical, EnemyPhysical);
		BiggieEmotionalAttackProxy = new BiggieAttackProxy(BiggiePhysical, EnemyEmotional);
		EnemyPhysicalAttackProxy = new OpponentAttackProxy(EnemyPhysical, BiggiePhysical);
		//GD.Print("End NewBattle");
	}
}
