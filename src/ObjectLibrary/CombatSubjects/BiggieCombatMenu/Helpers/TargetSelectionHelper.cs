using System.Linq;
using System.Collections.Generic;
using Godot;

public class TargetSelectionHelper : ISelectionHelper
{
	private static readonly StringName _STYLEBOX_NAME = new StringName("panel");
	private static readonly StringName _ACTIVE_ENEMY_TARGET_PANEL_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Active_EnemyTargetPanelOption.tres");
	private static readonly StringName _INACTIVE_ENEMY_TARGET_PANEL_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Inactive_EnemyTargetPanelOption.tres");

	private StyleBoxFlat _styleActiveEnemyTargetPanelOption = null;
	private StyleBoxFlat _styleInactiveEnemyTargetPanelOption = null;

	public List<OptionModel> OptionList { get; set; }
	private int CurrentSelectedOptionIndex { get; set; }

	public TargetSelectionHelper()
	{
		OptionList = new List<OptionModel>();
		CurrentSelectedOptionIndex = 0;
		InstantiateSelectionStyles();
	}

	public void InstantiateSelectionStyles()
	{
		_styleActiveEnemyTargetPanelOption = GD.Load<StyleBoxFlat>(_ACTIVE_ENEMY_TARGET_PANEL_OPTION);
		_styleInactiveEnemyTargetPanelOption = GD.Load<StyleBoxFlat>(_INACTIVE_ENEMY_TARGET_PANEL_OPTION);
	}

	public void AddOption(int id, int uiId, bool isSelected, Panel panel, Label label)
	{
		var option = new OptionModel(id, uiId, isSelected, panel, label);
		OptionList.Add(option);
	}

	public void ShiftSelectionLeft()
	{
		int len = OptionList.Count;
		if (len == 0)
		{
			//GD.Print("Error: OptionList is empty");
			return;
		}
		if (len == 1) return;

		OptionList[CurrentSelectedOptionIndex].IsSelected = false;
		if (CurrentSelectedOptionIndex == 0)
		{
			CurrentSelectedOptionIndex = len - 1;
		}
		else
		{
			CurrentSelectedOptionIndex -= 1;
		}
		OptionList[CurrentSelectedOptionIndex].IsSelected = true;
		return;
	}

	public void ShiftSelectionRight()
	{
		int len = OptionList.Count;
		if (len == 0)
		{
			//GD.Print("Error: OptionList is empty");
			return;
		}
		if (len == 1) return;

		OptionList[CurrentSelectedOptionIndex].IsSelected = false;

		if (CurrentSelectedOptionIndex == (len - 1))
		{
			CurrentSelectedOptionIndex = 0;
		}
		else
		{
			CurrentSelectedOptionIndex += 1;
		}
		OptionList[CurrentSelectedOptionIndex].IsSelected = true;
		return;
	}

	public int GetSelectedOptionId()
	{
		return CurrentSelectedOptionIndex;
	}

	public void ApplyActiveEnemyTargetPanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, _styleActiveEnemyTargetPanelOption);
	}

	public void ApplyInactiveEnemyTargetPanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, _styleInactiveEnemyTargetPanelOption);
	}

	public void Reset()
	{
		CurrentSelectedOptionIndex = 0;
		int len = OptionList.Count;
		for (int i = 0; i < len; i += 1)
		{
			if (i == 0)
			{
				OptionList[i].IsSelected = true;
				ApplyActiveEnemyTargetPanelOption(OptionList[i].SelectionPanel);
			}
			else
			{
				OptionList[i].IsSelected = false;
				ApplyInactiveEnemyTargetPanelOption(OptionList[i].SelectionPanel);
			}
		}
	}
}
