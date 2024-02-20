using Godot;
using System;
using System.Collections.Generic;

public partial class InteractionTextBox : CanvasLayer
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _UP_INPUT = new StringName("move_up");
	private static readonly StringName _DOWN_INPUT = new StringName("move_down");
	private static readonly StringName _OPTION_CONTAINER_SCENE = new StringName("res://ObjectLibrary/TextBoxes/InteractionTextBox/OptionContainer.tscn");

	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private VBoxContainer _nodeVBoxContainer = null;
	private Label _nodePromptLabel = null;

	private List<OptionContainer> OptionContainerList { get; set; }
	private int CurrentSelectedOptionIndex { get; set; }

	public bool IsOpen { get; set; }
	public bool IsReading { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeVBoxContainer = GetNode<VBoxContainer>("./TextBoxContainer/Panel/MarginContainer/VBoxContainer");
		_nodePromptLabel = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/VBoxContainer/PromptContainer/Prompt");

		OptionContainerList = new List<OptionContainer>();
		CurrentSelectedOptionIndex = 0;
		HideTextBox();
	}

	public override void _Process(double delta)
	{
		if (!IsOpen) return;
		if (Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			HandleInteraction();
		}
		if (Input.IsActionJustPressed(_UP_INPUT))
		{
			ShiftSelectionUp();
		}
		if (Input.IsActionJustPressed(_DOWN_INPUT))
		{
			ShiftSelectionDown();
		}
	}

	// call this to instantiate an InteractionTextBox with 
	// an initial prompt and default option
	public void StartInteraction(string promptText, string firstOptionText, int firstOptionId)
	{
		_nodePromptLabel.Text = promptText;
		AddOption(firstOptionText, firstOptionId);
	}

	// call this to add an additional option to the Option list
	public void AddOption(string optionText, int optionId)
	{
		var scene = GD.Load<PackedScene>(_OPTION_CONTAINER_SCENE);
		var instance = scene.Instantiate<OptionContainer>();
		instance.Id = optionId;
		Label nodeOptionLabel = (Label)instance.FindChild("Option", false, false);
		nodeOptionLabel.Text = optionText;
		_nodeVBoxContainer.AddChild(instance);
		OptionContainerList.Add(instance);
	}

	// call this to display the constructed prompt and option list
	// to the screen
	public void Execute()
	{
		IsOpen = true;
		IsReading = true;
		OptionContainerList[0].IsSelected = true;
		_nodeTextBoxContainer.Show();
	}

	// respond to an option, whose selection was submitted
	[Signal]
	public delegate void SelectedOptionIdEventHandler(int selectionOptionId);

	public void HandleInteraction()
	{
		EmitSignal(SignalName.SelectedOptionId, OptionContainerList[CurrentSelectedOptionIndex].Id);
		HideTextBox();
	}

	public bool CanCreateDialogue()
	{
		if (!IsOpen && IsReading)
		{
			IsReading = false;
			return false;
		}
		return !IsOpen && !IsReading;
	}

	private void HideTextBox()
	{
		IsOpen = false;
		_nodePromptLabel.Text = string.Empty;
		CurrentSelectedOptionIndex = 0;
		OptionContainerList.ForEach(instance =>
		{
			instance.QueueFree();
		});
		OptionContainerList.Clear();
		_nodeTextBoxContainer.Hide();
	}

	private void ShiftSelectionUp()
	{
		int len = OptionContainerList.Count;
		if (len == 1) return;

		OptionContainerList[CurrentSelectedOptionIndex].IsSelected = false;
		if (CurrentSelectedOptionIndex == 0)
		{
			CurrentSelectedOptionIndex = len - 1;
		}
		else
		{
			CurrentSelectedOptionIndex -= 1;
		}
		OptionContainerList[CurrentSelectedOptionIndex].IsSelected = true;
		return;
	}

	private void ShiftSelectionDown()
	{
		int len = OptionContainerList.Count;
		if (len == 1) return;

		OptionContainerList[CurrentSelectedOptionIndex].IsSelected = false;

		if (CurrentSelectedOptionIndex == (len - 1))
		{
			CurrentSelectedOptionIndex = 0;
		}
		else
		{
			CurrentSelectedOptionIndex += 1;
		}
		OptionContainerList[CurrentSelectedOptionIndex].IsSelected = true;
		return;
	}
}
