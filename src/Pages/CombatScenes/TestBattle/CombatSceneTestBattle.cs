using Godot;
using System;

public partial class CombatSceneTestBattle : Node2D
{
	private Node _nodeSelf = null;
	private CombatWrapper _nodeCombatWrapper = null;
	private MarginContainer _nodeDjAttackContainer = null;
	public bool Attacking { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node>(".");
		_nodeCombatWrapper = GetNode<CombatWrapper>("./CombatWrapper");
		_nodeDjAttackContainer = GetNode<MarginContainer>("./CombatWrapper/DjAttackContainer");
		_nodeDjAttackContainer.Hide();
		Attacking = true;
		_nodeCombatWrapper.Attacking = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Attacking)
		{
			_nodeDjAttackContainer.Show();
		}
		else
		{

			_nodeDjAttackContainer.Hide();
		}
	}
}
