using Godot;
using System;

public partial class BaseArrowUp : Area2D
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
	public delegate void DequeueFallingArrowUpEventHandler(int hit);

	public void HandleCollision(Enumerations.HitType hit)
	{
		EmitSignal(SignalName.DequeueFallingArrowUp, (int)hit);
		return;
	}
}
