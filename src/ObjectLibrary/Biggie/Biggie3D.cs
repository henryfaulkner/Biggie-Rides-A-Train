using Godot;
using System;

public partial class Biggie3D : CharacterBody3D
{
	private static readonly float STEPSIZE = 1.0f;

	private static readonly StringName _MOVE_LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _MOVE_UP_INPUT = new StringName("move_up");
	private static readonly StringName _MOVE_RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _MOVE_DOWN_INPUT = new StringName("move_down");

	private static readonly int _SPRITE_FRAME_IDLE = 0;
	private static readonly int _SPRITE_FRAME_WALK1 = 1;
	private static readonly int _SPRITE_FRAME_WALK2 = 2;
	private static readonly int _SPRITE_FRAME_WALK1_LEFT = 3;
	private static readonly int _SPRITE_FRAME_WALK2_LEFT = 4;
	public static readonly int _SPRITE_FRAME_CHANGE_INTERVAL = 45;
	public static readonly int _SPRITE_WALK_FRAME_LENGTH = 2;
	public static readonly float _BIGGIE_SPEED = 1.4f;
	public static readonly float _BIGGIE_SPEED_X_RATIO = 1.0f;
	public static readonly float _BIGGIE_SPEED_Z_RATIO = 0.7f;

	private Biggie3D _nodeSelf = null;
	private Node _nodeBiggieSpriteMeshInstance = null;
	private TextBox _nodeTextBox = null;
	private InteractionTextBox _nodeInteractionTextBox = null;
	private CollisionShape3D _nodeCollider = null;

	private bool _isMoving = false;
	private bool _canMove = true;
	private int _frameIncrement = 0;
	private Enumerations.Movement.Directions _currentFrameDirection = Enumerations.Movement.Directions.Down;

	private RelocationService _serviceRelocation = null;
	private GravityService _serviceGravity = null;
	private RotationService _serviceRotation = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Biggie3D>(".");
		_nodeBiggieSpriteMeshInstance = GetNode("./SpriteMeshInstance");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("../InteractionTextBox");
		_nodeCollider = GetNode<CollisionShape3D>("./CollisionShape3D");

