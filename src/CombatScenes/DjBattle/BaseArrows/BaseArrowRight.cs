using Godot;
using System;

public partial class BaseArrowRight : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	[Signal]
	public delegate void DequeueFallingArrowRightEventHandler(int hit);

	public void HandleCollision(Enumerations.HitType hit)
	{
		GD.Print("Right HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowRight, (int)hit);
		return;
	}
}
