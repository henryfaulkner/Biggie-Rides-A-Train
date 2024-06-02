using Godot;
using System;

public partial class AutoWalk_InteractableArea3D_1 : Area3D
{
	public override void _PhysicsProcess(double _delta)
	{
		if (HelperFunctions.ContainsBiggie(GetOverlappingBodies()))
		{
			//GD.Print("Emit AutoWalk_InteractableArea3D_1 Collision");
			EmitSignal(SignalName.Collision);
		}
	}

	[Signal]
	public delegate void CollisionEventHandler();
}
