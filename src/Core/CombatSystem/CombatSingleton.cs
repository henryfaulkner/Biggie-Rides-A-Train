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
	public List<EnemyTarget> EnemyTargetList { get; set; }
	private CombatParticipantModel EnemyEmotional { get; set; }
	public BiggieAttackProxy TargetedBiggiePhysicalAttackProxy { get; set; }
	public BiggieAttackProxy TargetedBiggieEmotionalAttackProxy { get; set; }

	public CombatSingleton()
	{
		CombatStateMachineService = new CombatStateMachineService();
		CombatStateMachineService.Reset();
		BiggiePhysical = new CombatBiggieModel(10);
		EnemyPhysicalAttackProxy = new OpponentAttackProxy(EnemyPhysical, BiggiePhysical);
		EnemyTargetList = new List<EnemyTarget>();
	}

	public void NewBattle(double totalBiggiePhysicalHealth, double totalEnemyPhysicalHealth, double totalEnemyEmotionalHealth)
	{
		CombatStateMachineService = new CombatStateMachineService();
		CombatStateMachineService.Reset();
		BiggiePhysical = new CombatBiggieModel(totalBiggiePhysicalHealth);
		EnemyPhysicalAttackProxy = new OpponentAttackProxy(EnemyPhysical, BiggiePhysical);
		EnemyTargetList = new List<EnemyTarget>();
	}

	public void AddEnemyTarget(int id, string name, Panel targetPanel, IEnemyAppearance appearance, double totalEnemyPhysicalHealth, double totalEnemyEmotionalHealth)
	{
		var tempEnemyPhysical = new CombatParticipantModel(totalEnemyPhysicalHealth);
		var tempEnemyEmotional = new CombatParticipantModel(totalEnemyEmotionalHealth);
		var enemyTarget = new EnemyTarget()
		{
			Id = id,
			Name = name,
			TargetPanel = targetPanel,
			Appearance = appearance,
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

	public bool AreAllEnemiesDefeated()
	{
		return EnemyTargetList
		.All(x => x.EnemyPhysical.CurrentHealth <= 0
			|| x.EnemyEmotional.CurrentHealth <= 0);
	}

	public bool AreAnyEnemiesPhysicallyDefeated()
	{
		return EnemyTargetList
			.Any(x => x.EnemyPhysical.CurrentHealth <= 0);
	}
}
