using Godot;
using System;

public partial class InteractionTest : Area2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Area2D _nodeSelf = null;
	private InteractionTextBox _nodeInteractionTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Area2D>(".");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("../InteractionTextBox");
		_nodeInteractionTextBox.SelectedOptionId += HandleInteraction;
	}

	public override void _Process(double delta)
	{
		if (_nodeSelf.GetOverlappingBodies().Count > 0
			&& Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			DisplayDialogue();
		}
	}

	private void DisplayDialogue()
	{
		if (!_nodeInteractionTextBox.CanCreateDialogue()) return;
		_nodeInteractionTextBox.StartInteraction("Hello World?", "Yes, hello world", 1);
		_nodeInteractionTextBox.AddOption("No, hello world", 2);
		_nodeInteractionTextBox.Execute();
	}

	public void HandleInteraction(int selectedOptionId)
	{
		GD.Print("I am handling the interaction in InteractionTest.");
		GD.Print("selectedOptionId: ", selectedOptionId);
	}
}
