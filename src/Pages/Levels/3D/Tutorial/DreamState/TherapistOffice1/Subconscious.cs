using Godot;
using System;

public partial class Subconscious : CharacterBody3D
{
	private static readonly StringName _MOVE_LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _MOVE_UP_INPUT = new StringName("move_up");
	private static readonly StringName _MOVE_RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _MOVE_DOWN_INPUT = new StringName("move_down");

	private static readonly int _SPRITE_FRAME_IDLE = 0;
	private static readonly int _SPRITE_FRAME_WALK1 = 1;
	private static readonly int _SPRITE_FRAME_WALK2 = 2;
	private static readonly int _SPRITE_FRAME_WALK1_RIGHT = 3;
	private static readonly int _SPRITE_FRAME_WALK2_RIGHT = 4;
	public static readonly int _SPRITE_FRAME_CHANGE_INTERVAL = 45;
	public static readonly int _SPRITE_WALK_FRAME_LENGTH = 2;
	public static readonly float _BIGGIE_SPEED = 1.4f;
	public static readonly float _BIGGIE_SPEED_X_RATIO = 1.0f;
	public static readonly float _BIGGIE_SPEED_Z_RATIO = 0.7f;

	private Node _nodeBiggieSpriteMeshInstance = null;
	private NavigationAgent3D _nodeNavigationAgent = null;

	[Export]
	public float Speed { get; set; }
	[Export]
	public float Acceleration { get; set; }

	private bool _isMoving = false;
	private bool _canMove = true;
	private int _frameIncrement = 0;
	private Enumerations.Movement.Directions _currentFrameDirection = Enumerations.Movement.Directions.Down;

	public override void _Ready()
	{
		_nodeBiggieSpriteMeshInstance = GetNode("./SpriteMeshInstance");
		_nodeNavigationAgent = GetNode<NavigationAgent3D>("./NavigationAgent3D");
	}

	public override void _PhysicsProcess(double _delta)
	{
	}

	private int ReturnSpriteWalkFrame(int frameIncrement)
	{
		var result = (frameIncrement / _SPRITE_FRAME_CHANGE_INTERVAL) % _SPRITE_WALK_FRAME_LENGTH;
		// Skip Idle sprite
		result += 1;
		// Account for sprite flipping
		if (_currentFrameDirection == Enumerations.Movement.Directions.Left) result += 1;
		return result;
	}

	public bool CanMove()
	{
		return _canMove;
	}

	public void CanMove(bool canMove)
	{
		_canMove = canMove;
	}

	public bool ForceWalk(Marker3D target, double delta)
	{
		//GD.Print("Subconscious ForceWalk");
		CanMove(false);
		var direction = MoveTowardTarget(target, delta);

		if (direction.X > 0) // RIGHT
		{
			_isMoving = true;
			_frameIncrement += 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Left;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (direction.X < 0) // LEFT
		{
			_isMoving = true;
			_frameIncrement -= 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Right;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		if (direction.Z != 0) // UP or DOWN
		{
			_isMoving = true;
			_frameIncrement = 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		// opposite the other conditionals in this function
		bool atTarget = direction.X - 0.5f < 0
			&& direction.X + 0.5f > 0
			&& direction.Z + 0.5f > 0
			&& direction.Z - 0.5f < 0;
		CanMove(atTarget);
		//GD.Print($"Subconscious atTarget: {atTarget}");
		return atTarget;
	}

	private Vector3 MoveTowardTarget(Marker3D target, double delta)
	{
		var direction = new Vector3();

		_nodeNavigationAgent.TargetPosition = target.GlobalPosition;

		direction = _nodeNavigationAgent.GetNextPathPosition() - GlobalPosition;
		direction = direction.Normalized();

		Velocity = Velocity.Lerp(direction * Speed, Acceleration * (float)delta);
		MoveAndSlide();
		return direction;
	}
}