		_serviceRelocation = GetNode<RelocationService>("/root/RelocationService");
		AttemptStoredLocationApplication();
		_serviceGravity = GetNode<GravityService>("/root/GravityService");
		_serviceRotation = GetNode<RotationService>("/root/RotationService");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_canMove
			&& _nodeSelf.IsVisibleInTree()
			&& (_nodeTextBox == null || !_nodeTextBox.IsOpen())
			&& (_nodeInteractionTextBox == null || !_nodeInteractionTextBox.IsOpen))
		{
			ProcessGravity(delta);
			var collision = Movement(delta);
			if (collision != null)
			{
				//GD.Print("Collide");
				StepMovement(GlobalTransform.Origin, Velocity.Normalized() * 10, delta);
				Collide(collision);
			}
		}
	}

	private int ReturnSpriteWalkFrame(int frameIncrement)
	{
		var result = (frameIncrement / _SPRITE_FRAME_CHANGE_INTERVAL) % _SPRITE_WALK_FRAME_LENGTH;
		// Skip Idle sprite
		result += 1;
		// Account for sprite flipping
		if (_currentFrameDirection == Enumerations.Movement.Directions.Right) result += 2;
		return result;
	}

	private KinematicCollision3D Movement(double delta)
	{
		Vector3 inputDirection = new Vector3();

		if (Input.IsActionPressed(_MOVE_LEFT_INPUT))
		{
			if (UsingMirrorControls)
			{
				inputDirection = _serviceRotation.ApplyLeftDirection(inputDirection, -_BIGGIE_SPEED_X_RATIO);
				_currentFrameDirection = Enumerations.Movement.Directions.Left;
			}
			else
			{ 
				inputDirection = _serviceRotation.ApplyRightDirection(inputDirection, -_BIGGIE_SPEED_X_RATIO);
				_currentFrameDirection = Enumerations.Movement.Directions.Right;
			}

			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (Input.IsActionPressed(_MOVE_RIGHT_INPUT))
		{
			if (UsingMirrorControls)
			{
				inputDirection = _serviceRotation.ApplyLeftDirection(inputDirection, _BIGGIE_SPEED_X_RATIO);
				_currentFrameDirection = Enumerations.Movement.Directions.Right;
			}
			else
			{
				inputDirection = _serviceRotation.ApplyRightDirection(inputDirection, _BIGGIE_SPEED_X_RATIO);
				_currentFrameDirection = Enumerations.Movement.Directions.Left;
			}

			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		if (Input.IsActionPressed(_MOVE_DOWN_INPUT))
		{
			if (UsingMirrorControls) inputDirection = _serviceRotation.ApplyUpDirection(inputDirection, _BIGGIE_SPEED_Z_RATIO);
			else inputDirection = _serviceRotation.ApplyDownDirection(inputDirection, _BIGGIE_SPEED_Z_RATIO);

			_isMoving = true;
			_frameIncrement = 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (Input.IsActionPressed(_MOVE_UP_INPUT))
		{
			if (UsingMirrorControls) inputDirection = _serviceRotation.ApplyDownDirection(inputDirection, _BIGGIE_SPEED_Z_RATIO);
			else inputDirection = _serviceRotation.ApplyUpDirection(inputDirection, _BIGGIE_SPEED_Z_RATIO);

			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		if (IsIdle())
		{
			_isMoving = false;
			_frameIncrement = 0;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", _SPRITE_FRAME_IDLE);
		}

		if (inputDirection != Vector3.Zero)
		{
			inputDirection = inputDirection.Normalized();
		}

		Velocity = inputDirection * _BIGGIE_SPEED;
		return MoveAndCollide(Velocity * (float)delta);
	}

	// https://thelowrooms.com/articledir/programming_stepclimbing.php
	public bool StepMovement(Vector3 originalPosition, Vector3 velocity, double delta)
	{
		Vector3 dest;
		Vector3 down;
		Vector3 up;
		TraceBusiness trace = new TraceBusiness();
		AddChild(trace);

		// Get destination position that is one step-size above the intended move
		dest = originalPosition;
		dest[0] += velocity[0] * (float)delta;
		dest[1] += STEPSIZE;
		dest[2] += velocity[2] * (float)delta;

		// 1st Trace: Check for collisions one stepsize above the original position
		up = originalPosition + Vector3.Up * STEPSIZE;
		trace.Standard(originalPosition, up, _nodeCollider.Shape, _nodeSelf.GetRid());
		dest[1] = trace.EndPosition[1];

		// 2nd Trace: Check for collisions one stepsize above the original position
		// and along the intended destination
		trace.Standard(trace.EndPosition, dest, _nodeCollider.Shape, _nodeSelf.GetRid());

		// 3rd Trace: Check for collisions below the stepsize until 
		// level with original position
		down = new Vector3(trace.EndPosition[0], originalPosition[1], trace.EndPosition[2]);
		trace.Standard(trace.EndPosition, down, _nodeCollider.Shape, _nodeSelf.GetRid());

		// Move to trace collision position if step is higher than original position 
		// and not steep
		if (trace.EndPosition[1] > originalPosition[1] && trace.Normal[1] >= 0.7f)
		{
			GlobalPosition = trace.EndPosition;
			return true;
		}
		return false;
	}

	public void Collide(KinematicCollision3D collision)
	{
		if (collision.GetCollider().HasMethod("Hit"))
		{
			//GD.Print("Call Hit");
			collision.GetCollider().Call("Hit");
		}
	}

	public bool CanMove()
	{
		return _canMove;
	}

	public void CanMove(bool canMove)
	{
		_canMove = canMove;
	}

	public bool ForceWalk(Vector3 target, double delta)
	{
		//GD.Print("Biggie ForceWalk");
		CanMove(false);
		Vector3 direction = (target - Position).Normalized();
		Vector3 inputDirection = Vector3.Zero;
		// //GD.Print($"target Position X:{target.X} Y:{target.Y} Z:{target.Z}");
		// //GD.Print($"biggie Position X:{Position.X} Y:{Position.Y} Z:{Position.Z}");

		if (direction.X - 0.5f > 0) // RIGHT
		{
			inputDirection.X = _BIGGIE_SPEED_X_RATIO;
			_isMoving = true;
			_frameIncrement += 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Left;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (direction.X + 0.5f < 0) // LEFT
		{
			inputDirection.X = _BIGGIE_SPEED_X_RATIO;
			_isMoving = true;
			_frameIncrement -= 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Right;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		if (direction.Z + 0.5f < 0) // UP
		{
			inputDirection.Z -= _BIGGIE_SPEED_Z_RATIO;
			_isMoving = true;
			_frameIncrement = 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (direction.Z - 0.5f > 0) // DOWN
		{
			inputDirection.Z += _BIGGIE_SPEED_Z_RATIO;
			_isMoving = true;
			_frameIncrement = 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		Velocity = inputDirection * _BIGGIE_SPEED;
		MoveAndCollide(Velocity * (float)delta);

		// opposite the other conditionals in this function
		bool atTarget = direction.X - 0.5f < 0
			&& direction.X + 0.5f > 0
			&& direction.Z + 0.5f > 0
			&& direction.Z - 0.5f < 0;
		CanMove(atTarget);
		//GD.Print($"Biggie3D atTarget: {atTarget}");
		return atTarget;
	}

	private bool IsIdle()
	{
		return
			!Input.IsActionPressed(_MOVE_UP_INPUT)
			&& !Input.IsActionPressed(_MOVE_RIGHT_INPUT)
			&& !Input.IsActionPressed(_MOVE_DOWN_INPUT)
			&& !Input.IsActionPressed(_MOVE_LEFT_INPUT);
	}

	private bool HittingWorldBorderX(Biggie3D biggie, Vector3 inputDirection)
	{
		return _nodeSelf.Position.X <= 0 && inputDirection.X == -_BIGGIE_SPEED_X_RATIO;
	}

	private bool HittingWorldBorderZ(Biggie3D biggie, Vector3 inputDirection)
	{
		return biggie.Position.Z <= 0 && inputDirection.Z == -_BIGGIE_SPEED_Z_RATIO;
	}

	private void AttemptStoredLocationApplication()
	{
		//////GD.Print("AttemptStoredLocationApplication");
		try
		{
			using (var context = new SaveStateService())
			{
				var contextState = context.Load();
				var storedLocation = contextState.StoredLocation;
				var rs = new RelocationService();
				if (storedLocation != null)
				{
					//////GD.Print($"Biggie applied Position. X: {storedLocation.X}. Y: {storedLocation.Y}");
					Position = new Vector3(storedLocation.X, Position.Y, storedLocation.Z);
				}
			}
		}
		catch (Exception exception)
		{
			//////GD.Print($"AttemptStoredLocationApplication exception: {exception}");
		}
	}

	public void ProcessGravity(double delta)
	{
		Velocity = new Vector3(
			Velocity.X + _serviceGravity.CurrentGravity.X,
			Velocity.Y + _serviceGravity.CurrentGravity.Y,
			Velocity.Z + _serviceGravity.CurrentGravity.Z
		);
		MoveAndCollide(Velocity * (float)delta);
	}

	public bool UsingMirrorControls { get; set; }
	public void SetDefaultControls()
	{
		UsingMirrorControls = false;
	}

	public void SetMirrorControls()
	{
		UsingMirrorControls = true;
	}
}
