using Godot;
using System;

public partial class BaseArrowDown : Area2D
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
	public delegate void DequeueFallingArrowDownEventHandler(int hit);

	public void HandleCollision(Enumerations.HitType hit)
	{
		GD.Print("Down HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowDown, (int)hit);
		return;
	}
}
