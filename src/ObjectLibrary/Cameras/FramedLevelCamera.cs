using Godot;
using System;

public partial class FramedLevelCamera : Camera3D
{
	private static readonly int _CAMERA_HYPOTHETICAL_SPAN = 1024;
	private static readonly int _CAMERA_BARRIER_TOLERANCE_RIGHT = -512;
	private static readonly int _CAMERA_BARRIER_TOLERANCE_LEFT = 512;
	private static readonly int _CAMERA_BIGGIE_TOLERANCE = 384;
	private static readonly float _CAMERA_SPEED = 5.0f;

	private Camera3D _nodeSelf = null;
	private CharacterBody3D _nodeBiggie = null;
	private StaticBody3D _nodeRightMostBarrier = null;
	private StaticBody3D _nodeBottomMostBarrier = null;
	private StaticBody3D _nodeLeftMostBarrier = null;

	private double LeftLimit { get; set; }
	private double RightLimit { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Camera3D>(".");
		_nodeBiggie = GetNode<CharacterBody3D>("../LevelWrapper/TextBoxWrapper/Biggie3D");
		_nodeRightMostBarrier = GetNode<StaticBody3D>("../LevelWrapper/TextBoxWrapper/SceneBorders/RightMostBarrier");
		_nodeBottomMostBarrier = GetNode<StaticBody3D>("../LevelWrapper/TextBoxWrapper/SceneBorders/BottomMostBarrier");
		_nodeLeftMostBarrier = GetNode<StaticBody3D>("../LevelWrapper/TextBoxWrapper/SceneBorders/LeftMostBarrier");
	}

	public override void _PhysicsProcess(double delta)
	{
		var cameraX = _nodeSelf.Position.X;
		var touchingBarrierRight = CheckBarrierTolerance(cameraX, _CAMERA_BARRIER_TOLERANCE_RIGHT);
		var touchingBarrierLeft = CheckBarrierTolerance(cameraX, _CAMERA_BARRIER_TOLERANCE_LEFT);
		if (touchingBarrierLeft || touchingBarrierRight)
		{
			// Target is Biggie, Biggie is Target
			var movement = CheckTargetTolerances(cameraX, _nodeBiggie.Position.X, _CAMERA_BIGGIE_TOLERANCE);
			if (movement.ShouldMove)
			{
				_nodeSelf.Position = new Vector3(
					_nodeSelf.Position.X + (movement.DirectionX * _CAMERA_SPEED),
					_nodeSelf.Position.Y,
					_nodeSelf.Position.Z + (movement.DirectionY * _CAMERA_SPEED)
				);
			}
		}
	}

	private bool CheckBarrierTolerance(float cameraX, int tolerance)
	{
		float cameraBarrierSum = cameraX + tolerance;
		return -_CAMERA_SPEED <= cameraBarrierSum
			&& cameraBarrierSum <= _CAMERA_SPEED;
	}

	public FramedLevelCameraMovement CheckTargetTolerances(float cameraX, float targetX, int tolerance)
	{
		float cameraTargetDifferenceX = cameraX - targetX;
		FramedLevelCameraMovement movement;
		switch (cameraTargetDifference)
		{
			case (cameraTargetDifferenceX < )
			default:
		}

		return new FramedLevelCameraMovement();
	}

	public class FramedLevelCameraMovement
	{
		public FramedLevelCameraMovement() { }
		public FramedLevelCameraMovement(bool shouldMove, Direction movementDirection)
		{
			ShouldMove = shouldMove;
			switch (movementDirection)
			{
				case Direction.Up:
					DirectionX = 0;
					DirectionY = -1;
					break;
				case Direction.Right:
					DirectionX = 1;
					DirectionY = 0;
					break;
				case Direction.Down:
					DirectionX = 0;
					DirectionY = 1;
					break;
				case Direction.Left:
					DirectionX = -1;
					DirectionY = 0;
					break;
				default:
					DirectionX = 0;
					DirectionY = 0;
					break;
			}
		}

		public bool ShouldMove { get; set; }
		public int DirectionX { get; set; }
		public int DirectionY { get; set; }
		public enum Direction
		{
			Up = 0,
			Right = 1,
			Down = 2,
			Left = 3,
		}
	}
}

