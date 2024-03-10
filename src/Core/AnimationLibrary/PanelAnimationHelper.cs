using Godot;
using System;

public class PanelAnimationHelper
{
    private static readonly float _ANIMATION_SPEED = 6f;

    // Translate subjectArea towards targetArea x and y, simulatiously
    // return false until subjectArea's x and y are equal to targetArea's x and y
    public bool TranslateOverTime(double delta, Control subjectArea, Control targetArea)
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
                    subjectArea.Position.X + (translateDirectionX * _ANIMATION_SPEED),
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
                    subjectArea.Position.Y + (translateDirectionY * _ANIMATION_SPEED)
                );
            }
        }

        return CheckPosition(subjectArea, targetArea);
    }

    CenterScaleHorizontal(double delta, Control subjectArea, Control targetArea)
    {

    }

    // Scale subjectArea towards targetArea, scale x first, then scale y
    // return false until subjectArea's width, y, and height are equal to targetArea's x, width, y, and height
    public bool CenterScaleHAndUniScaleYOverTime(double delta, Control subjectArea, Control targetArea)
    {
        // +1 is right, -1 is left 
        int translateDirectionX = subjectArea.Position.X == targetArea.Position.X ? 0 : (subjectArea.Position.X < targetArea.Position.X ? 1 : -1);
        // +1 is down, -1 is up
        int translateDirectionY = subjectArea.Position.Y == targetArea.Position.Y ? 0 : (subjectArea.Position.Y < targetArea.Position.Y ? 1 : -1);
        // +1 is widen, -1 is narrow
        int scaleDirectionX = subjectArea.Size.X == targetArea.Size.X ? 0 : (subjectArea.Size.X < targetArea.Size.X ? 1 : -1);
        // +1 is heighten, -1 is shorten
        int scaleDirectionY = subjectArea.Size.Y == targetArea.Size.Y ? 0 : (subjectArea.Size.Y < targetArea.Size.Y ? 1 : -1);

        var amountToScale = _ANIMATION_SPEED;
        var amountToTranslate = _ANIMATION_SPEED / 2;

        bool checkSizeX = subjectArea.Size.X == targetArea.Size.X;
        bool checkPositionX = subjectArea.Position.X == targetArea.Position.X;
        bool checkSizeY = subjectArea.Size.Y == targetArea.Size.Y;
        bool checkPositionY = subjectArea.Position.Y == targetArea.Position.Y;

        if (!checkSizeX || !checkPositionX)
        {
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
        }
        else
        {
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
                        subjectArea.Position.Y + (translateDirectionY * amountToScale)
                    );
                }
            }
        }

        return CheckPosition(subjectArea, targetArea) && CheckSize(subjectArea, targetArea);
    }

    private bool CheckPosition(Control controlOne, Control controlTwo)
    {
        return controlOne.Position.X == controlTwo.Position.X
            && controlOne.Position.Y == controlTwo.Position.Y;
    }

    private bool CheckSize(Control controlOne, Control controlTwo)
    {
        return controlOne.Size.X == controlTwo.Size.X
            && controlOne.Size.Y == controlTwo.Size.Y;
    }

    private bool CheckWithinSpeedThreshold(float subjectValue, float targetValue)
    {
        return -_ANIMATION_SPEED <= subjectValue - targetValue
            && subjectValue - targetValue <= _ANIMATION_SPEED;
    }
}
