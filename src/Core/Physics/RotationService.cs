using System;
using Godot;

public partial class RotationService : Node
{
	public Vector3 CurrentNormalizedVector { get; set; }

	public Enumerations.Physics.Rotations CurrentRotation { get; private set; }
	private Vector3 DefaultMovementNoralizedVector => new Vector3(1, 0, 1);
	private Vector3 ForwardMovementNormalizedVector => new Vector3(1, 1, 0);

	private GravityService _serviceGravity = null;

	public RotationService()
	{
		CurrentNormalizedVector = DefaultMovementNoralizedVector;
		CurrentRotation = Enumerations.Physics.Rotations.Default;
	}

	public void RotateToDefault()
	{
		CurrentNormalizedVector = DefaultMovementNoralizedVector;
		CurrentRotation = Enumerations.Physics.Rotations.Default;
	}

	public void RotateToForward()
	{
		CurrentNormalizedVector = ForwardMovementNormalizedVector;
		CurrentRotation = Enumerations.Physics.Rotations.Forward;
	}

	public Vector3 ApplyUpDirection(Vector3 directionVector, float speedRatio)
	{
		switch (CurrentRotation)
		{
			case Enumerations.Physics.Rotations.Default:
				directionVector.Z = -speedRatio;
				break;
			case Enumerations.Physics.Rotations.Forward:
				directionVector.Y = speedRatio;
				break;
			default:
				GD.Print("RotationService CurrentRotation could not be mapped.");
				break;
		}
		return directionVector;
	}

	public Vector3 ApplyRightDirection(Vector3 directionVector, float speedRatio)
	{
		switch (CurrentRotation)
		{
			case Enumerations.Physics.Rotations.Default:
			case Enumerations.Physics.Rotations.Forward:
				directionVector.X = speedRatio;
				break;
			default:
				GD.Print("RotationService CurrentRotation could not be mapped.");
				break;
		}
		return directionVector;
	}

	public Vector3 ApplyDownDirection(Vector3 directionVector, float speedRatio)
	{
		switch (CurrentRotation)
		{
			case Enumerations.Physics.Rotations.Default:
				directionVector.Z = speedRatio;
				break;
			case Enumerations.Physics.Rotations.Forward:
				directionVector.Y = -speedRatio;
				break;
			default:
				GD.Print("RotationService CurrentRotation could not be mapped.");
				break;
		}
		return directionVector;
	}

	public Vector3 ApplyLeftDirection(Vector3 directionVector, float speedRatio)
	{
		switch (CurrentRotation)
		{
			case Enumerations.Physics.Rotations.Default:
			case Enumerations.Physics.Rotations.Forward:
				directionVector.X = -speedRatio;
				break;
			default:
				GD.Print("RotationService CurrentRotation could not be mapped.");
				break;
		}
		return directionVector;
	}
}

