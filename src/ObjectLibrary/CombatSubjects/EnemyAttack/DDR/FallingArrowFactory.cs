using Godot;
using System;

public class FallingArrowFactory
{
	private static readonly StringName _FALLING_ARROW_UP_SCENE = new StringName("res://ObjectLibrary/CombatSubjects/EnemyAttack/DDR/FallingArrows/FallingArrowUp.tscn");
	private static readonly StringName _FALLING_ARROW_RIGHT_SCENE = new StringName("res://ObjectLibrary/CombatSubjects/EnemyAttack/DDR/FallingArrows/FallingArrowRight.tscn");
	private static readonly StringName _FALLING_ARROW_DOWN_SCENE = new StringName("res://ObjectLibrary/CombatSubjects/EnemyAttack/DDR/FallingArrows/FallingArrowDown.tscn");
	private static readonly StringName _FALLING_ARROW_LEFT_SCENE = new StringName("res://ObjectLibrary/CombatSubjects/EnemyAttack/DDR/FallingArrows/FallingArrowLeft.tscn");

	private Vector2 BaseArrowUpPosition { get; set; }
	private Vector2 BaseArrowRightPosition { get; set; }
	private Vector2 BaseArrowDownPosition { get; set; }
	private Vector2 BaseArrowLeftPosition { get; set; }

	public FallingArrowFactory() { }
	public FallingArrowFactory(Vector2 up, Vector2 right, Vector2 down, Vector2 left)
	{
		BaseArrowUpPosition = up;
		BaseArrowRightPosition = right;
		BaseArrowDownPosition = down;
		BaseArrowLeftPosition = left;
	}

	public CharacterBody2D SpawnFallingArrowUp()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_UP_SCENE);
		var instance = scene.Instantiate<CharacterBody2D>();
		instance.Position = new Vector2(BaseArrowUpPosition.X, BaseArrowUpPosition.Y - 300);
		return instance;
	}

	public CharacterBody2D SpawnFallingArrowRight()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_RIGHT_SCENE);
		var instance = scene.Instantiate<CharacterBody2D>();
		instance.Position = new Vector2(BaseArrowRightPosition.X + 300, BaseArrowRightPosition.Y);
		return instance;
	}

	public CharacterBody2D SpawnFallingArrowDown()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_DOWN_SCENE);
		var instance = scene.Instantiate<CharacterBody2D>();
		instance.Position = new Vector2(BaseArrowDownPosition.X, BaseArrowDownPosition.Y + 300);
		return instance;
	}

	public CharacterBody2D SpawnFallingArrowLeft()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_LEFT_SCENE);
		var instance = scene.Instantiate<CharacterBody2D>();
		instance.Position = new Vector2(BaseArrowLeftPosition.X - 300, BaseArrowLeftPosition.Y);
		return instance;
	}
}
