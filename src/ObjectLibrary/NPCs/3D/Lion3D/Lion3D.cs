using Godot;
using System;

public partial class Lion3D : CharacterBody3D
{
	private readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;
	private NavigationAgent3D _nodeNavigationAgent = null;


	[Export]
	public Marker3D MarkerNearDoor { get; set; }
	[Export]
	public Marker3D MarkerPastDoor { get; set; }

	private TextBoxService _serviceTextBox = null;
	private SaveStateService _serviceSaveState = null;

	[Export]
	public float Speed { get; set; }
	[Export]
	public float Acceleration { get; set; }
	public LionStateMachine StateMachine { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_nodeNavigationAgent = GetNode<NavigationAgent3D>("./NavigationAgent3D");

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		StateMachine = new LionStateMachine(_serviceSaveState);
	}

	[Signal]
	public delegate void InteractLionEventHandler();

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(_INTERACT_INPUT)
			&& HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
		{
			EmitSignal(SignalName.InteractLion);
			ProcessInteractionState();
		}
		else
		{
			ProcessWalkState(delta);
		}
	}

	private void ProcessInteractionState()
	{
		switch (StateMachine.GetStateId())
		{
			case LionStateMachine.LionStates.Introduction:
				{
					var processTextBox = _serviceTextBox.CreateTextBox();
					processTextBox.AddDialogue("FISH FOOD! Fish are food... but you're not food.");
					processTextBox.AddDialogue("Actually, very few of my fish are. You're like an honorary fish.");
					processTextBox.AddDialogue("Welcome to the station's [color=Aqua][b]BEST[/b] aquarium gift shop[/color], honor fish.");
					_serviceTextBox.EnqueueProcess(processTextBox);

					var processInteractionTextBox = _serviceTextBox.CreateInteractionTextBox();
					processInteractionTextBox.StartInteraction("Are fish food or foe? You tell me.", "Food", 1);
					processInteractionTextBox.AddOption("Foe", 2);
					processInteractionTextBox.AddOption("*evade this conversation*", 3);
					processInteractionTextBox.SelectedOptionId += HandleFoodOrFoeQuestion;
					_serviceTextBox.EnqueueProcess(processInteractionTextBox);

					_serviceTextBox.ExecuteQueuedProcesses();
				}
				break;
			case LionStateMachine.LionStates.EvadedFoodOrFoeQuestion:
				{
					var processTextBox = _serviceTextBox.CreateTextBox();
					processTextBox.AddDialogue("YARRRRR! He returns, the honor fish.");
					_serviceTextBox.EnqueueProcess(processTextBox);

					var processInteractionTextBox = _serviceTextBox.CreateInteractionTextBox();
					processInteractionTextBox.StartInteraction("Do they be food or foe?", "Food", 1);
					processInteractionTextBox.AddOption("Foe", 2);
					processInteractionTextBox.AddOption("*leave again*", 3);
					processInteractionTextBox.SelectedOptionId += HandleFoodOrFoeQuestion;
					_serviceTextBox.EnqueueProcess(processInteractionTextBox);

					_serviceTextBox.ExecuteQueuedProcesses();
				}
				break;
			case LionStateMachine.LionStates.Food:
				{
					var processTextBox = _serviceTextBox.CreateTextBox();
					processTextBox.AddDialogue("Geez.. You're a weird fish. I was looking for foe to be completely honest.");
					processTextBox.AddDialogue("Come with me to the [color=Aqua]aquarium[/color]. You will see.");
					processTextBox.CompleteProcess += () => StateMachine.Transition(LionStateMachine.LionEvents.GeneralProgreesion);
					_serviceTextBox.EnqueueProcess(processTextBox);

					_serviceTextBox.ExecuteQueuedProcesses();
				}
				break;
			case LionStateMachine.LionStates.Foe:
				{
					var processTextBox = _serviceTextBox.CreateTextBox();
					processTextBox.AddDialogue("Exactly what I have been saying!! or at least recently. Usually, I think of them more as a tax right-off.");
					processTextBox.AddDialogue("Come with me to the [color=Aqua]aquarium[/color]. You will see.");
					processTextBox.CompleteProcess += () => StateMachine.Transition(LionStateMachine.LionEvents.GeneralProgreesion);
					_serviceTextBox.EnqueueProcess(processTextBox);

					_serviceTextBox.ExecuteQueuedProcesses();
				}
				break;
			default:
				GD.Print("Lion3D is in an unmapped state.");
				break;
		}
	}

	private void ProcessWalkState(double delta)
	{
		switch (StateMachine.GetStateId())
		{
			case LionStateMachine.LionStates.WalkIntoAquarium:
				MoveTowardTarget(MarkerPastDoor, delta);
				break;
			default:
				break;
		}
	}

	private void HandleFoodOrFoeQuestion(int selectedOptionId)
	{
		if (selectedOptionId == 1) // food
		{
			StateMachine.Transition(LionStateMachine.LionEvents.AnswerFood);
		}
		else if (selectedOptionId == 2) // foe
		{
			StateMachine.Transition(LionStateMachine.LionEvents.AnswerFoe);
		}
		else if (selectedOptionId == 3) // evade
		{
			StateMachine.Transition(LionStateMachine.LionEvents.EvadeFoodOrFoeQuestion);
		}
	}

	private void MoveTowardTarget(Marker3D target, double delta)
	{
		var direction = new Vector3();

		_nodeNavigationAgent.TargetPosition = target.GlobalPosition;

		direction = _nodeNavigationAgent.GetNextPathPosition() - GlobalPosition;
		direction = direction.Normalized();

		Velocity = Velocity.Lerp(direction * Speed, Acceleration * (float)delta);
		MoveAndSlide();
	}
}
