using Godot;
using System;

public partial class CombatSceneMushroomBattle_1 : Node2D
{
	public static readonly StringName _SCENE_BIGGIE_DEFEAT = new StringName("res://Pages/DefeatScenes/DjBattle/DefeatSceneDjBattle.tscn");
	public static readonly StringName _SCENE_CLUB = new StringName("res://Pages/Levels/2D/Club/LevelClub.tscn");

	private static readonly int _MAX_HEALTH_PHYSICAL_BIGGIE = 9;
	private static readonly int _MAX_HEALTH_PHYSICAL_MUSHROOM = 9;
	private static readonly int _MAX_HEALTH_EMOTIONAL_MUSHROOM = 9;

	private Node2D _nodeSelf = null;
	private CombatWrapper _nodeCombatWrapper = null;
	private BiggieCombatMenu _nodeBiggieCombatMenu = null;
	private MushroomAttackContainer _nodeMushroomAttackContainer = null;
	private ChatterTextBox _nodeChatterTextBox = null;
	private ProgressBar _nodeBiggieHealthBar = null;
	private Label _nodeBiggieHpValueLabel = null;

	private CombatSingleton _globalCombatSingleton = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeCombatWrapper = GetNode<CombatWrapper>("./CombatWrapper");
		_nodeBiggieCombatMenu = GetNode<BiggieCombatMenu>("./CombatWrapper/BiggieCombatMenu");
		_nodeMushroomAttackContainer = GetNode<MushroomAttackContainer>("./CombatWrapper/MushroomAttackContainer");
		_nodeChatterTextBox = GetNode<ChatterTextBox>("./CombatWrapper/ChatterTextBox");
		_nodeBiggieHealthBar = GetNode<ProgressBar>("./CombatWrapper/HudContainer/HealthContainer/MarginContainer/Health/MarginContainer/ProgressBar");
		_nodeBiggieHpValueLabel = GetNode<Label>("./CombatWrapper/HudContainer/HealthContainer/MarginContainer/Health/HpValueLabel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		_globalCombatSingleton.NewBattle(_MAX_HEALTH_PHYSICAL_BIGGIE, _MAX_HEALTH_PHYSICAL_MUSHROOM, _MAX_HEALTH_EMOTIONAL_MUSHROOM);
		_globalCombatSingleton.CombatStateMachineService.SetCheckChatterConditions(CheckChatterConditions);
		ChangeBiggieHealthBar();

		_nodeCombatWrapper.StartBiggieTextTurn += StartBiggieTextTurn;
		_nodeCombatWrapper.StartEnemyAttackTurn += StartEnemyAttackTurn;
		//_nodeCombatWrapper.ProjectPhysicalDamage += ChangeDjHealthBar;
		_nodeCombatWrapper.ProjectPhysicalDamage += () => _globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		_nodeBiggieCombatMenu.EndBiggieCombatMenuTurn += EndBiggieCombatMenuTurn;
		_nodeMushroomAttackContainer.ProjectPhysicalDamage += ChangeBiggieHealthBar;
		_nodeMushroomAttackContainer.EndEnemyAttackTurn += EndEnemyAttackTurn;

		_nodeMushroomAttackContainer.Hide();
		_nodeMushroomAttackContainer.IsAttacking = false;
		StartBiggieTextTurn();
	}

	public void StartBiggieTextTurn()
	{
		_nodeBiggieCombatMenu.StartTurn();
		_nodeBiggieCombatMenu.Show();
	}

	public void EndBiggieCombatMenuTurn(int combatOptionIndex)
	{
		//GD.Print("EndBiggieTurn");
		var combatOption = (Enumerations.Combat.CombatOptions)combatOptionIndex;
		_nodeBiggieCombatMenu.Visible = false;
		_nodeBiggieCombatMenu.EndTurn();

		if (_globalCombatSingleton.BiggiePhysicalAttackProxy.IsTargetDefeated())
		{
			//GD.Print("Dj Physical Defeat");
			HandleMushroomPhysicalDefeat();
			return;
		}
		if (_globalCombatSingleton.BiggieEmotionalAttackProxy.IsTargetDefeated())
		{
			//GD.Print("Dj Emotional Defeat");
			HandleMushroomEmotionalDefeat();
			return;
		}

		switch (combatOption)
		{
			case Enumerations.Combat.CombatOptions.Ask:
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectChatAsk);
				break;
			case Enumerations.Combat.CombatOptions.Charm:
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectChatCharm);
				break;
			case Enumerations.Combat.CombatOptions.Scratch:
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectFightScratch);
				break;
			case Enumerations.Combat.CombatOptions.Bite:
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectFightBite);
				break;
			default:
				GD.Print("CombatSceneDjBattle.EndBiggieCombatMenuTurn: Could not map combat options");
				break;
		}

		return;
	}

	public void StartEnemyAttackTurn()
	{
		_nodeMushroomAttackContainer.Visible = true;
		//_nodeHitCallout.Visible = true;
		_nodeMushroomAttackContainer.StartTurn();
		GD.Print("CombatSceneMushroomBattle_1 StartEnemyAttackTurn");
	}

	public void EndEnemyAttackTurn()
	{
		_nodeMushroomAttackContainer.Visible = false;
		//_nodeHitCallout.Visible = false;
		//_nodeMushroomAttackContainer.EndTurn();
		ChangeBiggieHealthBar();

		if (_globalCombatSingleton.EnemyPhysicalAttackProxy.IsTargetDefeated())
		{
			//GD.Print("Biggie Physical Defeat");
			HandleBiggieDefeat();
			return;
		}

		_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishEnemyAttack);
		return;
	}

	public void ChangeBiggieHealthBar()
	{
		//GD.Print("Start ChangeBiggieHealthBar");
		_nodeBiggieHealthBar.Value = _globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetHealthPercentage();
		_nodeBiggieHpValueLabel.Text = $"{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetCurrentHealth()}/{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetMaxHealth()}";
		//GD.Print($"End ChangeBiggieHealthBar {_nodeBiggieHealthBar.Value}");
	}

	public bool firstDialogueDone = false;
	public bool CheckChatterConditions()
	{
		GD.Print("CombatSceneMushroomBattle_1 CheckChatterConditions is false");
		// GD.Print("CombatSceneDjBattle CheckChatterConditions");
		// if (_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetHealthPercentage() < 100 && !firstDialogueDone)
		// {
		// 	_nodeChatterTextBox.AddDialogue("Pizza Pizza.");
		// 	_nodeChatterTextBox.AddDialogue("Please.");
		// 	_nodeChatterTextBox.ExecuteDialogueQueue();
		// 	firstDialogueDone = true;
		// 	return true;
		// }
		return false;
	}

	public void HandleBiggieDefeat()
	{

	}

	public void HandleMushroomPhysicalDefeat()
	{

	}

	public void HandleMushroomEmotionalDefeat()
	{

	}
}
