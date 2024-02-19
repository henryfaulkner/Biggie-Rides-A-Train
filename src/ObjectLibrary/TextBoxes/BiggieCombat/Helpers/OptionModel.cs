using Godot;
using System;

public partial class OptionModel
{
	public OptionModel()
	{
		Id = -1;
		IsSelected = false;
		SelectionPanel = null;
		OptionLabel = null;
	}
	
	public OptionModel(int id, bool isSelected, Panel panel, Label label)
	{
		Id = id;
		IsSelected = isSelected;
		SelectionPanel = panel;
		OptionLabel = label;
	}
	
	public int Id { get; set; }
	public bool IsSelected { get; set; }
	public Panel SelectionPanel { get; set; }
	public Label OptionLabel { get; set; }
}
