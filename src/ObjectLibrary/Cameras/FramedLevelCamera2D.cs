using Godot;
using System;

public partial class FramedLevelCamera2D : Camera3D
{
	private static readonly int _CAMERA_HYPOTHETICAL_SPAN = 1024;
	private static readonly float _CAMERA_BARRIER_TOLERANCE_TOP = 1.0f;
	private static readonly float _CAMERA_BARRIER_TOLERANCE_RIGHT = -1.0f;
	private static readonly float _CAMERA_BARRIER_TOLERANCE_BOTTOM = -1.0f;
	private static readonly float _CAMERA_BARRIER_TOLERANCE_LEFT = 1.0f;
	private static readonly float _CAMERA_BIGGIE_TOLERANCE = 1.5f;
	private static readonly float _CAMERA_SPEED = .05f;

	private CharacterBody3D _nodeBiggie = null;
	private StaticBody3D _nodeTopMostBarrier = null;
	private StaticBody3D _nodeRightMostBarrier = null;
	private StaticBody3D _nodeBottomMostBarrier = null;
	private StaticBody3D _nodeLeftMostBarrier = null;

	private float TopLimit { get; set; }
	private float RightLimit { get; set; }
	private float BottomLimit { get; set; }
	private float LeftLimit { get; set; }
	private Vector3 Overshoot { get; set; }
	private int DeltaIndex { get; set; }
	private FramedLevelCameraMovement Movement { get; set; }

	public override void _Ready()
	{
		_nodeBiggie = GetNode<CharacterBody3D>("../Biggie3D");

		_nodeTopMostBarrier = GetNode<StaticBody3D>("../SceneBorders/TopMostBarrier");
		_nodeRightMostBarrier = GetNode<StaticBody3D>("../SceneBorders/RightMostBarrier");
		_nodeBottomMostBarrier = GetNode<StaticBody3D>("../SceneBorders/BottomMostBarrier");
		_nodeLeftMostBarrier = GetNode<StaticBody3D>("../SceneBorders/LeftMostBarrier");

		RightLimit = _nodeRightMostBarrier.Position.X + _CAMERA_BARRIER_TOLERANCE_RIGHT;
		LeftLimit = _nodeLeftMostBarrier.Position.X + _CAMERA_BARRIER_TOLERANCE_LEFT;
		BottomLimit = _nodeBottomMostBarrier.Position.Z + _CAMERA_BARRIER_TOLERANCE_BOTTOM;
		Overshoot = new Vector3(1f, 0.0f, 0.0f);
		Movement = new FramedLevelCameraMovement();
	}

