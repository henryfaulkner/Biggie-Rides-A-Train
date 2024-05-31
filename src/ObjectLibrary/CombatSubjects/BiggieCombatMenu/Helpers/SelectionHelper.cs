using Godot;
using System;
using System.Collections.Generic;

public class SelectionHelper : ISelectionHelper
{

	private static readonly StringName _STYLEBOX_NAME = new StringName("panel");
	private static readonly StringName _ACTIVE_PAGE_LABEL_SETTING_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Active_PageLabelSettingOption.tres");
	private static readonly StringName _INACTIVE_PAGE_LABEL_SETTING_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Inactive_PageLabelSettingOption.tres");
	private static readonly StringName _ACTIVE_PAGE_PANEL_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Active_PagePanelOption.tres");
	private static readonly StringName _INACTIVE_PAGE_PANEL_OPTION = new StringName("res://ObjectLibrary/CombatSubjects/BiggieCombatMenu/PageStyles/Inactive_PagePanelOption.tres");

	private LabelSettings _styleActivePageLabelSettingOption = null;
	private LabelSettings _styleInactivePageLabelSettingOption = null;
	private StyleBoxFlat _styleActivePagePanelOption = null;
	private StyleBoxFlat _styleInactivePagePanelOption = null;

	public SelectionHelper()
	{
		GD.Print("SelectionHelper!!!!");
		OptionList = new List<OptionModel>();
		CurrentSelectedOptionIndex = 0;
		InitializeStyles();
	}

	private void InitializeStyles()
	{
		GD.Print("SelectionHelper InstantiateSelectionStyles!!!");
		_styleActivePageLabelSettingOption = GD.Load<LabelSettings>(_ACTIVE_PAGE_LABEL_SETTING_OPTION);
		_styleInactivePageLabelSettingOption = GD.Load<LabelSettings>(_INACTIVE_PAGE_LABEL_SETTING_OPTION);
		_styleActivePagePanelOption = GD.Load<StyleBoxFlat>(_ACTIVE_PAGE_PANEL_OPTION);
		_styleInactivePagePanelOption = GD.Load<StyleBoxFlat>(_INACTIVE_PAGE_PANEL_OPTION);
	}

	public List<OptionModel> OptionList { get; set; }
	protected int CurrentSelectedOptionIndex { get; set; }

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
		return OptionList[CurrentSelectedOptionIndex].Id;
	}

	public int GetSelectedOptionIndex()
	{
		return CurrentSelectedOptionIndex;
	}

	public void ApplyActivePagePanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, _styleActivePagePanelOption);
	}

	public void ApplyInactivePagePanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, _styleInactivePagePanelOption);
	}

	public void ApplyActivePageLabelSettingOption(Label label)
	{
		label.LabelSettings = _styleActivePageLabelSettingOption;
	}

	public void ApplyInactivePageLabelSettingOption(Label label)
	{
		label.LabelSettings = _styleInactivePageLabelSettingOption;
	}

	public bool HandleSelectedOptionDescription(int combatOptionId, Label titleLabel, Label subtitleLabel, Label desciptionLabel)
	{
		CombatOption combatOption = new CombatOption(combatOptionId);
		if (!string.IsNullOrEmpty(combatOption.Name) || !string.IsNullOrEmpty(combatOption.Effect) || !string.IsNullOrEmpty(combatOption.Description))
		{
			titleLabel.Text = combatOption.Name;
			subtitleLabel.Text = combatOption.Effect;
			desciptionLabel.Text = combatOption.Description;
			return true;
		}
		return false;
	}

	public virtual void Reset()
	{
		CurrentSelectedOptionIndex = 0;
		int len = OptionList.Count;
		for (int i = 0; i < len; i += 1)
		{
			if (i == 0)
			{
				OptionList[i].IsSelected = true;
				ApplyActivePagePanelOption(OptionList[i].SelectionPanel);
				ApplyActivePageLabelSettingOption(OptionList[i].OptionLabel);
			}
			else
			{
				OptionList[i].IsSelected = false;
				ApplyInactivePagePanelOption(OptionList[i].SelectionPanel);
				ApplyInactivePageLabelSettingOption(OptionList[i].OptionLabel);
			}
		}
	}
}
