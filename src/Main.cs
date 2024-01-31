using Godot;
using System;
using Newtonsoft.Json;

public partial class Main : Node2D
{
	private static readonly StringName _LEVEL_OUTSIDE_STATION = new StringName("res://Levels/OutsideStation/LevelOutsideStation.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SaveState ss = new SaveState()
		{
			FirstName = "Henry",
			LastName = "Faulkner"
		};
		string json = JsonConvert.SerializeObject(ss, Formatting.Indented);
		GD.Print(json);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

