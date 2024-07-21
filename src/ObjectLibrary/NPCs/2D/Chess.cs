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
		//if (!processTextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			switch (contextState.DialogueStateChess)
			{
				case Enumerations.DialogueStates.Chess.Introduce:
					{
						var processTextBox = _serviceTextBox.CreateTextBox();
						processTextBox.AddDialogue("Player 1: Hmmmmm...");
						processTextBox.AddDialogue("Player 2: Hmmmmm...");
						processTextBox.AddDialogue("*Both players act like it is their turn and are in deep focus*");
						_serviceTextBox.EnqueueProcess(processTextBox);

						_serviceTextBox.ExecuteQueuedProcesses();
					}
					contextState.DialogueStateChess = Enumerations.DialogueStates.Chess.PlayOpponent;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Chess.PlayOpponent:
					{
						var processTextBox = _serviceTextBox.CreateTextBox();
						processTextBox.AddDialogue("*Both player look to be focusing intensely but neither has made a move since you have been here*");
						_serviceTextBox.EnqueueProcess(processTextBox);

						_serviceTextBox.ExecuteQueuedProcesses();
					}
					break;
				default:
					break;
			}
		}
	}
}
