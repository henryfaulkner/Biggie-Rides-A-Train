using Godot;
using System;

public partial class BiggieDjCombat : CharacterBody2D
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
	public delegate void DequeueFallingArrowUpEventHandler();

	public void HandleCollisionUp()
	{
		////GD.Print("Up HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowUp);
		return;
	}

	[Signal]
	public delegate void DequeueFallingArrowRightEventHandler();

	public void HandleCollisionRight()
	{
		////GD.Print("Right HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowRight);
		return;
	}

	[Signal]
	public delegate void DequeueFallingArrowDownEventHandler();

	public void HandleCollisionDown()
	{
		////GD.Print("Down HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowDown);
		return;
	}

	[Signal]
	public delegate void DequeueFallingArrowLeftEventHandler();

	public void HandleCollisionLeft()
	{
		////GD.Print("Left HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowLeft);
		return;
	}
}
