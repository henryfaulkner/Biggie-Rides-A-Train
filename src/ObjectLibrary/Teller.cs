using Godot;
using System;

public partial class Teller : StaticBody2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	
	private Teller _nodeSelf = null;
	private TextBox _nodeTextBox = null;
	private Area2D _nodeInteractableArea = null;
	
	private bool _hasIntroduced = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Teller>(".");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
		_nodeInteractableArea = GetNode<Area2D>("./Area2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_nodeInteractableArea.GetOverlappingBodies().Count > 0
			&& Input.IsActionJustPressed(_INTERACT_INPUT)) 
		{
			DisplayDialogue();
		}
	}
	
	private void DisplayDialogue() 
	{
		if (_nodeTextBox.IsOpen()) return;
		if (!_hasIntroduced) {
			_nodeTextBox.AddDialogue("Hi, welcome to the Station's Teller Station. I can take your train ticket if you have one.");
			_nodeTextBox.AddDialogue("Usually, I sell train ticket but not today. Today's train is SOLD OUT. Apparently, there is some huge show happening at the CATHEDRAL in West Bay.");
			_nodeTextBox.AddDialogue("Probably another one of the CONDUCTOR'S doing. What a genius.");
			_nodeTextBox.AddDialogue("I can tell you don't have a train ticket right now. Come back when you have one.");
			_nodeTextBox.ExecuteDialogueQueue();
			_hasIntroduced = true;
		} 
		else
		{
			_nodeTextBox.AddDialogue("You have a ticket yet?");
			_nodeTextBox.AddDialogue("It seems like you don't. Come back when you have train ticket.");
			_nodeTextBox.ExecuteDialogueQueue();
		}
		
	}
}
