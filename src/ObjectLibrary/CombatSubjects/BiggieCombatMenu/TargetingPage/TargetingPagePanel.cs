using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class TargetingPagePanel : Panel
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private AudioStreamPlayer _audioSelect = null;
	private AudioStreamPlayer _audioSwitch = null;

	private CombatSingleton _serviceCombat = null;

	public TargetSelectionHelper SelectionHelperInstance { get; set; }
	public bool IsTargeting { get; set; }

	public override void _Ready()
	{
		_serviceCombat = GetNode<CombatSingleton>("/root/CombatSingleton");
		_audioSelect = GetNode<AudioStreamPlayer>("../Select_AudioStreamPlayer");
		_audioSwitch = GetNode<AudioStreamPlayer>("../Switch_AudioStreamPlayer");

		SelectionHelperInstance = new TargetSelectionHelper();
		foreach ((EnemyTarget target, int i) in _serviceCombat.EnemyTargetList.Select((value, i) => (value, i)))
		{
			SelectionHelperInstance.AddOption(target.Id, target.Id, true, target.TargetPanel, null);
		}
		ProcessSelection();
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (!IsTargeting) return;

		if (Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			_audioSelect.Play();
			EmitSignal(SignalName.SelectTarget, SelectionHelperInstance.GetSelectedOptionId());
		}

		if (Input.IsActionJustPressed(_LEFT_INPUT))
		{
			//GD.Print("Left Input");
			_audioSwitch.Play();
			SelectionHelperInstance.ShiftSelectionLeft();
			ProcessSelection();
		}
		if (Input.IsActionJustPressed(_RIGHT_INPUT))
		{
			//GD.Print("Right Input");
			_audioSwitch.Play();
			SelectionHelperInstance.ShiftSelectionRight();
			ProcessSelection();
		}
	}

	[Signal]
	public delegate void SelectTargetEventHandler();

	public void ProcessSelection()
	{
		foreach (var option in SelectionHelperInstance.OptionList)
		{
			try
			{
				if (option.IsSelected)
				{
					SelectionHelperInstance.ApplyActiveEnemyTargetPanelOption(option.SelectionPanel);
				}
				else
				{
					SelectionHelperInstance.ApplyInactiveEnemyTargetPanelOption(option.SelectionPanel);
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
}
