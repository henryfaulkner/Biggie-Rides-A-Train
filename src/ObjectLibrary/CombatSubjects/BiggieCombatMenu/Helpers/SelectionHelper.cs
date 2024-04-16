using Godot;
using System;
using System.Collections.Generic;

public class SelectionHelper
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
		OptionList = new List<OptionModel>();
		CurrentSelectedOptionIndex = 0;
		InstantiateSelectionStyles();
	}

	public void InstantiateSelectionStyles()
	{
		_styleActivePageLabelSettingOption = GD.Load<LabelSettings>(_ACTIVE_PAGE_LABEL_SETTING_OPTION);
		_styleInactivePageLabelSettingOption = GD.Load<LabelSettings>(_INACTIVE_PAGE_LABEL_SETTING_OPTION);
		_styleActivePagePanelOption = GD.Load<StyleBoxFlat>(_ACTIVE_PAGE_PANEL_OPTION);
		_styleInactivePagePanelOption = GD.Load<StyleBoxFlat>(_INACTIVE_PAGE_PANEL_OPTION);
	}

	public List<OptionModel> OptionList { get; set; }
	private int CurrentSelectedOptionIndex { get; set; }

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

	public void AddSelectBorder(Panel panel)
	{
		if (panel.HasThemeStylebox(_STYLEBOX_NAME))
		{
			StyleBoxFlat newStyleboxNormal = panel.GetThemeStylebox(_STYLEBOX_NAME).Duplicate() as StyleBoxFlat;
			newStyleboxNormal.BorderWidthLeft = 2;
			newStyleboxNormal.BorderWidthTop = 2;
			newStyleboxNormal.BorderWidthRight = 2;
			newStyleboxNormal.BorderWidthBottom = 2;
			panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, newStyleboxNormal);
		}
	}

	public void RemoveSelectBorder(Panel panel)
	{
		if (panel.HasThemeStylebox(_STYLEBOX_NAME))
		{
			StyleBoxFlat newStyleboxNormal = panel.GetThemeStylebox(_STYLEBOX_NAME).Duplicate() as StyleBoxFlat;
			newStyleboxNormal.BorderWidthLeft = 0;
			newStyleboxNormal.BorderWidthTop = 0;
			newStyleboxNormal.BorderWidthRight = 0;
			newStyleboxNormal.BorderWidthBottom = 0;
			panel.AddThemeStyleboxOverride(_STYLEBOX_NAME, newStyleboxNormal);
		}
	}

	public void AddWhiteFont(Label label)
	{
		if (label.LabelSettings != null)
		{
			var newLabelSettings = label.LabelSettings.Duplicate() as LabelSettings;
			newLabelSettings.FontColor = Colors.White;
			label.LabelSettings = newLabelSettings;
		}
		else
		{
			////GD.Print("LabelSettings are null. White FontColor was not applied");
		}
	}

	public void AddGreyFont(Label label)
	{
		if (label.LabelSettings != null)
		{
			var newLabelSettings = label.LabelSettings.Duplicate() as LabelSettings;
			newLabelSettings.FontColor = new Color(0x707070ff);
			label.LabelSettings = newLabelSettings;
		}
		else
		{
			////GD.Print("LabelSettings are null. Grey FontColor was not applied");
		}
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
		GD.Print("ApplyInactivePageLabelSettingOption");
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

	public void Reset()
	{
		CurrentSelectedOptionIndex = 0;
		int len = OptionList.Count;
		for (int i = 0; i < len; i += 1)
		{
			if (i == 0)
			{
				OptionList[i].IsSelected = true;
				AddSelectBorder(OptionList[i].SelectionPanel);
				AddWhiteFont(OptionList[i].OptionLabel);
			}
			else
			{
				OptionList[i].IsSelected = false;
				RemoveSelectBorder(OptionList[i].SelectionPanel);
				AddGreyFont(OptionList[i].OptionLabel);
			}
		}
	}
}
