using Godot;
using System;

public partial class FightPageBasePanel : Panel
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private Panel _nodeSelf = null;
	private Panel _nodeScratchSelectionPanel = null;
	private Label _nodeScratchOptionLabel = null;
	private Panel _nodeBiteSelectionPanel = null;
	private Label _nodeBiteOptionLabel = null;
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

		// Fight Panel Nodes
		_nodeScratchSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ScratchOptionContainer/MarginContainer/Button/Panel");
		_nodeScratchOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ScratchOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeBiteSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/BiteOptionContainer/MarginContainer/Button/Panel");
		_nodeBiteOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/BiteOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_nodeBackSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/Button/Panel");
		_nodeBackOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");

		_nodeActionDescriptionMainPanel = GetNode<Panel>("../../../HudContainer/ActionInfo/Panel");
		_nodeActionTitleLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionName");
		_nodeActionEffectLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/HBoxContainer/ActionEffect");
		_nodeActionDescriptionLabel = GetNode<Label>("../../../HudContainer/ActionInfo/Panel/MarginContainer/VBoxContainer/ActionDescription");

		SelectionHelperInstance = new SelectionHelper();
		SelectionHelperInstance.AddOption((int)Enumerations.Combat.CombatOptions.Scratch, (int)Enumerations.Combat.FightPagePanelOptions.Scratch, true, _nodeScratchSelectionPanel, _nodeScratchOptionLabel);
		SelectionHelperInstance.AddOption((int)Enumerations.Combat.CombatOptions.Bite, (int)Enumerations.Combat.FightPagePanelOptions.Bite, false, _nodeBiteSelectionPanel, _nodeBiteOptionLabel);
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
				EmitSignal(SignalName.SelectFight, SelectionHelperInstance.GetSelectedOptionId());
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
	public delegate void SelectFightEventHandler(int index);

	public void ProcessSelection()
	{
		foreach (var option in SelectionHelperInstance.OptionList)
		{
			try
			{
				if (option.IsSelected)
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
