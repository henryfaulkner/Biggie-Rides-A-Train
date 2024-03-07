using Godot;
using System;

public partial class OptionModel
{
	public OptionModel()
	{
		Id = -1;
		UiId = -1;
		IsSelected = false;
		SelectionPanel = null;
		OptionLabel = null;
	}

	public OptionModel(int id, int uiId, bool isSelected, Panel panel, Label label)
	{
		Id = id;
		UiId = uiId;
		IsSelected = isSelected;
		SelectionPanel = panel;
		OptionLabel = label;
	}

	public int Id { get; set; }
	public int UiId { get; set; }
	public bool IsSelected { get; set; }
	public Panel SelectionPanel { get; set; }
	public Label OptionLabel { get; set; }
}
