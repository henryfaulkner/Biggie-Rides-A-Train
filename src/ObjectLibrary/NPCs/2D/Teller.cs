using Godot;
using System;

public partial class Teller : StaticBody2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Teller _nodeSelf = null;
	private Area2D _nodeInteractableArea = null;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Teller>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
	}

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
		//if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			switch (contextState.DialogueStateTeller)
			{
				case Enumerations.DialogueStates.Teller.Introduce:
					{
						var processTextBox = _serviceTextBox.CreateTextBox();
						processTextBox.AddDialogue("Hi, welcome to the Station's Teller Station. I can take your [wave amp=50 freq=6]train ticket[/wave] if you have one.");
						processTextBox.AddDialogue("Usually, I sell train ticket but not today. Today's train is SOLD OUT. Apparently, there is some huge show happening at the CATHEDRAL in West Bay.");
						processTextBox.AddDialogue("Probably another one of the CONDUCTOR'S doing. What a genius.");
						processTextBox.AddDialogue("I can tell you don't have a [wave amp=50 freq=6]train ticket[/wave] right now. Come back when you have one.");
						_serviceTextBox.EnqueueProcess(processTextBox);

						_serviceTextBox.ExecuteQueuedProcesses();
					}

					contextState.DialogueStateTeller = Enumerations.DialogueStates.Teller.AskForTicket;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Teller.AskForTicket:
					{
						var processTextBox = _serviceTextBox.CreateTextBox();
						processTextBox.AddDialogue("You have a [wave amp=50 freq=6]train ticket[/wave] yet?");
						if (CheckForTicket(contextState))
						{
							processTextBox.AddDialogue("It looks like you got your hands on a [wave amp=50 freq=6]train ticket![/wave]");
							_serviceTextBox.EnqueueProcess(processTextBox);

							var processInteractionTextBox = _serviceTextBox.CreateInteractionTextBox();
							processInteractionTextBox.StartInteraction("Are you ready to depart?", "Yes", 1);
							processInteractionTextBox.AddOption("No", 2);
							processInteractionTextBox.SelectedOptionId += HandleInteraction_Boarding;
							_serviceTextBox.EnqueueProcess(processInteractionTextBox);
						}
						else
						{
							processTextBox.AddDialogue("It seems like you don't. Come back when you have [wave amp=50 freq=6]train ticket[/wave].");
							_serviceTextBox.EnqueueProcess(processTextBox);
						}
						_serviceTextBox.ExecuteQueuedProcesses();
					}
					break;
				default:
					break;
			}
		}
	}

	private bool CheckForTicket(SaveStateModel contextState)
	{
		return contextState.HasItemTicketPieceOne
			&& contextState.HasItemTicketPieceTwo
			&& contextState.HasItemTicketPieceThree
			&& contextState.HasItemTicketPieceFour
			&& contextState.HasItemTape;
	}

	private void HandleInteraction_Boarding(int selectedOptionId)
	{
		if (selectedOptionId == 1)
		{
			TriggerFinalCutScene();
		}
		else
		{
			var processTextBox = _serviceTextBox.CreateTextBox();
			processTextBox.AddDialogue("No problemo. Come back when you're ready to board.");
			_serviceTextBox.EnqueueProcess(processTextBox);

			_serviceTextBox.ExecuteQueuedProcesses();
		}
	}

	private void TriggerFinalCutScene()
	{
		return;
	}
}
