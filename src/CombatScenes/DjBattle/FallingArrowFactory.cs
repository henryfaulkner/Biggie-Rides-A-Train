using Godot;
using System;

public class FallingArrowFactory
{
	private static readonly StringName _FALLING_ARROW_UP_SCENE = new StringName("res://CombatScenes/DjBattle/FallingArrows/FallingArrowUp.tscn");
	private static readonly StringName _FALLING_ARROW_RIGHT_SCENE = new StringName("res://CombatScenes/DjBattle/FallingArrows/FallingArrowRight.tscn");
	private static readonly StringName _FALLING_ARROW_DOWN_SCENE = new StringName("res://CombatScenes/DjBattle/FallingArrows/FallingArrowDown.tscn");
	private static readonly StringName _FALLING_ARROW_LEFT_SCENE = new StringName("res://CombatScenes/DjBattle/FallingArrows/FallingArrowLeft.tscn");

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

	public Area2D SpawnFallingArrowUp()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_UP_SCENE);
		var instance = scene.Instantiate<Area2D>();
		instance.Position = new Vector2(BaseArrowUpPosition.X, BaseArrowUpPosition.Y - 150);
		return instance;
	}

	public Area2D SpawnFallingArrowRight()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_RIGHT_SCENE);
		var instance = scene.Instantiate<Area2D>();
		instance.Position = new Vector2(BaseArrowRightPosition.X + 150, BaseArrowRightPosition.Y);
		return instance;
	}

	public Area2D SpawnFallingArrowDown()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_DOWN_SCENE);
		var instance = scene.Instantiate<Area2D>();
		instance.Position = new Vector2(BaseArrowDownPosition.X, BaseArrowDownPosition.Y + 150);
		return instance;
	}

	public Area2D SpawnFallingArrowLeft()
	{
		var scene = GD.Load<PackedScene>(_FALLING_ARROW_LEFT_SCENE);
		var instance = scene.Instantiate<Area2D>();
		instance.Position = new Vector2(BaseArrowLeftPosition.X - 150, BaseArrowLeftPosition.Y);
		return instance;
	}
}
