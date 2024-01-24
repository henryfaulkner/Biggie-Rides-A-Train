using Godot;
using System;

public partial class LevelWrapper : Node2D
{
	public static readonly Vector2I _windowSize = new Vector2I(2048, 1024);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		((Window)GetViewport()).Size = _windowSize;
		((Window)GetViewport()).Unresizable = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// ASSUMING INPUTMAP HAS A MAPPING FOR restart_scene
		if (Input.IsActionPressed("restart_scene")) {
			GetTree().ReloadCurrentScene();
		}
		
		// ASSUMING INPUTMAP HAS A MAPPING FOR terminate_game
		if (Input.IsActionPressed("terminate_game")) {
			var nextScene = (PackedScene)ResourceLoader.Load("res://Main.tscn");
			GetTree().ChangeSceneToPacked(nextScene);
		}
	}
}
