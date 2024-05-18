using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CombatSingleton : Node
{
	public CombatStateMachineService CombatStateMachineService { get; set; }
	public EnemyAttackPanelService EnemyAttackPanelService { get; set; }

	public OpponentAttackProxy EnemyPhysicalAttackProxy { get; set; }

	private CombatBiggieModel BiggiePhysical { get; set; }
	private CombatParticipantModel EnemyPhysical { get; set; }
	private List<EnemyTarget> EnemyTargetList { get; set; }

	public CombatSingleton()
	{
		CombatStateMachineService = new CombatStateMachineService();
		EnemyAttackPanelService = new EnemyAttackPanelService();
		BiggiePhysical = null;
		EnemyPhysicalAttackProxy = null;
	}

	public void NewBattle(double totalBiggiePhysicalHealth)
	{
		//GD.Print("Start NewBattle");
		CombatStateMachineService.Reset();
		//EnemyAttackPanelService = new EnemyAttackPanelService();
		BiggiePhysical = new CombatBiggieModel(totalBiggiePhysicalHealth);
		EnemyPhysicalAttackProxy = new OpponentAttackProxy(EnemyPhysical, BiggiePhysical);
		//GD.Print("End NewBattle");
	}

	//FOR THE COMPILER
	private CombatParticipantModel EnemyEmotional { get; set; }
	public BiggieAttackProxy BiggiePhysicalAttackProxy { get; set; }
	public BiggieAttackProxy BiggieEmotionalAttackProxy { get; set; }
	public void NewBattle(double totalBiggiePhysicalHealth, double totalEnemyPhysicalHealth, double totalEnemyEmotionalHealth) { }

	public void AddEnemyTarget(int id, Node targetNode, double totalEnemyPhysicalHealth, double totalEnemyEmotionalHealth)
	{
		var tempEnemyPhysical = new CombatParticipantModel(totalEnemyPhysicalHealth);
		var tempEnemyEmotional = new CombatParticipantModel(totalEnemyEmotionalHealth);
		var enemyTarget = new EnemyTarget()
		{
			Id = id,
			TargetNode = targetNode,
			EnemyPhysical = tempEnemyPhysical,
			EnemyEmotional = tempEnemyEmotional,
			BiggiePhysicalAttackProxy = new BiggieAttackProxy(BiggiePhysical, tempEnemyPhysical),
			BiggieEmotionalAttackProxy = new BiggieAttackProxy(BiggiePhysical, tempEnemyEmotional),
		};
		EnemyTargetList.Add(enemyTarget);
	}

	public void RemoveEnemyTarget(int id)
	{
		var enemyTarget = EnemyTargetList.FirstOrDefault(x => x.Id == id);
		if (enemyTarget != null)
		{
			EnemyTargetList.Remove(enemyTarget);
		}
	}
}

public partial class EnemyTarget
{
	public int Id { get; set; }
	public Node TargetNode { get; set; }
	public CombatParticipantModel EnemyEmotional { get; set; }
	public CombatParticipantModel EnemyPhysical { get; set; }
	public BiggieAttackProxy BiggiePhysicalAttackProxy { get; set; }
	public BiggieAttackProxy BiggieEmotionalAttackProxy { get; set; }
}
