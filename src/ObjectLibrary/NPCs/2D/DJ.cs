using Godot;
using System;

public partial class DJ : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _COMBAT_SCENE_DJ_BATTLE = new StringName("res://Pages/CombatScenes/DjBattle/CombatSceneDjBattle.tscn");

	private static readonly int _SPRITE_FRAME_MUSHROOM = 0;
	private static readonly int _SPRITE_FRAME_GOAT_IDLE = 1;
	private static readonly int _SPRITE_FRAME_GOAT_SPEAKING = 2;
	private static readonly int _SPRITE_FRAME_CHAIR = 3;

	private Node2D _nodeSelf = null;
	private Area2D _nodeInteractableArea = null;
	private Sprite2D _nodeGoatSprites = null;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");
		_nodeGoatSprites = GetNode<Sprite2D>("./VBoxContainer/Goat/Sprite2D");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
	}

	public override void _Process(double delta)
	{
		if (_nodeInteractableArea.GetOverlappingBodies().Count > 2
			&& Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			DisplayDialogue();
		}

		// if (_serviceTextBox.TextBox.IsOpen())
		// {
		// 	_nodeGoatSprites.Frame = _SPRITE_FRAME_GOAT_SPEAKING;
		// }
		// else
		// {
		// 	_nodeGoatSprites.Frame = _SPRITE_FRAME_GOAT_IDLE;
		// }
	}

	private void DisplayDialogue()
	{
		//if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			switch (contextState.DialogueStateDJ)
			{
				case Enumerations.DialogueStates.DJ.Introduce:
					{
						var processTextBox = _serviceTextBox.CreateTextBox();
						processTextBox.AddDialogue("Hi. Welcome to The Club. Please stand up.");
						_serviceTextBox.EnqueueProcess(processTextBox);

						_serviceTextBox.ExecuteQueuedProcesses();
					}
					contextState.DialogueStateDJ = Enumerations.DialogueStates.DJ.Battle;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.DJ.Battle:
					{
						var processInteractionTextBox = _serviceTextBox.CreateInteractionTextBox();
						processInteractionTextBox.StartInteraction("Do we need to dance battle?", "Yes", 0);
						processInteractionTextBox.AddOption("No", 1);
						processInteractionTextBox.SelectedOptionId += HandleInteraction;
						_serviceTextBox.EnqueueProcess(processInteractionTextBox);

						_serviceTextBox.ExecuteQueuedProcesses();
					}
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
				var nextScene = (PackedScene)ResourceLoader.Load(_COMBAT_SCENE_DJ_BATTLE);
				GetTree().ChangeSceneToPacked(nextScene);
				break;
			case 1:
				{
					var processTextBox = _serviceTextBox.CreateTextBox();
					processTextBox.AddDialogue("Wack!");
					_serviceTextBox.EnqueueProcess(processTextBox);

					_serviceTextBox.ExecuteQueuedProcesses();
				}
				break;
			default:
				//////GD.Print("DJ.HandleInteraction option id did not map.");
				break;
		}
	}
}
