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

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");
		_nodeGoatSprites = GetNode<Sprite2D>("./VBoxContainer/Goat/Sprite2D");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
	}

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
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			switch (contextState.DialogueStateTherapist)
			{
				case Enumerations.DialogueStates.Therapist.Introduce:
					_nodeTextBox.AddDialogue("Hi. Welcome to Therapy. Please take a seat.");
					_nodeTextBox.ExecuteDialogueQueue();
					contextState.DialogueStateTherapist = Enumerations.DialogueStates.Therapist.OfferTherapy;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Therapist.OfferTherapy:
					_nodeTextBox.AddDialogue("Please take a seat.");
					_nodeTextBox.ExecuteDialogueQueue();
					break;
				default:
					break;
			}
		}
	}
}