	public override void _PhysicsProcess(double delta)
	{
		// this logic resets Movement object
		// allowing early stops and disabling overshoot attempt
		if (DeltaIndex % 60 == 0)
		{
			var camera = Position;
			var touchingBarrierRight = CheckBarrierCollision(camera.X, RightLimit);
			var touchingBarrierLeft = CheckBarrierCollision(camera.X, LeftLimit);
			var touchingBarrierBottom = CheckBarrierCollision(camera.Z, BottomLimit);
			Enumerations.Cameras.Direction? xMove = null;
			Enumerations.Cameras.Direction? zMove = null;

			if (!touchingBarrierLeft && !touchingBarrierRight)
			{
				GD.Print($"Is touching barrier left? {touchingBarrierLeft}");
				GD.Print($"Is touching barrier right? {touchingBarrierRight}");

				// Target is Biggie, Biggie is Target
				xMove = CheckTargetTolerancesForXAxis(camera, _nodeBiggie.Position, _CAMERA_BIGGIE_TOLERANCE);
			}

			if (!touchingBarrierBottom)
			{
				GD.Print($"Is touching barrier right? {touchingBarrierBottom}");

				// Praise be the Target
				zMove = CheckTargetTolerancesForZAxis(camera, _nodeBiggie.Position, _CAMERA_BIGGIE_TOLERANCE);
			}

			if (xMove != null && zMove != null)
			{
				var xzMove = SumMovement(xMove.Value, zMove.Value);
				Movement = new FramedLevelCameraMovement(true, _nodeBiggie.Position, xzMove.Value);
				GD.Print($"Should move in the XZ direction. Enum is {xzMove}");
			}
			else if (xMove != null)
			{
				Movement = new FramedLevelCameraMovement(true, _nodeBiggie.Position, xMove.Value);
				GD.Print($"Should move in the X direction. Enum is {xMove}");
			}
			else if (zMove != null)
			{
				Movement = new FramedLevelCameraMovement(true, _nodeBiggie.Position, zMove.Value);
				GD.Print($"Should move in the Z direction. Enum is {zMove}");
			}
			else
			{
				Movement = new FramedLevelCameraMovement();
			}
		}
		if (Movement.ShouldMove)
		{
			var targetVector = new Vector3(
				Position.X + (Movement.DirectionX * _CAMERA_SPEED),
				Position.Y,
				Position.Z + (Movement.DirectionZ * _CAMERA_SPEED)
			);
			Position = LerpOvershootHelper.LerpOvershootV(Position, targetVector, 0.2f, Overshoot);
		}
		DeltaIndex += 1;
		if (DeltaIndex == 1000) DeltaIndex = 0;
	}

	private bool CheckBarrierCollision(float camera, float barrierLimit)
	{
		float cameraBarrierLimitSum = camera + barrierLimit;
		return -_CAMERA_SPEED <= cameraBarrierLimitSum
			&& cameraBarrierLimitSum <= _CAMERA_SPEED;
	}

	private Enumerations.Cameras.Direction? CheckTargetTolerancesForXAxis(Vector3 camera, Vector3 target, float tolerance)
	{
		float targetRightLimit = target.X - tolerance;
		float targetLeftLimit = target.X + tolerance;
		bool moveRight = camera.X < targetRightLimit;
		bool moveLeft = camera.X > targetLeftLimit;
		if (moveRight)
		{
			return Enumerations.Cameras.Direction.Right;
		}
		else if (moveLeft)
		{
			return Enumerations.Cameras.Direction.Left;
		}
		else
		{
			return null;
		}
	}

	private Enumerations.Cameras.Direction? CheckTargetTolerancesForZAxis(Vector3 camera, Vector3 target, float tolerance)
	{
		float biggieCameraZDifference = camera.Z - target.Z;
		float targetBottomLimit = target.Z + tolerance - biggieCameraZDifference - 1;
		float targetTopLimit = target.Z - tolerance;
		bool moveBottom = camera.Z < targetBottomLimit;
		bool moveTop = camera.Z > targetTopLimit;
		if (moveBottom)
		{
			return Enumerations.Cameras.Direction.Down;
		}
		else if (moveTop)
		{
			return Enumerations.Cameras.Direction.Up;
		}
		else
		{
			return null;
		}
	}

	public Enumerations.Cameras.Direction? SumMovement(
		Enumerations.Cameras.Direction moveX,
		Enumerations.Cameras.Direction moveZ)
	{
		if (moveX == Enumerations.Cameras.Direction.Right
			&& moveX == Enumerations.Cameras.Direction.Up)
			return Enumerations.Cameras.Direction.UpRight;

		if (moveX == Enumerations.Cameras.Direction.Right
			&& moveX == Enumerations.Cameras.Direction.Down)
			return Enumerations.Cameras.Direction.DownRight;

		if (moveX == Enumerations.Cameras.Direction.Left
			&& moveX == Enumerations.Cameras.Direction.Down)
			return Enumerations.Cameras.Direction.DownLeft;

		if (moveX == Enumerations.Cameras.Direction.Left
			&& moveX == Enumerations.Cameras.Direction.Up)
			return Enumerations.Cameras.Direction.UpLeft;

		return null;
	}
}
