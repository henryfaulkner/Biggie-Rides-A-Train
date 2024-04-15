using Godot;
using System;

public partial class CombatSceneTestBattle : Node2D
{
	private Node _nodeSelf = null;
	private CombatWrapper _nodeCombatWrapper = null;
	private MarginContainer _nodeDjAttackContainer = null;
	private CombatSingleton _globalCombatSingleton = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node>(".");
		_nodeCombatWrapper = GetNode<CombatWrapper>("./CombatWrapper");
		_nodeDjAttackContainer = GetNode<MarginContainer>("./CombatWrapper/DjAttackContainer");
		_nodeDjAttackContainer.Hide();
		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_globalCombatSingleton.CombatState == Enumerations.Combat.StateMachine.States.EnemyAttack)
		{
			_nodeDjAttackContainer.Show();
		}
		else
		{
			_nodeDjAttackContainer.Hide();
		}
	}
}
