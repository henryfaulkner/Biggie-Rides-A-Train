using Godot;
using System;

public partial class CombatSceneDjBattle : Node2D
{
	private static readonly int _MAX_HEALTH_PHYSICAL_BIGGIE = 9;
	private static readonly int _MAX_HEALTH_PHYSICAL_DJ = 9;
	private static readonly int _MAX_HEALTH_EMOTIONAL_DJ = 9;

	private Node2D _nodeSelf = null;
	private BiggieCombatTextBox _nodeBiggieCombatTextBox = null;
	private DjAttackContainer _nodeDjAttackContainer = null;
	private CanvasLayer _nodeHitCallout = null;
	private ProgressBar _nodeHealthBar = null;

	private CombatSingleton _globalCombatSingleton = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeBiggieCombatTextBox = GetNode<BiggieCombatTextBox>("./CombatWrapper/BiggieCombatTextBox");
		_nodeDjAttackContainer = GetNode<DjAttackContainer>("./DjAttackContainer");
		_nodeHitCallout = GetNode<CanvasLayer>("./HitCallout");
		_nodeHealthBar = GetNode<ProgressBar>("HBoxContainer/HealthContainer/MarginContainer/Health/MarginContainer/ProgressBar");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		_globalCombatSingleton.NewBattle(_MAX_HEALTH_PHYSICAL_BIGGIE, _MAX_HEALTH_PHYSICAL_DJ, _MAX_HEALTH_EMOTIONAL_DJ);
		ChangeBiggieHealthBar();
		ChangeDjHealthBar();

		_nodeBiggieCombatTextBox.ProjectPhysicalDamage += ChangeDjHealthBar;
		_nodeBiggieCombatTextBox.EndBiggieTurn += EndBiggieTurn;
		_nodeDjAttackContainer.ProjectPhysicalDamage += ChangeBiggieHealthBar;
		_nodeDjAttackContainer.EndOpponentTurn += EndOpponentTurn;

		EndOpponentTurn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartBiggieTurn()
	{
		_nodeBiggieCombatTextBox.Visible = true;
		_nodeBiggieCombatTextBox.StartTurn();
	}

	public void EndBiggieTurn()
	{
		GD.Print("EndBiggieTurn");
		_nodeBiggieCombatTextBox.Visible = false;
		_nodeBiggieCombatTextBox.EndTurn();
		StartOpponentTurn();
		return;
	}

	public void StartOpponentTurn()
	{
		_nodeDjAttackContainer.Visible = true;
		_nodeHitCallout.Visible = true;
		_nodeDjAttackContainer.StartTurn();
	}

	public void EndOpponentTurn()
	{
		GD.Print("EndOpponentTurn");
		_nodeDjAttackContainer.Visible = false;
		_nodeHitCallout.Visible = false;
		_nodeDjAttackContainer.EndTurn();
		StartBiggieTurn();
		return;
	}

	public void ChangeBiggieHealthBar()
	{
		_nodeHealthBar.Value = _globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetHealthPercentage();
	}

	public void ChangeDjHealthBar()
	{
		_nodeHealthBar.Value = _globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetHealthPercentage();
	}
}