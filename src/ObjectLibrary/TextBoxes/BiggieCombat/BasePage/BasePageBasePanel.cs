using Godot;
using System;

public partial class BasePageBasePanel : Panel
{
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private Panel _nodeSelf = null;
	private Panel _nodeFightSelectionPanel = null;
	private Label _nodeFightOptionLabel = null;
	private Panel _nodeChatSelectionPanel = null;
	private Label _nodeChatOptionLabel = null;
	private Panel _nodeExitSelectionPanel = null;
	private Label _nodeExitOptionLabel = null;

	public SelectionHelper SelectionHelperInstance { get; set; }
	public bool IsOpen { get; set; }

	public override void _Ready()
	{
		// Combat Scene Nodes
		_nodeSelf = GetNode<Panel>(".");

		// Base Panel Nodes
		_nodeFightSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/FightOptionContainer/MarginContainer/Button/Panel");
		_nodeFightOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/FightOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeChatSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ChatOptionContainer/MarginContainer/Button/Panel");
		_nodeChatOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ChatOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeExitSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/Button/Panel");
		_nodeExitOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");

		SelectionHelperInstance = new SelectionHelper();
		SelectionHelperInstance.AddOption((int)Enumerations.BasePagePanelOptions.Fight, true, _nodeFightSelectionPanel, _nodeFightOptionLabel);
		SelectionHelperInstance.AddOption((int)Enumerations.BasePagePanelOptions.Chat, false, _nodeChatSelectionPanel, _nodeChatOptionLabel);
		SelectionHelperInstance.AddOption((int)Enumerations.BasePagePanelOptions.Exit, false, _nodeExitSelectionPanel, _nodeExitOptionLabel);
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