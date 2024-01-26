using Godot;
using System;

public partial class DoorBody : StaticBody2D
{
	private static readonly StringName _LEVEL_MAIN_STATION = new StringName("res://Levels/MainStation/LevelMainStation.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Hit() 
	{
		var nextScene = (PackedScene)ResourceLoader.Load(_LEVEL_MAIN_STATION);
		GetTree().ChangeSceneToPacked(nextScene);	
	}
}
