using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class TargetingPagePanel : Panel
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private Panel _nodeBackSelectionPanel = null;
	private Label _nodeBackOptionLabel = null;

	private AudioStreamPlayer _audioSelect = null;
	private AudioStreamPlayer _audioSwitch = null;

	private CombatSingleton _serviceCombat = null;

	public TargetSelectionHelper SelectionHelperInstance { get; set; }
	public bool IsOpen { get; set; }
	private Enumerations.Combat.CombatOptions CurrentCombatOption { get; set; }
	private bool IsInitialized { get; set; }

	public override void _Ready()
	{
		_nodeBackSelectionPanel = GetNode<Panel>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/Button/Panel");
		_nodeBackOptionLabel = GetNode<Label>("./MarginContainer/OptionContainer/ExitOptionContainer/MarginContainer/HBoxContainer/MarginContainer/Label");
		_audioSelect = GetNode<AudioStreamPlayer>("../Select_AudioStreamPlayer");
		_audioSwitch = GetNode<AudioStreamPlayer>("../Switch_AudioStreamPlayer");
		_serviceCombat = GetNode<CombatSingleton>("/root/CombatSingleton");

		IsInitialized = false;
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (!IsInitialized)
		{
			IsInitialized = InitializeTargetingPanels();
			HideSelectionPanels();
		}

		if (!IsOpen)
		{
			if (Visible) Visible = false;
			return;
		}

		if (!Visible)
		{
			Visible = true;
		}
		else
		{
			if (Input.IsActionJustPressed(_INTERACT_INPUT))
			{
				_audioSelect.Play();
				EmitSignal(SignalName.SelectTarget, (int)CurrentCombatOption, SelectionHelperInstance.GetSelectedOptionId());
			}
			if (Input.IsActionJustPressed(_LEFT_INPUT))
			{
				////GD.Print("Left Input");
				_audioSwitch.Play();
				SelectionHelperInstance.ShiftSelectionLeft();
				ProcessSelection(CurrentCombatOption);
			}
			if (Input.IsActionJustPressed(_RIGHT_INPUT))
			{
				////GD.Print("Right Input");
				_audioSwitch.Play();
				SelectionHelperInstance.ShiftSelectionRight();
				ProcessSelection(CurrentCombatOption);
			}
		}
	}

	public bool InitializeTargetingPanels()
	{
		if (_serviceCombat.EnemyTargetList.Count == 0) return false;
		SelectionHelperInstance = new TargetSelectionHelper(_serviceCombat);
		//GD.Print($"EnemyTarget Count: {_serviceCombat.EnemyTargetList.Count}");
		foreach ((EnemyTarget target, int i) in _serviceCombat.EnemyTargetList.Select((value, i) => (value, i)))
		{
			SelectionHelperInstance.AddOption(target.Id, target.Id, i == 0, target.TargetPanel, null);
		}
		// Add BACK Panel as an option.
		SelectionHelperInstance.AddOption(-1, -1, false, _nodeBackSelectionPanel, _nodeBackOptionLabel);
		ProcessSelection(null);
		return true;
	}

	[Signal]
	public delegate void SelectTargetEventHandler(int combatOption, int enemyTargetIndex);

	public void ProcessSelection(Enumerations.Combat.CombatOptions? combatOption)
	{
		if (combatOption.HasValue) CurrentCombatOption = combatOption.Value;
		foreach (var option in SelectionHelperInstance.OptionList)
		{
			try
			{
				// -1 is the BACK Panel.
				if (option.Id == -1)
				{
					if (option.IsSelected)
					{
						//GD.Print("ApplyActiveBack");
						if (option.OptionLabel != null)
							SelectionHelperInstance.ApplyActivePageLabelSettingOption(option.OptionLabel);
						else
						{
							//GD.Print("Back Label is null");
						}
						if (option.SelectionPanel != null)
							SelectionHelperInstance.ApplyActivePagePanelOption(option.SelectionPanel);
						else
						{
							//GD.Print("Back Panel is null");
						}
					}
					else
					{
						//GD.Print("ApplyInactiveBack");
						if (option.OptionLabel != null)
							SelectionHelperInstance.ApplyInactivePageLabelSettingOption(option.OptionLabel);
						else
						//GD.Print("Back Label is null");
						if (option.SelectionPanel != null)
							SelectionHelperInstance.ApplyInactivePagePanelOption(option.SelectionPanel);
						else
						{
							//GD.Print("Back Panel is null");
						}
					}
				}
				else
				{
					if (option.IsSelected)
					{
						//GD.Print("ApplyActiveEnemyTarget");
						if (option.SelectionPanel != null)
							SelectionHelperInstance.ApplyActiveEnemyTargetPanelOption(option.SelectionPanel);
						else
						{
							//GD.Print($"Enemy Target {option.Id} Panel is null");
						}
					}
					else
					{
						//GD.Print("ApplyInactiveEnemyTarget");
						if (option.SelectionPanel != null)
							SelectionHelperInstance.ApplyInactiveEnemyTargetPanelOption(option.SelectionPanel);
						else
						{
							//GD.Print($"Enemy Target {option.Id} Panel is null");
						}
					}
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

	public void ShowSelectionPanels()
	{
		foreach (var panel in SelectionHelperInstance.OptionList.Select(x => x.SelectionPanel))
		{
			panel.SelfModulate = new Color(
				panel.SelfModulate.R,
				panel.SelfModulate.G,
				panel.SelfModulate.B,
				1.0f
			);
		}
	}

	public void HideSelectionPanels()
	{
		foreach (var panel in SelectionHelperInstance.OptionList.Select(x => x.SelectionPanel))
		{
			panel.SelfModulate = new Color(
				panel.SelfModulate.R,
				panel.SelfModulate.G,
				panel.SelfModulate.B,
				0.0f
			);
		}
	}
}
