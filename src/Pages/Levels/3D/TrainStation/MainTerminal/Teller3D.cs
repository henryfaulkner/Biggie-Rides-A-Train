using Godot;
using System;

public partial class Teller3D : StaticBody3D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Area3D _nodeInteractableArea = null;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
	}

	public override void _Process(double delta)
	{
		if (HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies())
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
			switch (contextState.DialogueStateTeller)
			{
				case Enumerations.DialogueStates.Teller.Introduce:
					_serviceTextBox.TextBox.AddDialogue("Hi, welcome to the Station's Teller Station. I can take your [wave amp=50 freq=6]train ticket[/wave] if you have one.");
					_serviceTextBox.TextBox.AddDialogue("Usually, I sell train ticket but not today. Today's train is SOLD OUT. Apparently, there is some huge show happening at the CATHEDRAL in West Bay.");
					_serviceTextBox.TextBox.AddDialogue("Probably another one of the CONDUCTOR'S doing. What a genius.");
					_serviceTextBox.TextBox.AddDialogue("I can tell you don't have a [wave amp=50 freq=6]train ticket[/wave] right now. Come back when you have one.");
					_serviceTextBox.TextBox.ExecuteDialogueQueue();
					contextState.DialogueStateTeller = Enumerations.DialogueStates.Teller.AskForTicket;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Teller.AskForTicket:
					_serviceTextBox.TextBox.AddDialogue("You have a [wave amp=50 freq=6]train ticket[/wave] yet?");
					if (CheckForTicket(contextState))
					{
						_serviceTextBox.TextBox.AddDialogue("It looks like you got your hands on a [wave amp=50 freq=6]train ticket![/wave]");
						_serviceTextBox.TextBox.ExecuteDialogueQueue();
						_serviceTextBox.InteractionTextBox.StartInteraction("Are you ready to depart?", "Yes", 1);
						_serviceTextBox.InteractionTextBox.AddOption("No", 2);
						_serviceTextBox.InteractionTextBox.SelectedOptionId += HandleInteraction_Boarding;
						_serviceTextBox.InteractionTextBox.Execute();
					}
					else
					{
						_serviceTextBox.TextBox.AddDialogue("It seems like you don't. Come back when you have [wave amp=50 freq=6]train ticket[/wave].");
						_serviceTextBox.TextBox.ExecuteDialogueQueue();
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
			_serviceTextBox.TextBox.AddDialogue("No problemo. Come back when you're ready to board.");
			_serviceTextBox.TextBox.ExecuteDialogueQueue();
		}
	}

	private void TriggerFinalCutScene()
	{
		return;
	}
}
