using Godot;
using System;

public partial class Mushroom3D : Node3D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (Input.IsActionJustPressed(_INTERACT_INPUT)
			&& HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
		{
			GD.Print("Emit Mushroom3D Interact");
			EmitSignal(SignalName.Interact);
		}
	}

	[Signal]
	public delegate void InteractEventHandler();
}
