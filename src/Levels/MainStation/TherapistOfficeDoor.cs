using Godot;
using System;

public partial class TherapistOfficeDoor : Area2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact"); 
	private static readonly StringName _LEVEL_THERAPIST_OFFICE = new StringName("res://Levels/TherapistOffice/LevelTherapistOffice.tscn"); 
	
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
		var nextScene = (PackedScene)ResourceLoader.Load(_LEVEL_THERAPIST_OFFICE);
		GetTree().ChangeSceneToPacked(nextScene);	
	}
}
