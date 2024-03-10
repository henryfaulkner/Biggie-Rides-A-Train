using Godot;
using System;

public class PanelAnimationHelper
{
	public PanelAnimationHelper() { }
	public PanelAnimationHelper(float animationSpeed)
	{
		AnimationSpeed = animationSpeed;
	}

	public float AnimationSpeed { get; set; }

	// Translate subjectArea towards targetArea x and y, simulatiously
	// return false until subjectArea's x and y are equal to targetArea's x and y
	public bool TranslateOverTime(Control subjectArea, Control targetArea, double delta = 0.0)
	{
		// +1 is right, -1 is left 
		int translateDirectionX = subjectArea.Position.X == targetArea.Position.X ? 0 : (subjectArea.Position.X < targetArea.Position.X ? 1 : -1);
		// +1 is down, -1 is up
		int translateDirectionY = subjectArea.Position.Y == targetArea.Position.Y ? 0 : (subjectArea.Position.Y < targetArea.Position.Y ? 1 : -1);

		if (subjectArea.Position.X != targetArea.Position.X)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Position.X, targetArea.Position.X))
			{
				subjectArea.Position = new Vector2(targetArea.Position.X, subjectArea.Position.Y);
			}
			else
			{
				subjectArea.Position = new Vector2(
					subjectArea.Position.X + (translateDirectionX * AnimationSpeed),
					subjectArea.Position.Y
				);
			}
		}

		if (subjectArea.Position.Y != targetArea.Position.Y)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Position.Y, targetArea.Position.Y))
			{
				subjectArea.Position = new Vector2(subjectArea.Position.X, targetArea.Position.Y);
			}
			else
			{
				subjectArea.Position = new Vector2(
					subjectArea.Position.X,
					subjectArea.Position.Y + (translateDirectionY * AnimationSpeed)
				);
			}
		}

		return CheckPosition(subjectArea, targetArea);
	}


	// Scale subjectArea towards targetArea, scale x first, then scale y
	// return false until subjectArea's width, y, and height are equal to targetArea's x, width, y, and height

	public bool CenterScaleX(Control subjectArea, Control targetArea, double delta = 0.0)
	{
		// +1 is right, -1 is left 
		int translateDirectionX = subjectArea.Position.X == targetArea.Position.X ? 0 : (subjectArea.Position.X < targetArea.Position.X ? 1 : -1);
		// +1 is widen, -1 is narrow
		int scaleDirectionX = subjectArea.Size.X == targetArea.Size.X ? 0 : (subjectArea.Size.X < targetArea.Size.X ? 1 : -1);

		var amountToScale = AnimationSpeed;
		var amountToTranslate = AnimationSpeed / 2;

		bool checkSizeX = subjectArea.Size.X == targetArea.Size.X;
		bool checkPositionX = subjectArea.Position.X == targetArea.Position.X;

		if (!checkSizeX)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Size.X, targetArea.Size.X))
			{
				subjectArea.Size = new Vector2(targetArea.Size.X, subjectArea.Size.Y);
			}
			else
			{
				subjectArea.Size = new Vector2(
					subjectArea.Size.X + (scaleDirectionX * amountToScale),
					subjectArea.Size.Y
				);
			}
		}
		if (!checkPositionX)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Position.X, targetArea.Position.X))
			{
				subjectArea.Position = new Vector2(targetArea.Position.X, subjectArea.Position.Y);
			}
			else
			{
				subjectArea.Position = new Vector2(
					subjectArea.Position.X + (translateDirectionX * amountToTranslate),
					subjectArea.Position.Y
				);
			}
		}

		return CheckPositionX(subjectArea, targetArea) && CheckSizeX(subjectArea, targetArea);
	}

	public bool CenterScaleY(Control subjectArea, Control targetArea, double delta = 0.0)
	{
		// +1 is down, -1 is up
		int translateDirectionY = subjectArea.Position.Y == targetArea.Position.Y ? 0 : (subjectArea.Position.Y < targetArea.Position.Y ? 1 : -1);
		// +1 is heighten, -1 is shorten
		int scaleDirectionY = subjectArea.Size.Y == targetArea.Size.Y ? 0 : (subjectArea.Size.Y < targetArea.Size.Y ? 1 : -1);
		var amountToScale = AnimationSpeed;
		var amountToTranslate = AnimationSpeed / 2;
		bool checkSizeY = subjectArea.Size.Y == targetArea.Size.Y;
		bool checkPositionY = subjectArea.Position.Y == targetArea.Position.Y;

		if (!checkSizeY)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Size.Y, targetArea.Size.Y))
			{
				subjectArea.Size = new Vector2(subjectArea.Size.X, targetArea.Size.Y);
			}
			else
			{
				subjectArea.Size = new Vector2(
					subjectArea.Size.X,
					subjectArea.Size.Y + (scaleDirectionY * amountToScale)
				);
			}
		}
		if (!checkPositionY)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Position.Y, targetArea.Position.Y))
			{
				subjectArea.Position = new Vector2(subjectArea.Position.X, targetArea.Position.Y);
			}
			else
			{
				subjectArea.Position = new Vector2(
					subjectArea.Position.X,
					subjectArea.Position.Y + (translateDirectionY * amountToTranslate)
				);
			}
		}

		return CheckPositionY(subjectArea, targetArea) && CheckSizeY(subjectArea, targetArea);
	}

	public bool MonoScaleX(Control subjectArea, Control targetArea, double delta = 0.0)
	{
		// +1 is right, -1 is left 
		int translateDirectionX = subjectArea.Position.X == targetArea.Position.X ? 0 : (subjectArea.Position.X < targetArea.Position.X ? 1 : -1);
		// +1 is widen, -1 is narrow
		int scaleDirectionX = subjectArea.Size.X == targetArea.Size.X ? 0 : (subjectArea.Size.X < targetArea.Size.X ? 1 : -1);

		var amountToScale = AnimationSpeed;
		var amountToTranslate = AnimationSpeed;

		bool checkSizeX = subjectArea.Size.X == targetArea.Size.X;
		bool checkPositionX = subjectArea.Position.X == targetArea.Position.X;

		if (!checkSizeX)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Size.X, targetArea.Size.X))
			{
				subjectArea.Size = new Vector2(targetArea.Size.X, subjectArea.Size.Y);
			}
			else
			{
				subjectArea.Size = new Vector2(
					subjectArea.Size.X + (scaleDirectionX * amountToScale),
					subjectArea.Size.Y
				);
			}
		}
		if (!checkPositionX)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Position.X, targetArea.Position.X))
			{
				subjectArea.Position = new Vector2(targetArea.Position.X, subjectArea.Position.Y);
			}
			else
			{
				subjectArea.Position = new Vector2(
					subjectArea.Position.X + (translateDirectionX * amountToTranslate),
					subjectArea.Position.Y
				);
			}
		}

		return CheckPositionX(subjectArea, targetArea) && CheckSizeX(subjectArea, targetArea);
	}

	public bool MonoScaleY(Control subjectArea, Control targetArea, double delta = 0.0)
	{
		// +1 is down, -1 is up
		int translateDirectionY = subjectArea.Position.Y == targetArea.Position.Y ? 0 : (subjectArea.Position.Y < targetArea.Position.Y ? 1 : -1);
		// +1 is heighten, -1 is shorten
		int scaleDirectionY = subjectArea.Size.Y == targetArea.Size.Y ? 0 : (subjectArea.Size.Y < targetArea.Size.Y ? 1 : -1);
		var amountToScale = AnimationSpeed;
		var amountToTranslate = AnimationSpeed;
		bool checkSizeY = subjectArea.Size.Y == targetArea.Size.Y;
		bool checkPositionY = subjectArea.Position.Y == targetArea.Position.Y;

		if (!checkSizeY)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Size.Y, targetArea.Size.Y))
			{
				subjectArea.Size = new Vector2(subjectArea.Size.X, targetArea.Size.Y);
			}
			else
			{
				subjectArea.Size = new Vector2(
					subjectArea.Size.X,
					subjectArea.Size.Y + (scaleDirectionY * amountToScale)
				);
			}
		}
		if (!checkPositionY)
		{
			if (CheckWithinSpeedThreshold(subjectArea.Position.Y, targetArea.Position.Y))
			{
				subjectArea.Position = new Vector2(subjectArea.Position.X, targetArea.Position.Y);
			}
			else
			{
				subjectArea.Position = new Vector2(
					subjectArea.Position.X,
					subjectArea.Position.Y + (translateDirectionY * amountToTranslate)
				);
			}
		}

		return CheckPositionY(subjectArea, targetArea) && CheckSizeY(subjectArea, targetArea);
	}


	public bool CheckPosition(Control controlOne, Control controlTwo)
	{
		return CheckPositionX(controlOne, controlTwo)
			&& CheckPositionY(controlOne, controlTwo);
	}

	public bool CheckSize(Control controlOne, Control controlTwo)
	{
		return CheckSizeX(controlOne, controlTwo)
			&& CheckSizeY(controlOne, controlTwo);
	}

	private bool CheckWithinSpeedThreshold(float subjectValue, float targetValue)
	{
		return -AnimationSpeed <= subjectValue - targetValue
			&& subjectValue - targetValue <= AnimationSpeed;
	}

	public bool CheckPositionX(Control controlOne, Control controlTwo)
	{
		return controlOne.Position.X == controlTwo.Position.X;
	}

	public bool CheckPositionY(Control controlOne, Control controlTwo)
	{
		return controlOne.Position.Y == controlTwo.Position.Y;
	}

	public bool CheckSizeX(Control controlOne, Control controlTwo)
	{
		return controlOne.Size.X == controlTwo.Size.X;
	}

	public bool CheckSizeY(Control controlOne, Control controlTwo)
	{
		return controlOne.Size.Y == controlTwo.Size.Y;
	}
}
