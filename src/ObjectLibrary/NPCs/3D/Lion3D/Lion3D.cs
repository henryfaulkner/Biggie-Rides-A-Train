using Godot;
using System;

public partial class Lion3D : Node3D
{
	private readonly StringName _INTERACT_INPUT = new StringName("interact");

	[Export]
	private Node3D RestingLocation { get; set; }

	[Export]
	private PathFollow3D MoveToAquariumPathFollow { get; set; }

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;

	private TextBoxService _serviceTextBox = null;
	private SaveStateService _serviceSaveState = null;

	[Export]
	public float Speed { get; set; }
	public LionStateMachine StateMachine { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		StateMachine = new LionStateMachine(_serviceSaveState);
	}

	[Signal]
	public delegate void InteractLionEventHandler();

	public override void _Process(double _delta)
	{
		if (Input.IsActionJustPressed(_INTERACT_INPUT)
			&& HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
		{
			EmitSignal(SignalName.InteractLion);
		}
		else
		{
			ProcessState();
		}
	}

	private void ProcessState()
	{
		switch (StateMachine.GetStateId())
		{
			case LionStateMachine.LionStates.Introduction:
				_serviceTextBox.TextBox.AddDialogue("FISH FOOD! Fish are food... but you're not food.");
				_serviceTextBox.TextBox.AddDialogue("Actually, very few of my fish are. You're like an honorary fish.");
				_serviceTextBox.TextBox.AddDialogue("Welcome to the station's [b]BEST[/b] aquarium, honor fish.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				_serviceTextBox.InteractionTextBox.StartInteraction("Are fish food or foe? You tell me.", "Food", 1);
				_serviceTextBox.InteractionTextBox.AddOption("Foe", 2);
				_serviceTextBox.InteractionTextBox.AddOption("*evade this conversation*", 3);
				_serviceTextBox.InteractionTextBox.SelectedOptionId += HandleFoodOrFoeQuestion;
				_serviceTextBox.InteractionTextBox.Execute();
				break;
			case LionStateMachine.LionStates.EvadedFoodOrFoeQuestion:
				_serviceTextBox.TextBox.AddDialogue("YARRRRR! It is me honorary fish.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				_serviceTextBox.TextBox.AddDialogue("Do they be food or foe?");
				_serviceTextBox.InteractionTextBox.StartInteraction("Are fish food or foe? You tell me.", "Food", 1);
				_serviceTextBox.InteractionTextBox.AddOption("Foe", 2);
				_serviceTextBox.InteractionTextBox.AddOption("*leave again*", 3);
				_serviceTextBox.InteractionTextBox.Execute();
				break;
			case LionStateMachine.LionStates.Food:
				_serviceTextBox.TextBox.AddDialogue("Geez.. You're a weird fish. I was looking for foe to be completely honest.");
				_serviceTextBox.TextBox.AddDialogue("Come with me to the aquarium. You will see.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				StateMachine.SetStateId(LionStateMachine.LionStates.WalkIntoAquarium);
				break;
			case LionStateMachine.LionStates.Foe:
				_serviceTextBox.TextBox.AddDialogue("Exactly what I have been saying!! or at least recently. Usually, I think of them more as a tax right-off.");
				_serviceTextBox.TextBox.AddDialogue("Come with me to the aquarium. You will see.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				break;
			default:
				GD.Print("Lion3D is in an unmapped state.");
				break;
		}
	}

	private void HandleFoodOrFoeQuestion(int selectedOptionId)
	{
		if (selectedOptionId == 1) // food
		{
			StateMachine.SetStateId(LionStateMachine.LionStates.Food);
		}
		else if (selectedOptionId == 2) // foe
		{
			StateMachine.SetStateId(LionStateMachine.LionStates.Foe);
		}
		else if (selectedOptionId == 3) // evade
		{
			StateMachine.SetStateId(LionStateMachine.LionStates.EvadedFoodOrFoeQuestion);
		}
	}
}
