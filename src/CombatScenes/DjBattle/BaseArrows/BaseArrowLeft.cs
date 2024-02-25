using Godot;
using System;

public partial class BaseArrowLeft : Area2D
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
	public delegate void DequeueFallingArrowLeftEventHandler(int hit);

	public void HandleCollision(Enumerations.HitType hit)
	{
		GD.Print("Left HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowLeft, (int)hit);
		return;
	}
}
