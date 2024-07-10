using Godot;
using System;

public partial class InteractionTest : Area2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Area2D _nodeSelf = null;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Area2D>(".");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
		_serviceTextBox.InteractionTextBox.SelectedOptionId += HandleInteraction;
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
		if (!_serviceTextBox.InteractionTextBox.CanCreateDialogue()) return;
		_serviceTextBox.InteractionTextBox.StartInteraction("Hello World?", "Yes, hello world", 1);
		_serviceTextBox.InteractionTextBox.AddOption("No, hello world", 2);
		_serviceTextBox.InteractionTextBox.Execute();
	}

	public void HandleInteraction(int selectedOptionId)
	{
		//////GD.Print("I am handling the interaction in InteractionTest.");
		//////GD.Print("selectedOptionId: ", selectedOptionId);
	}
}
