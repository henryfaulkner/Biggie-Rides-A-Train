using Godot;
using System;

public partial class BiggieMushroomCombat : CharacterBody2D
{
	private static readonly StringName _MOVE_LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _MOVE_UP_INPUT = new StringName("move_up");
	private static readonly StringName _MOVE_RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _MOVE_DOWN_INPUT = new StringName("move_down");
	
	[Export]
	public int Speed { get; set; } = 400;

	public void GetInput()
	{

		Vector2 inputDirection = Input.GetVector(_MOVE_LEFT_INPUT, _MOVE_RIGHT_INPUT, _MOVE_UP_INPUT, _MOVE_DOWN_INPUT);
		Velocity = inputDirection * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}
