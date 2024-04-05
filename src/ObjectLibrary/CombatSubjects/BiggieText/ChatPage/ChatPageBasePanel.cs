using Godot;
using System;

public partial class ChatPageBasePanel : Panel
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private Panel _nodeSelf = null;
	private Panel _nodeAskSelectionPanel = null;
	private Label _nodeAskOptionLabel = null;
	private Panel _nodeCharmSelectionPanel = null;
	private Label _nodeCharmOptionLabel = null;
	private Panel _nodeBackSelectionPanel = null;
	private Label _nodeBackOptionLabel = null;

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

		// Chat Panel Nodes
		_nodeAskSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/AskOptionContainer/MarginContainer/Button/Panel");
		_nodeAskOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/AskOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeCharmSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/CharmOptionContainer/MarginContainer/Button/Panel");
		_nodeCharmOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/CharmOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeBackSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/BackOptionContainer/MarginContainer/Button/Panel");
		_nodeBackOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/BackOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");

		_nodeActionDescriptionMainPanel = GetNode<Panel>("../../../HudContainer/ActionInfo/Panel");
		_nodeActionTitleLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionName");
		_nodeActionEffectLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionEffect");
		_nodeActionDescriptionLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/ActionDescription");

		SelectionHelperInstance = new SelectionHelper();
		SelectionHelperInstance.AddOption((int)Enumerations.CombatOptions.Ask, (int)Enumerations.ChatPagePanelOptions.Ask, true, _nodeAskSelectionPanel, _nodeAskOptionLabel);
		SelectionHelperInstance.AddOption((int)Enumerations.CombatOptions.Charm, (int)Enumerations.ChatPagePanelOptions.Charm, false, _nodeCharmSelectionPanel, _nodeCharmOptionLabel);
		SelectionHelperInstance.AddOption(-1, (int)Enumerations.ChatPagePanelOptions.Back, false, _nodeBackSelectionPanel, _nodeBackOptionLabel);

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
				EmitSignal(SignalName.SelectChat, SelectionHelperInstance.GetSelectedOptionId());
			}
			if (Input.IsActionJustPressed(_LEFT_INPUT))
			{
				//GD.Print("Left Input");
				SelectionHelperInstance.ShiftSelectionLeft();
				ProcessSelection();
			}
			if (Input.IsActionJustPressed(_RIGHT_INPUT))
			{
				//GD.Print("Right Input");
				SelectionHelperInstance.ShiftSelectionRight();
				ProcessSelection();
			}
		}
	}

	[Signal]
	public delegate void SelectChatEventHandler(int index);

	public void ProcessSelection()
	{
		foreach (var option in SelectionHelperInstance.OptionList)
		{
			try
			{
				if (option.IsSelected)
				{
					SelectionHelperInstance.AddWhiteFont(option.OptionLabel);
					SelectionHelperInstance.AddSelectBorder(option.SelectionPanel);
					var shouldShowActionInfo = SelectionHelperInstance.HandleSelectedOptionDescription(option.Id, _nodeActionTitleLabel, _nodeActionEffectLabel, _nodeActionDescriptionLabel);
					if (shouldShowActionInfo) EmitSignal(SignalName.ShowActionInfo);
					else EmitSignal(SignalName.HideActionInfo);
				}
				else
				{
					SelectionHelperInstance.AddGreyFont(option.OptionLabel);
					SelectionHelperInstance.RemoveSelectBorder(option.SelectionPanel);
				}
			}
			catch (Exception exception)
			{
				////GD.Print($"Exception occured on option id {option.Id}: {exception.Message}");
			}
		}
	}

	public void ResetPointerOffset()
	{
		SelectionHelperInstance.Reset();
	}

	[Signal]
	public delegate void ShowActionInfoEventHandler();
	[Signal]
	public delegate void HideActionInfoEventHandler();
}
