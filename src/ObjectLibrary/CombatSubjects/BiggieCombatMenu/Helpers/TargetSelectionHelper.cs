using System.Linq;
using System.Collections.Generic;
using Godot;

public class TargetSelectionHelper : SelectionHelper, ISelectionHelper
{
	private static readonly StringName _STYLEBOX_NAME = new StringName("panel");
	private static readonly StringName _ACTIVE_ENEMY_TARGET_PANEL_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Active_EnemyTargetPanelOption.tres");
	private static readonly StringName _INACTIVE_ENEMY_TARGET_PANEL_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Inactive_EnemyTargetPanelOption.tres.tres");

	private StyleBoxFlat _styleActiveEnemyTargetPanelOption = null;
	private StyleBoxFlat _styleInactiveEnemyTargetPanelOption = null;

	public TargetSelectionHelper() : base()
	{
		LoadStyles();
	}

	public void LoadStyles()
	{
		_styleActiveEnemyTargetPanelOption = GD.Load<StyleBoxFlat>(_ACTIVE_ENEMY_TARGET_PANEL_OPTION);
		_styleInactiveEnemyTargetPanelOption = GD.Load<StyleBoxFlat>(_INACTIVE_ENEMY_TARGET_PANEL_OPTION);
	}

	public void ApplyActiveEnemyTargetPanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, _styleActiveEnemyTargetPanelOption);
	}

	public void ApplyInactiveEnemyTargetPanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, _styleInactiveEnemyTargetPanelOption);
	}

	public override void Reset()
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
