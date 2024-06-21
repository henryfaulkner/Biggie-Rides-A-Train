using System.Linq;
using Godot;
using System.Collections.Generic;

public interface ISelectionHelper
{
	public List<OptionModel> OptionList { get; set; }
	public void AddOption(int id, int uiId, bool isSelected, Panel panel, Label label, bool isDisabled = false);
	public void ShiftSelectionLeft();
	public void ShiftSelectionRight();
	public int GetSelectedOptionIndex();
	public void Reset();
}
