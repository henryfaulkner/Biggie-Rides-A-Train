using System;
using Godot;

public class SpecialMeter
{
	public float MaxLevel { get; set; }
	public float CurrentLevel { get; set; }
	public float VisualLevel { get; set; }

	public Label ValueLabel { get; set; }
	public ProgressBar ProgressBar { get; set; }

	public bool IsAtMax()
	{
		return CurrentLevel >= MaxLevel;
	}

	public float GetVisualLevelPercentage()
	{
		return VisualLevel / MaxLevel;
	}

	public void AddToSpecialMeter(float amount)
	{
		if (CurrentLevel + amount < MaxLevel)
		{
			CurrentLevel += amount;
		}
		else
		{
			CurrentLevel = MaxLevel;
		}
		ValueLabel.Text = $"{CurrentLevel}/{MaxLevel}";
		TweenVisualLevelTowardCurrentLevel();
	}

	// https://www.youtube.com/watch?v=fpBOEJXZeYs&t=5s
	public void TweenVisualLevelTowardCurrentLevel()
	{
		//if (CurrentLevel == VisualLevel) return;
		var tween = ProgressBar.GetTree().CreateTween();
		tween.TweenProperty(ProgressBar, "value", CurrentLevel / MaxLevel * 100, 1).SetTrans(Tween.TransitionType.Linear);
	}
}
