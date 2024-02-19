using Godot;
using System;

public partial class FightPageBasePanel : Panel
{
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private Panel _nodeSelf = null;
	private Panel _nodeScratchSelectionPanel = null;
	private Label _nodeScratchOptionLabel = null;
	private Panel _nodeBiteSelectionPanel = null;
	private Label _nodeBiteOptionLabel = null;
	private Panel _nodeBackSelectionPanel = null;
	private Label _nodeBackOptionLabel = null;

	public SelectionHelper SelectionHelperInstance { get; set; }
	public bool IsOpen { get; set; }

	public override void _Ready()
	{
		// Combat Scene Nodes
		_nodeSelf = GetNode<Panel>(".");

		// Fight Panel Nodes
		_nodeScratchSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ScratchOptionContainer/MarginContainer/Button/Panel");
		_nodeScratchOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ScratchOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeBiteSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/BiteOptionContainer/MarginContainer/Button/Panel");
		_nodeBiteOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/BiteOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeBackSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/Button/Panel");
		_nodeBackOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");

		SelectionHelperInstance = new SelectionHelper();
		SelectionHelperInstance.AddOption((int)Enumerations.FightPagePanelOptions.Scratch, true, _nodeScratchSelectionPanel, _nodeScratchOptionLabel);
		SelectionHelperInstance.AddOption((int)Enumerations.FightPagePanelOptions.Bite, false, _nodeBiteSelectionPanel, _nodeBiteOptionLabel);
		SelectionHelperInstance.AddOption((int)Enumerations.FightPagePanelOptions.Back, false, _nodeBackSelectionPanel, _nodeBackOptionLabel);
		ProcessSelection();
	}

	public override void _Process(double delta)
	{
		if (!IsOpen)
		{
			if (_nodeSelf.Visible)
			{
				_nodeSelf.Visible = false;
			}
			return;
		}
		else
		{
			if (!_nodeSelf.Visible)
			{
				_nodeSelf.Visible = true;
			}
		}


		if (Input.IsActionJustPressed(_LEFT_INPUT))
		{
			GD.Print("Left Input");
			SelectionHelperInstance.ShiftSelectionLeft();
			ProcessSelection();
		}
		if (Input.IsActionJustPressed(_RIGHT_INPUT))
		{
			GD.Print("Right Input");
			SelectionHelperInstance.ShiftSelectionRight();
			ProcessSelection();
		}
	}

	public void ProcessSelection()
	{
		foreach (var option in SelectionHelperInstance.OptionList)
		{
			try
			{
				if (option.IsSelected)
				{
					GD.Print($"Selected action: {option.Id}");
					SelectionHelperInstance.AddWhiteFont(option.OptionLabel);
					SelectionHelperInstance.AddSelectBorder(option.SelectionPanel);
				}
				else
				{
					SelectionHelperInstance.AddGreyFont(option.OptionLabel);
					SelectionHelperInstance.RemoveSelectBorder(option.SelectionPanel);
				}
			}
			catch (Exception exception)
			{
				GD.Print($"Exception occured on option id {option.Id}: {exception.Message}");
			}
		}
	}
}
