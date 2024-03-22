using Godot;
using System;
using System.Collections.Generic;

public class SelectionHelper
{

	private static readonly StringName _STYLEBOX_NAME = new StringName("panel");

	public SelectionHelper()
	{
		OptionList = new List<OptionModel>();
		CurrentSelectedOptionIndex = 0;
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

	public void HandleSelectedOptionDescription(int combatOptionId, Panel mainPanel, Label titleLabel, Label subtitleLabel, Label desciptionLabel)
	{
		CombatOption combatOption = new CombatOption(combatOptionId);
		mainPanel.Visible = !string.IsNullOrEmpty(combatOption.Name) || !string.IsNullOrEmpty(combatOption.Effect) || !string.IsNullOrEmpty(combatOption.Description);
		//GD.Print($"combatOption.Name {combatOption.Name}");
		//GD.Print($"combatOption.Effect {combatOption.Effect}");
		//GD.Print($"combatOption.Description {combatOption.Description}");
		titleLabel.Text = combatOption.Name;
		subtitleLabel.Text = combatOption.Effect;
		desciptionLabel.Text = combatOption.Description;
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
