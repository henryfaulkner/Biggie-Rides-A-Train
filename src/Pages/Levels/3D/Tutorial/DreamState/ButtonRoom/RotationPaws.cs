using Godot;
using System;

public partial class RotationPaws : Node3D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Area3D _nodeInteractableArea = null;

	public override void _Ready()
	{
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (Input.IsActionJustPressed(_INTERACT_INPUT)
			&& HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
		{
			//GD.Print("RotationPaws");
			EmitSignal(SignalName.Rotate);
		}
	}

	[Signal]
	public delegate void RotateEventHandler();
}
