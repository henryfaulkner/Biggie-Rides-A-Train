using Godot;
using System;

public partial class CombatSceneDjBattle : Node2D
{
	public static readonly StringName _SCENE_BIGGIE_DEFEAT = new StringName("res://Pages/DefeatScenes/DjBattle/DefeatSceneDjBattle.tscn");
	public static readonly StringName _SCENE_CLUB = new StringName("res://Pages/Levels/2D/Club/LevelClub.tscn");

	private static readonly int _MAX_HEALTH_PHYSICAL_BIGGIE = 9;
	private static readonly int _MAX_HEALTH_PHYSICAL_DJ = 9;
	private static readonly int _MAX_HEALTH_EMOTIONAL_DJ = 9;

	private Node2D _nodeSelf = null;
	private CombatWrapper _nodeCombatWrapper = null;
	private BiggieCombatMenu _nodeBiggieCombatMenu = null;
	private DjAttackContainer _nodeDjAttackContainer = null;
	private ChatterTextBox _nodeChatterTextBox = null;
	private CanvasLayer _nodeHitCallout = null;
	private ProgressBar _nodeBiggieHealthBar = null;
	private Label _nodeBiggieHpValueLabel = null;
	private ProgressBar _nodeDjHealthBar = null;
	private Label _nodeDjHpValueLabel = null;

	private CombatSingleton _globalCombatSingleton = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeCombatWrapper = GetNode<CombatWrapper>("./CombatWrapper");
		_nodeBiggieCombatMenu = GetNode<BiggieCombatMenu>("./CombatWrapper/BiggieCombatMenu");
		_nodeDjAttackContainer = GetNode<DjAttackContainer>("./CombatWrapper/EnemyAttackContainer/EnemyAttackPanel/DjAttackContainer");
		_nodeChatterTextBox = GetNode<ChatterTextBox>("./CombatWrapper/ChatterTextBox");
		_nodeHitCallout = GetNode<CanvasLayer>("./CombatWrapper/HitCallout");
		_nodeBiggieHealthBar = GetNode<ProgressBar>("./CombatWrapper/HudContainer/HealthContainer/MarginContainer/Health/MarginContainer/ProgressBar");
		_nodeBiggieHpValueLabel = GetNode<Label>("./CombatWrapper/HudContainer/HealthContainer/MarginContainer/Health/HpValueLabel");
		_nodeDjHealthBar = GetNode<ProgressBar>("./CombatWrapper/EnemyPhysicalHealth/HBoxContainer/HealthContainer/MarginContainer/Health/MarginContainer/ProgressBar");
		_nodeDjHpValueLabel = GetNode<Label>("./CombatWrapper/EnemyPhysicalHealth/HBoxContainer/HealthContainer/MarginContainer/Health/HpValueLabel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		_globalCombatSingleton.NewBattle(_MAX_HEALTH_PHYSICAL_BIGGIE, _MAX_HEALTH_PHYSICAL_DJ, _MAX_HEALTH_EMOTIONAL_DJ);
		_globalCombatSingleton.CombatStateMachineService.SetCheckChatterConditions(CheckChatterConditions);
		ChangeBiggieHealthBar();
		ChangeDjHealthBar();

		_nodeCombatWrapper.StartBiggieTextTurn += StartBiggieTextTurn;
		_nodeCombatWrapper.StartEnemyAttackTurn += StartEnemyAttackTurn;
		_nodeCombatWrapper.ProjectPhysicalDamage += ChangeDjHealthBar;
		_nodeCombatWrapper.ProjectPhysicalDamage += () => _globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		_nodeCombatWrapper.BiggieDefeat += HandleBiggieDefeat;
		_nodeCombatWrapper.EnemyListPhysicalDefeat += HandleDjPhysicalDefeat;
		_nodeCombatWrapper.EnemyListEmotionalDefeat += HandleDjEmotionalDefeat;
		_nodeDjAttackContainer.ProjectPhysicalDamage += ChangeBiggieHealthBar;
		_nodeDjAttackContainer.EndEnemyAttackTurn += EndEnemyAttackTurn;

		_nodeDjAttackContainer.Hide();
		_nodeHitCallout.Hide();
		_nodeDjAttackContainer.IsAttacking = false;
		StartBiggieTextTurn();
	}

	public void StartBiggieTextTurn()
	{
		_nodeBiggieCombatMenu.StartTurn();
		_nodeBiggieCombatMenu.Show();
	}

	public void StartEnemyAttackTurn()
	{
		_nodeDjAttackContainer.Visible = true;
		_nodeHitCallout.Visible = true;
		_nodeDjAttackContainer.StartTurn();
	}

	public void EndEnemyAttackTurn()
	{
		_nodeDjAttackContainer.Visible = false;
		_nodeHitCallout.Visible = false;
		//_nodeDjAttackContainer.EndTurn();
		ChangeBiggieHealthBar();

		if (_globalCombatSingleton.EnemyPhysicalAttackProxy.IsTargetDefeated())
		{
			////GD.Print("Biggie Physical Defeat");
			HandleBiggieDefeat();
			return;
		}

		_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishEnemyAttack);
		return;
	}

	public void ChangeBiggieHealthBar()
	{
		////GD.Print("Start ChangeBiggieHealthBar");
		_nodeBiggieHealthBar.Value = _globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetHealthPercentage();
		_nodeBiggieHpValueLabel.Text = $"{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetCurrentHealth()}/{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetMaxHealth()}";
		////GD.Print($"End ChangeBiggieHealthBar {_nodeBiggieHealthBar.Value}");
	}


	public void ChangeDjHealthBar()
	{
		////GD.Print("Start ChangeDjHealthBar");
		_nodeDjHealthBar.Value = _globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.GetTargetHealthPercentage();
		_nodeDjHpValueLabel.Text = $"{_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.GetTargetCurrentHealth()}/{_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.GetTargetMaxHealth()}";
		////GD.Print($"End ChangeDjHealthBar {_nodeDjHealthBar.Value}");
	}

	public void HandleBiggieDefeat()
	{
		////GD.Print("HandleBiggieDefeat");
		var root = GetTree().Root;

		// Remove the current level
		var level = root.GetNode("CombatSceneDjBattle");
		root.RemoveChild(level);
		level.CallDeferred("free");

		// Add the next level
		var nextLevelResource = GD.Load<PackedScene>(_SCENE_BIGGIE_DEFEAT);
		var nextLevel = nextLevelResource.Instantiate<Node>();
		root.AddChild(nextLevel);
	}

	public void HandleDjPhysicalDefeat()
	{
		GetTree().ChangeSceneToFile(_SCENE_CLUB);
	}

	public void HandleDjEmotionalDefeat()
	{
		GetTree().ChangeSceneToFile(_SCENE_CLUB);
	}

	public void HandleBiggieAttackDealDamage(double damagePercentage)
	{

	}

	public bool firstDialogueDone = false;
	public bool CheckChatterConditions()
	{
		//GD.Print("CombatSceneDjBattle CheckChatterConditions");
		if (_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.GetTargetHealthPercentage() < 100 && !firstDialogueDone)
		{
			_nodeChatterTextBox.AddDialogue("Pizza Pizza.");
			_nodeChatterTextBox.AddDialogue("Please.");
			_nodeChatterTextBox.ExecuteDialogueQueue();
			firstDialogueDone = true;
			return true;
		}
		return false;
	}
}
