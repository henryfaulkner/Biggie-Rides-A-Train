using Godot;
using System;

public partial class Chess : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Node2D _nodeSelf = null;
	private Area2D _nodeInteractableArea = null;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
	}

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
		if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			switch (contextState.DialogueStateChess)
			{
				case Enumerations.DialogueStates.Chess.Introduce:
					_serviceTextBox.TextBox.AddDialogue("Player 1: Hmmmmm...");
					_serviceTextBox.TextBox.AddDialogue("Player 2: Hmmmmm...");
					_serviceTextBox.TextBox.AddDialogue("*Both players act like it is their turn and are in deep focus*");
					_serviceTextBox.TextBox.ExecuteDialogueQueue();
					contextState.DialogueStateChess = Enumerations.DialogueStates.Chess.PlayOpponent;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Chess.PlayOpponent:
					_serviceTextBox.TextBox.AddDialogue("*Both player look to be focusing intensely but neither has made a move since you have been here*");
					_serviceTextBox.TextBox.ExecuteDialogueQueue();
					break;
				default:
					break;
			}
		}
	}
}
