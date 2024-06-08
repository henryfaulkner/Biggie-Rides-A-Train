using Godot;
using System;

public class FramedLevelCameraMovement
{
	public FramedLevelCameraMovement()
	{
		ShouldMove = false;
		Target = Vector3.Zero;
	}
	public FramedLevelCameraMovement(bool shouldMove, Vector3 target, Enumerations.Cameras.Direction movementDirection)
	{
		ShouldMove = shouldMove;
		Target = target;
		switch (movementDirection)
		{
			case Enumerations.Cameras.Direction.Up:
				DirectionX = 0;
				DirectionZ = -1;
				break;
			case Enumerations.Cameras.Direction.Right:
				DirectionX = 1;
				DirectionZ = 0;
				break;
			case Enumerations.Cameras.Direction.Down:
				DirectionX = 0;
				DirectionZ = 1;
				break;
			case Enumerations.Cameras.Direction.Left:
				DirectionX = -1;
				DirectionZ = 0;
				break;
			case Enumerations.Cameras.Direction.UpRight:
				DirectionX = 1;
				DirectionZ = -1;
				break;
			case Enumerations.Cameras.Direction.DownRight:
				DirectionX = 1;
				DirectionZ = 1;
				break;
			case Enumerations.Cameras.Direction.DownLeft:
				DirectionX = -1;
				DirectionZ = 1;
				break;
			case Enumerations.Cameras.Direction.UpLeft:
				DirectionX = -1;
				DirectionZ = -1;
				break;
			default:
				DirectionX = 0;
				DirectionZ = 0;
				break;
		}
	}

	public bool ShouldMove { get; set; }
	public Vector3 Target { get; set; }
	public int DirectionX { get; set; }
	public int DirectionZ { get; set; }
}
