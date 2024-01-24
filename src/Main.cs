using Godot;
using System;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_play_pressed()
	{
		var nextScene = (PackedScene)ResourceLoader.Load("res://Levels/LevelOutsideStation.tscn");
		GetTree().ChangeSceneToPacked(nextScene);
	}


	public void _on_options_pressed()
	{
		// Replace with function body.
	}


	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}

