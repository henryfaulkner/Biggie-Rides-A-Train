using Godot;
using System;

public partial class Chess : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	
	private Node2D _nodeSelf = null;
	private Area2D _nodeInteractableArea = null;
	private TextBox _nodeTextBox = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_nodeInteractableArea.GetOverlappingBodies().Count > 3
			&& Input.IsActionJustPressed(_INTERACT_INPUT)) 
		{
			DisplayDialogue();
		}
	}
	
	private void DisplayDialogue() 
	{
		if (!_nodeTextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			switch(contextState.DialogueStateChess)
			{
				case Enumerations.DialogueStates.Chess.Introduce:
					_nodeTextBox.AddDialogue("Player 1: Hmmmmm...");
					_nodeTextBox.AddDialogue("Player 2: Hmmmmm...");
					_nodeTextBox.AddDialogue("*Both players act like it is their turn and are in deep focus*");
					_nodeTextBox.ExecuteDialogueQueue();
					contextState.DialogueStateChess = Enumerations.DialogueStates.Chess.PlayOpponent;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Chess.PlayOpponent:
					_nodeTextBox.AddDialogue("*Both player look to be focusing intensely but neither has made a move since you have been here*");
					_nodeTextBox.ExecuteDialogueQueue();
					break;
				default:
					break;
			}
		}
	}
}
