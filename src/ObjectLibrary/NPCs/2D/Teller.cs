using Godot;
using System;

public partial class Teller : StaticBody2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Teller _nodeSelf = null;
	private Area2D _nodeInteractableArea = null;
	private TextBox _nodeTextBox = null;
	private InteractionTextBox _nodeInteractionTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Teller>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("../InteractionTextBox");
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
		if (!_nodeTextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateService())
		{
			var contextState = context.Load();
			switch (contextState.DialogueStateTeller)
			{
				case Enumerations.DialogueStates.Teller.Introduce:
					_nodeTextBox.AddDialogue("Hi, welcome to the Station's Teller Station. I can take your [wave amp=50 freq=6]train ticket[/wave] if you have one.");
					_nodeTextBox.AddDialogue("Usually, I sell train ticket but not today. Today's train is SOLD OUT. Apparently, there is some huge show happening at the CATHEDRAL in West Bay.");
					_nodeTextBox.AddDialogue("Probably another one of the CONDUCTOR'S doing. What a genius.");
					_nodeTextBox.AddDialogue("I can tell you don't have a [wave amp=50 freq=6]train ticket[/wave] right now. Come back when you have one.");
					_nodeTextBox.ExecuteDialogueQueue();
					contextState.DialogueStateTeller = Enumerations.DialogueStates.Teller.AskForTicket;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.Teller.AskForTicket:
					_nodeTextBox.AddDialogue("You have a [wave amp=50 freq=6]train ticket[/wave] yet?");
					if (CheckForTicket(contextState))
					{
						_nodeTextBox.AddDialogue("It looks like you got your hands on a [wave amp=50 freq=6]train ticket![/wave]");
						_nodeTextBox.ExecuteDialogueQueue();
						_nodeInteractionTextBox.StartInteraction("Are you ready to depart?", "Yes", 1);
						_nodeInteractionTextBox.AddOption("No", 2);
						_nodeInteractionTextBox.SelectedOptionId += HandleInteraction_Boarding;
						_nodeInteractionTextBox.Execute();
					}
					else
					{
						_nodeTextBox.AddDialogue("It seems like you don't. Come back when you have [wave amp=50 freq=6]train ticket[/wave].");
						_nodeTextBox.ExecuteDialogueQueue();
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
			_nodeTextBox.AddDialogue("No problemo. Come back when you're ready to board.");
			_nodeTextBox.ExecuteDialogueQueue();
		}
	}

	private void TriggerFinalCutScene()
	{
		return;
	}
}
