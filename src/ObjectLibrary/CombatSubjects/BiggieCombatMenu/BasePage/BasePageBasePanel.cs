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
	private Panel _nodeInfoSelectionPanel = null;
	private Label _nodeInfoOptionLabel = null;
	//private Panel _nodeExitSelectionPanel = null;
	//private Label _nodeExitOptionLabel = null;

	private Panel _nodeActionDescriptionMainPanel = null;
	private Label _nodeActionTitleLabel = null;
	private Label _nodeActionEffectLabel = null;
	private Label _nodeActionDescriptionLabel = null;
	private AudioStreamPlayer _audioSelect = null;
	private AudioStreamPlayer _audioSwitch = null;

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
		_nodeInfoSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/InfoOptionContainer/MarginContainer/Button/Panel");
		_nodeInfoOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/InfoOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		//_nodeExitSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/Button/Panel");
		//_nodeExitOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");

		_nodeActionDescriptionMainPanel = GetNode<Panel>("../../../HudContainer/ActionInfo/Panel");
		_nodeActionTitleLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionName");
		_nodeActionEffectLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionEffect");
		_nodeActionDescriptionLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/ActionDescription");
		_audioSelect = GetNode<AudioStreamPlayer>("../Select_AudioStreamPlayer");
		_audioSwitch = GetNode<AudioStreamPlayer>("../Switch_AudioStreamPlayer");

		SelectionHelperInstance = new SelectionHelper();
		SelectionHelperInstance.AddOption(-1, (int)Enumerations.Combat.BasePagePanelOptions.Fight, true, _nodeFightSelectionPanel, _nodeFightOptionLabel);
		SelectionHelperInstance.AddOption(-2, (int)Enumerations.Combat.BasePagePanelOptions.Chat, false, _nodeChatSelectionPanel, _nodeChatOptionLabel, true);
		SelectionHelperInstance.AddOption(-3, (int)Enumerations.Combat.BasePagePanelOptions.Info, false, _nodeInfoSelectionPanel, _nodeInfoOptionLabel);
		//SelectionHelperInstance.AddOption(-4, (int)Enumerations.Combat.BasePagePanelOptions.Exit, false, _nodeExitSelectionPanel, _nodeExitOptionLabel);
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

		if (!_nodeSelf.Visible)
		{
			_nodeSelf.Visible = true;
		}
		else
		{
			if (Input.IsActionJustPressed(_INTERACT_INPUT))
			{
				_audioSelect.Play();
				EmitSignal(SignalName.SelectBase, SelectionHelperInstance.GetSelectedOptionIndex());
			}

			if (Input.IsActionJustPressed(_LEFT_INPUT))
			{
				////GD.Print("Left Input");
				_audioSwitch.Play();
				SelectionHelperInstance.ShiftSelectionLeft();
				ProcessSelection();
			}
			if (Input.IsActionJustPressed(_RIGHT_INPUT))
			{
				////GD.Print("Right Input");
				_audioSwitch.Play();
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
				if (option.IsDisabled)
				{
					SelectionHelperInstance.ApplyDisabledPageLabelSettingOption(option.OptionLabel);
					SelectionHelperInstance.ApplyDisabledPagePanelOption(option.SelectionPanel);
				}
				else if (option.IsSelected)
				{
					SelectionHelperInstance.ApplyActivePageLabelSettingOption(option.OptionLabel);
					SelectionHelperInstance.ApplyActivePagePanelOption(option.SelectionPanel);
					var shouldShowActionInfo = SelectionHelperInstance.HandleSelectedOptionDescription(option.Id, _nodeActionTitleLabel, _nodeActionEffectLabel, _nodeActionDescriptionLabel);
					if (shouldShowActionInfo) EmitSignal(SignalName.ShowActionInfo);
					else EmitSignal(SignalName.HideActionInfo);
				}
				else
				{
					SelectionHelperInstance.ApplyInactivePageLabelSettingOption(option.OptionLabel);
					SelectionHelperInstance.ApplyInactivePagePanelOption(option.SelectionPanel);
				}
			}
			catch (Exception exception)
			{
				//////GD.Print($"Exception occured on option id {option.Id}: {exception.Message}");
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
