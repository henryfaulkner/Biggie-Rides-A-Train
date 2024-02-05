using Godot;
using System;

public partial class InteractionTest : Area2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	
	private Area2D _nodeSelf = null;
	private InteractionTextBox _nodeInteractionTextBox = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Area2D>(".");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("../InteractionTextBox");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_nodeSelf.GetOverlappingBodies().Count > 0
			&& Input.IsActionJustPressed(_INTERACT_INPUT)) 
		{
			DisplayDialogue();
		}
	}
	
	private void DisplayDialogue() 
	{
		if (!_nodeInteractionTextBox.CanCreateDialogue()) return;
		_nodeInteractionTextBox.StartInteration("Hello World?", "Yes, hello world");
		_nodeInteractionTextBox.AddOption("No, hello world");
		_nodeInteractionTextBox.Execute();
	}
}
