using Godot;
using System;

public partial class MainStationDoor : Area2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact"); 
	private static readonly StringName _LEVEL_MAIN_STATION = new StringName("res://Levels/MainStation/LevelMainStation.tscn"); 
	
	private Area2D _nodeSelf = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Area2D>(".");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (ShouldRedirect())
		{
			Redirect();
		}
	}
	
	private bool ShouldRedirect() {
		return 
			_nodeSelf.GetOverlappingBodies().Count > 0
			&& Input.IsActionJustPressed(_INTERACT_INPUT);
	}
	
	private void Redirect() 
	{
		var nextScene = (PackedScene)ResourceLoader.Load(_LEVEL_MAIN_STATION);
		GetTree().ChangeSceneToPacked(nextScene);	
	}
}
