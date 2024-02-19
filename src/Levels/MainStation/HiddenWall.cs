using Godot;
using System;

public partial class HiddenWall : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly float _OPEN_WALL_Y = -500;

	private Node2D _nodeSelf = null;
	private StaticBody2D _nodeBookCase = null;
	private Area2D _nodeInteractableArea = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeBookCase = GetNode<StaticBody2D>("./BookCase");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");
	}

	public override void _Process(double delta)
	{
		if (_nodeInteractableArea.GetOverlappingBodies().Count > 0
			&& Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			_nodeSelf.Position = new Vector2(_nodeSelf.Position.X, _OPEN_WALL_Y);
		}
	}
}
