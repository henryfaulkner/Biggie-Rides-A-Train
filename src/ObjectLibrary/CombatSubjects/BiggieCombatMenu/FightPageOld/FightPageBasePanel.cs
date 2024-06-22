using Godot;
using System;

public partial class FightPageBasePanelOld : Panel
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private Panel _nodeSelf = null;
	private Panel _nodeAttackSelectionPanel = null;
	private Label _nodeAttackOptionLabel = null;
	private Panel _nodeChatSelectionPanel = null;
	private Label _nodeChatOptionLabel = null;
	private Panel _nodeBackSelectionPanel = null;
	private Label _nodeBackOptionLabel = null;

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

		// Fight Panel Nodes
		_nodeAttackSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/AttackOptionContainer/MarginContainer/Button/Panel");
		_nodeAttackOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/AttackOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeChatSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ChatOptionContainer/MarginContainer/Button/Panel");
		_nodeChatOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ChatOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeBackSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/Button/Panel");
		_nodeBackOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");

		_nodeActionDescriptionMainPanel = GetNode<Panel>("../../../HudContainer/ActionInfo/Panel");
		_nodeActionTitleLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionName");
		_nodeActionEffectLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionEffect");
		_nodeActionDescriptionLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/ActionDescription");
		_audioSelect = GetNode<AudioStreamPlayer>("../Select_AudioStreamPlayer");
		_audioSwitch = GetNode<AudioStreamPlayer>("../Switch_AudioStreamPlayer");

		SelectionHelperInstance = new SelectionHelper();
		SelectionHelperInstance.AddOption((int)Enumerations.Combat.CombatOptions.Attack, (int)Enumerations.Combat.FightPagePanelOptions.Attack, true, _nodeAttackSelectionPanel, _nodeAttackOptionLabel);
		SelectionHelperInstance.AddOption((int)Enumerations.Combat.CombatOptions.Chat, (int)Enumerations.Combat.FightPagePanelOptions.Chat, false, _nodeChatSelectionPanel, _nodeChatOptionLabel);
		SelectionHelperInstance.AddOption(-1, (int)Enumerations.Combat.FightPagePanelOptions.Back, false, _nodeBackSelectionPanel, _nodeBackOptionLabel);

	}

	public override void _PhysicsProcess(double delta)
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
				EmitSignal(SignalName.SelectFight, SelectionHelperInstance.GetSelectedOptionIndex());
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
	public delegate void SelectFightEventHandler(int index);

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
