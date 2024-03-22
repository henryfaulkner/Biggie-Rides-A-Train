using Godot;
using System;

public partial class CombatSceneDjBattle : Node2D
{
	public static readonly StringName _SCENE_BIGGIE_DEFEAT = new StringName("res://Pages/DefeatScenes/DjBattle/DefeatSceneDjBattle.tscn");
	public static readonly StringName _SCENE_CLUB = new StringName("res://Pages/Levels/Club/LevelClub.tscn");

	private static readonly int _MAX_HEALTH_PHYSICAL_BIGGIE = 9;
	private static readonly int _MAX_HEALTH_PHYSICAL_DJ = 9;
	private static readonly int _MAX_HEALTH_EMOTIONAL_DJ = 9;

	private Node2D _nodeSelf = null;
	private CombatWrapper _nodeCombatWrapper = null;
	private BiggieCombatTextBox _nodeBiggieCombatTextBox = null;
	private DjAttackContainer _nodeDjAttackContainer = null;
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
		_nodeBiggieCombatTextBox = GetNode<BiggieCombatTextBox>("./CombatWrapper/BiggieCombatTextBox");
		_nodeDjAttackContainer = GetNode<DjAttackContainer>("./CombatWrapper/SquareAttackContainer/SquareAttackPanel/DjAttackContainer");
		_nodeHitCallout = GetNode<CanvasLayer>("./CombatWrapper/HitCallout");
		_nodeBiggieHealthBar = GetNode<ProgressBar>("./CombatWrapper/HudContainer/HealthContainer/MarginContainer/Health/MarginContainer/ProgressBar");
		_nodeBiggieHpValueLabel = GetNode<Label>("./CombatWrapper/HudContainer/HealthContainer/MarginContainer/Health/HpValueLabel");
		_nodeDjHealthBar = GetNode<ProgressBar>("./CombatWrapper/EnemyPhysicalHealth/HBoxContainer/HealthContainer/MarginContainer/Health/MarginContainer/ProgressBar");
		_nodeDjHpValueLabel = GetNode<Label>("./CombatWrapper/EnemyPhysicalHealth/HBoxContainer/HealthContainer/MarginContainer/Health/HpValueLabel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		_globalCombatSingleton.NewBattle(_MAX_HEALTH_PHYSICAL_BIGGIE, _MAX_HEALTH_PHYSICAL_DJ, _MAX_HEALTH_EMOTIONAL_DJ);
		ChangeBiggieHealthBar();
		ChangeDjHealthBar();

		_nodeCombatWrapper.StartBiggieTextTurn += StartBiggieTextTurn;
		_nodeCombatWrapper.StartEnemyAttackTurn += StartEnemyAttackTurn;
		_nodeBiggieCombatTextBox.ProjectPhysicalDamage += ChangeDjHealthBar;
		_nodeBiggieCombatTextBox.EndBiggieTextTurn += EndBiggieTextTurn;
		_nodeDjAttackContainer.ProjectPhysicalDamage += ChangeBiggieHealthBar;
		_nodeDjAttackContainer.EndEnemyAttackTurn += EndEnemyAttackTurn;


		_globalCombatSingleton.CombatState = Enumerations.CombatStates.Text;
		_nodeDjAttackContainer.Hide();
		_nodeHitCallout.Hide();
		_nodeDjAttackContainer.IsAttacking = false;
		StartBiggieTextTurn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartBiggieTextTurn()
	{
		_nodeBiggieCombatTextBox.StartTurn();
		_nodeBiggieCombatTextBox.Show();
	}

	public void EndBiggieTextTurn()
	{
		//GD.Print("EndBiggieTurn");
		_nodeBiggieCombatTextBox.Visible = false;
		_nodeBiggieCombatTextBox.EndTurn();
		ChangeDjHealthBar();

		if (_globalCombatSingleton.BiggiePhysicalAttackProxy.IsTargetDefeated())
		{
			//GD.Print("Dj Physical Defeat");
			HandleDjPhysicalDefeat();
			return;
		}
		if (_globalCombatSingleton.BiggieEmotionalAttackProxy.IsTargetDefeated())
		{
			//GD.Print("Dj Emotional Defeat");
			HandleDjEmotionalDefeat();
			return;
		}

		_globalCombatSingleton.CombatState = Enumerations.CombatStates.TransitionToEnemyAttack;
		return;
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
		_nodeDjAttackContainer.EndTurn();
		ChangeBiggieHealthBar();

		if (_globalCombatSingleton.EnemyPhysicalAttackProxy.IsTargetDefeated())
		{
			//GD.Print("Biggie Physical Defeat");
			HandleBiggieDefeat();
			return;
		}

		_globalCombatSingleton.CombatState = Enumerations.CombatStates.TransitionToText;
		return;
	}

	public void ChangeBiggieHealthBar()
	{
		//GD.Print("Start ChangeBiggieHealthBar");
		_nodeBiggieHealthBar.Value = _globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetHealthPercentage();
		_nodeBiggieHpValueLabel.Text = $"{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetCurrentHealth()}/{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetMaxHealth()}";
		//GD.Print($"End ChangeBiggieHealthBar {_nodeBiggieHealthBar.Value}");


	}

	public void ChangeDjHealthBar()
	{
		//GD.Print("Start ChangeDjHealthBar");
		_nodeDjHealthBar.Value = _globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetHealthPercentage();
		_nodeDjHpValueLabel.Text = $"{_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetCurrentHealth()}/{_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetMaxHealth()}";
		//GD.Print($"End ChangeDjHealthBar {_nodeDjHealthBar.Value}");
	}

	public void HandleBiggieDefeat()
	{
		//GD.Print("HandleBiggieDefeat");
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
}
