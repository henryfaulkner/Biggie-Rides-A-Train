using Godot;
using System;

public partial class Therapist : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly int _SPRITE_FRAME_MUSHROOM = 0;
	private static readonly int _SPRITE_FRAME_GOAT_IDLE = 1;
	private static readonly int _SPRITE_FRAME_GOAT_SPEAKING = 2;
	private static readonly int _SPRITE_FRAME_CHAIR = 3;
	
	private Node2D _nodeSelf = null;
	private Area2D _nodeInteractableArea = null;
	private Sprite2D _nodeGoatSprites = null;
	private TextBox _nodeTextBox = null;
	
	private bool _hasIntroduced = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");
		_nodeGoatSprites = GetNode<Sprite2D>("./VBoxContainer/Goat/Sprite2D");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_nodeInteractableArea.GetOverlappingBodies().Count > 2
			&& Input.IsActionJustPressed(_INTERACT_INPUT)) 
		{
			DisplayDialogue();
		}
		
		if (_nodeTextBox.IsOpen()) 
		{
			_nodeGoatSprites.Frame = _SPRITE_FRAME_GOAT_SPEAKING;
		}
		else 
		{
			_nodeGoatSprites.Frame = _SPRITE_FRAME_GOAT_IDLE;
		}
	}
	
	private void DisplayDialogue() 
	{
		if (!_nodeTextBox.CanCreateDialogue()) return;
		if (!_hasIntroduced) 
		{
			_nodeTextBox.AddDialogue("Hi. Welcome to Therapy. Please take a seat.");
			_nodeTextBox.ExecuteDialogueQueue();
			_hasIntroduced = true;
		} 
		else 
		{
			_nodeTextBox.AddDialogue("Please take a seat.");
			_nodeTextBox.ExecuteDialogueQueue();
		}
	}
}
