using Godot;
using Godot.Collections;
using System;

public partial class Therapist3D_Shitty : Node3D
{
	private static readonly StringName _COMBAT_SCENE_TEST_BATTLE = new StringName("res://Pages/CombatScenes/DjBattle/CombatSceneDjBattle.tscn");

	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly int _SPRITE_FRAME_MUSHROOM = 0;
	private static readonly int _SPRITE_FRAME_GOAT_IDLE = 1;
	private static readonly int _SPRITE_FRAME_GOAT_SPEAKING = 2;
	private static readonly int _SPRITE_FRAME_CHAIR = 3;

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;
	private Node _nodeGoatBodySprites = null;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_nodeGoatBodySprites = GetNode("./StaticBody3D/BodySpriteMeshInstance");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
		_serviceTextBox.InteractionTextBox.SelectedOptionId += HandleInteraction;
	}

	public override void _Process(double delta)
	{
		var overlappingBodies = _nodeInteractableArea.GetOverlappingBodies();
		if (HelperFunctions.ContainsBiggie(overlappingBodies)
			&& Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			DisplayDialogue();
		}

		if (_serviceTextBox.TextBox.IsOpen())
		{
			_nodeGoatBodySprites.Call("set_frame", _SPRITE_FRAME_GOAT_SPEAKING);
		}
		else
		{
			_nodeGoatBodySprites.Call("set_frame", _SPRITE_FRAME_GOAT_IDLE);
		}
	}

	private void DisplayDialogue()
	{
		if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			switch (contextState.DialogueStateTherapist)
			{
				case Enumerations.DialogueStates.Therapist.Introduce:
					_serviceTextBox.TextBox.AddDialogue("Hi. Welcome to Therapy. Please take a seat.");
					_serviceTextBox.TextBox.ExecuteDialogueQueue();
					contextState.DialogueStateTherapist = Enumerations.DialogueStates.Therapist.OfferTherapy;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Therapist.OfferTherapy:
					_serviceTextBox.TextBox.AddDialogue("Please take a seat.");
					_serviceTextBox.TextBox.ExecuteDialogueQueue();
					contextState.DialogueStateTherapist = Enumerations.DialogueStates.Therapist.TestBattle;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Therapist.TestBattle:
					_serviceTextBox.InteractionTextBox.StartInteraction("Do we need to test battle?", "Yes", 0);
					_serviceTextBox.InteractionTextBox.AddOption("No", 1);
					_serviceTextBox.InteractionTextBox.Execute();
					break;
				default:
					break;
			}
		}
	}

	public void HandleInteraction(int selectedOptionId)
	{
		switch (selectedOptionId)
		{
			case 0:
				var nextScene = (PackedScene)ResourceLoader.Load(_COMBAT_SCENE_TEST_BATTLE);
				GetTree().ChangeSceneToPacked(nextScene);
				break;
			case 1:
				_serviceTextBox.TextBox.AddDialogue("Wack!");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				break;
			default:
				//////GD.Print("DJ.HandleInteraction option id did not map.");
				break;
		}
	}
}
