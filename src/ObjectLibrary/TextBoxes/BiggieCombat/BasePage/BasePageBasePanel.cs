using Godot;
using System;

public partial class BasePageBasePanel : Panel
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private Panel _nodeSelf = null;
	private Panel _nodeDescriptionPanel = null;
	private Panel _nodeFightSelectionPanel = null;
	private Label _nodeFightOptionLabel = null;
	private Panel _nodeChatSelectionPanel = null;
	private Label _nodeChatOptionLabel = null;
	private Panel _nodeExitSelectionPanel = null;
	private Label _nodeExitOptionLabel = null;

	private Panel _nodeActionDescriptionMainPanel = null;
	private Label _nodeActionTitleLabel = null;
	private Label _nodeActionEffectLabel = null;
	private Label _nodeActionDescriptionLabel = null;

	private SelectionHelper SelectionHelperInstance { get; set; }
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

		_nodeActionDescriptionMainPanel = GetNode<Panel>("../../../HudContainer/ActionInfo/Panel");
		_nodeActionTitleLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionName");
		_nodeActionEffectLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionEffect");
		_nodeActionDescriptionLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/ActionDescription");

		SelectionHelperInstance = new SelectionHelper();
		SelectionHelperInstance.AddOption(-1, (int)Enumerations.BasePagePanelOptions.Fight, true, _nodeFightSelectionPanel, _nodeFightOptionLabel);
		SelectionHelperInstance.AddOption(-1, (int)Enumerations.BasePagePanelOptions.Chat, false, _nodeChatSelectionPanel, _nodeChatOptionLabel);
		SelectionHelperInstance.AddOption(-1, (int)Enumerations.BasePagePanelOptions.Exit, false, _nodeExitSelectionPanel, _nodeExitOptionLabel);
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

		if (!_nodeSelf.Visible)
		{
			_nodeSelf.Visible = true;
		}
		else
		{
			if (Input.IsActionJustPressed(_INTERACT_INPUT))
			{
				EmitSignal(SignalName.SelectBase, SelectionHelperInstance.GetSelectedOptionId());
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
	}

	[Signal]
	public delegate void SelectBaseEventHandler(int index);

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
					SelectionHelperInstance.HandleSelectedOptionDescription(option.Id, _nodeActionDescriptionMainPanel, _nodeActionTitleLabel, _nodeActionEffectLabel, _nodeActionDescriptionLabel);
				}
				else
				{
					SelectionHelperInstance.AddGreyFont(option.OptionLabel);
					SelectionHelperInstance.RemoveSelectBorder(option.SelectionPanel);
				}
			}
			catch (Exception exception)
			{
				//GD.Print($"Exception occured on option id {option.Id}: {exception.Message}");
			}
		}
	}

	public void ResetPointerOffset()
	{
		SelectionHelperInstance.Reset();
	}
}
