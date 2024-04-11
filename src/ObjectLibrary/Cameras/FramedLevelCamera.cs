using Godot;
using System;

public partial class FramedLevelCamera : Camera3D
{
	private static readonly int _CAMERA_HYPOTHETICAL_SPAN = 1024;
	private static readonly float _CAMERA_BARRIER_TOLERANCE_RIGHT = -1.0f;
	private static readonly float _CAMERA_BARRIER_TOLERANCE_LEFT = 1.0f;
	private static readonly float _CAMERA_BIGGIE_TOLERANCE = 1.5f;
	private static readonly float _CAMERA_SPEED = .05f;

	private Camera3D _nodeSelf = null;
	private CharacterBody3D _nodeBiggie = null;
	private StaticBody3D _nodeRightMostBarrier = null;
	private StaticBody3D _nodeBottomMostBarrier = null;
	private StaticBody3D _nodeLeftMostBarrier = null;

	private float LeftLimit { get; set; }
	private float RightLimit { get; set; }
	private Vector3 Overshoot { get; set; }
	private int DeltaIndex { get; set; }
	private FramedLevelCameraMovement Movement { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Camera3D>(".");
		_nodeBiggie = GetNode<CharacterBody3D>("../LevelWrapper/TextBoxWrapper/Biggie3D");
		_nodeRightMostBarrier = GetNode<StaticBody3D>("../LevelWrapper/TextBoxWrapper/SceneBorders/RightMostBarrier");
		_nodeBottomMostBarrier = GetNode<StaticBody3D>("../LevelWrapper/TextBoxWrapper/SceneBorders/BottomMostBarrier");
		_nodeLeftMostBarrier = GetNode<StaticBody3D>("../LevelWrapper/TextBoxWrapper/SceneBorders/LeftMostBarrier");

		RightLimit = _nodeRightMostBarrier.Position.X + _CAMERA_BARRIER_TOLERANCE_RIGHT;
		LeftLimit = _nodeLeftMostBarrier.Position.X + _CAMERA_BARRIER_TOLERANCE_LEFT;
		Overshoot = new Vector3(0.25f, 0.0f, 0.0f);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (DeltaIndex % 30 == 0)
		{
			var cameraX = _nodeSelf.Position.X;
			//GD.Print($"cameraX {cameraX}");
			var touchingBarrierRight = CheckBarrierCollision(cameraX, RightLimit);
			GD.Print($"touchingBarrierRight {touchingBarrierRight}");
			var touchingBarrierLeft = CheckBarrierCollision(cameraX, LeftLimit);
			GD.Print($"touchingBarrierLeft {touchingBarrierLeft}");
			if (!touchingBarrierLeft && !touchingBarrierRight)
			{
				// Target is Biggie, Biggie is Target
				Movement = CheckTargetTolerances(cameraX, _nodeBiggie.Position.X, _CAMERA_BIGGIE_TOLERANCE);
			}
		}
		if (Movement.ShouldMove)
		{
			var targetVector = new Vector3(
				_nodeSelf.Position.X + (Movement.DirectionX * _CAMERA_SPEED),
				_nodeSelf.Position.Y,
				_nodeSelf.Position.Z + (Movement.DirectionZ * _CAMERA_SPEED)
			);
			_nodeSelf.Position = LerpOvershootV(_nodeSelf.Position, targetVector, 0.2f, Overshoot);
		}
		DeltaIndex += 1;
		if (DeltaIndex == 1000) DeltaIndex = 0;
	}

	private bool CheckBarrierCollision(float cameraX, float barrierLimit)
	{
		float cameraBarrierLimitSum = cameraX + barrierLimit;
		//GD.Print($"CheckBarrierTolerance, cameraBarrierDiff {cameraBarrierLimitSum}");
		return -_CAMERA_SPEED <= cameraBarrierLimitSum
			&& cameraBarrierLimitSum <= _CAMERA_SPEED;
	}

	public FramedLevelCameraMovement CheckTargetTolerances(float cameraX, float targetX, float tolerance)
	{
		float targetRightLimit = targetX - tolerance;
		float targetLeftLimit = targetX + tolerance;
		bool moveRight = cameraX < targetRightLimit;
		bool moveLeft = cameraX > targetLeftLimit;
		if (moveRight)
		{
			GD.Print("Move right");
			return new FramedLevelCameraMovement(true, FramedLevelCameraMovement.Direction.Right);
		}
		else if (moveLeft)
		{
			GD.Print("Move left");
			return new FramedLevelCameraMovement(true, FramedLevelCameraMovement.Direction.Left);
		}
		else
		{
			GD.Print("Did not map cameraTargetDifference");
			return new FramedLevelCameraMovement();
		}
	}

	// https://www.reddit.com/r/godot/comments/rk6hlz/how_did_you_solve_2d_camera_smoothing/
	public float LerpOvershoot(float origin, float target, float weight, float overshoot)
	{
		var distance = (target - origin) * weight;
		if (IsEqualApprox(distance, 0.0f))
		{
			return target;
		}

		var distanceSign = Mathf.Sign(distance);
		var lerpValue = Mathf.Lerp(origin, target + (overshoot * distanceSign), weight);
		if (distanceSign == 1.0)
		{
			lerpValue = Mathf.Min(lerpValue, target);
		}
		else if (distanceSign == -1.0)
		{
			lerpValue = Mathf.Max(lerpValue, target);
		}
		return lerpValue;
	}

	private Vector3 LerpOvershootV(Vector3 from, Vector3 to, float weight, Vector3 overshoot)
	{
		var x = LerpOvershoot(from.X, to.X, weight, overshoot.X);
		var y = LerpOvershoot(from.Y, to.Y, weight, overshoot.Y);
		var z = LerpOvershoot(from.Z, to.Z, weight, overshoot.Z);
		return new Vector3(x, y, z);
	}

	private bool IsEqualApprox(float a, float b)
	{
		return a > b - float.Epsilon && a < b + float.Epsilon;
	}

	public class FramedLevelCameraMovement
	{
		public FramedLevelCameraMovement()
		{
			ShouldMove = false;
		}
		public FramedLevelCameraMovement(bool shouldMove, Direction movementDirection)
		{
			ShouldMove = shouldMove;
			switch (movementDirection)
			{
				case Direction.Up:
					DirectionX = 0;
					DirectionZ = -1;
					break;
				case Direction.Right:
					DirectionX = 1;
					DirectionZ = 0;
					break;
				case Direction.Down:
					DirectionX = 0;
					DirectionZ = 1;
					break;
				case Direction.Left:
					DirectionX = -1;
					DirectionZ = 0;
					break;
				default:
					DirectionX = 0;
					DirectionZ = 0;
					break;
			}
		}

		public bool ShouldMove { get; set; }
		public int DirectionX { get; set; }
		public int DirectionZ { get; set; }
		public enum Direction
		{
			Up = 0,
			Right = 1,
			Down = 2,
			Left = 3,
		}
	}
}

