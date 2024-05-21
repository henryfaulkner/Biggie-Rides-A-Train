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

	public ISelectionHelper SelectionHelperInstance { get; set; }
	public bool IsTargeting { get; set; }

	public override void _Ready()
	{
		_serviceCombat = GetNode<CombatSingleton>("/root/CombatSingleton");
		_audioSelect = GetNode<AudioStreamPlayer>("../Select_AudioStreamPlayer");
		_audioSwitch = GetNode<AudioStreamPlayer>("../Switch_AudioStreamPlayer");

		SelectionHelperInstance = new TargetSelectionHelper();
		foreach ((EnemyTarget target, int i) in _serviceCombat.EnemyTargetList.Select((value, i) => (value, i)))
		{
			SelectionHelperInstance.AddOption(-1, i, true, _nodeFightSelectionPanel, _nodeFightOptionLabel);
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

	}
}

public class TargetSelectionHelper : ISelectionHelper
{

}

public interface ISelectionHelper
{
	public List<OptionModel> OptionList { get; set; }
	public void InstantiateSelectionStyles();
	public void AddOption(int id, int uiId, bool isSelected, Panel panel, Label label);
	public void ShiftSelectionLeft();
	public void ShiftSelectionRight();
	public int GetSelectedOptionId();
	public void ApplyActivePagePanelOption(Panel panel);
	public void ApplyInactivePagePanelOption(Panel panel);
	public void ApplyActivePageLabelSettingOption(Label label);
	public void ApplyInactivePageLabelSettingOption(Label label);
	public void Reset();
}