using Godot;
using System;
using System.Collections.Generic;

public class SelectionHelper
{
	public SelectionHelper()
	{
		OptionList = new List<OptionViewModel>();
		CurrentSelectedOptionIndex = 0;
	}
	
	public List<OptionViewModel> OptionList { get; set; }
	private int CurrentSelectedOptionIndex { get; set; }
	
	public void AddOption(int optionId)
	{
		var option = new OptionViewModel();
		option.Id = optionId;
		OptionList.Add(option);
	}
	
	public void ShiftSelectionLeft()
	{
		int len = OptionList.Count;
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
}
