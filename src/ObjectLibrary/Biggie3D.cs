using Godot;
using System;

public partial class Biggie3D : CharacterBody3D
{
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
	public static readonly float _BIGGIE_SPEED = 2400f;

	private Biggie3D _nodeSelf = null;
	private Node _nodeBiggieSpriteMeshInstance = null;

	private bool _isMoving = false;
	private bool _canMove = true;
	private int _frameIncrement = 0;
	private Enumerations.Movement.Directions _currentFrameDirection = Enumerations.Movement.Directions.Down;

	public override void _Ready()
	{
		GD.Print("Hello World");
		_nodeSelf = GetNode<Biggie3D>(".");
		_nodeBiggieSpriteMeshInstance = GetNode("./SpriteMeshInstance");

		// Access the GDScript instance through the Script property
		//var spriteMeshInstance = _nodeBiggieSprites.GetScript();
		//AttemptStoredLocationApplication();
	}

	public override void _Process(double delta)
	{
		Movement(delta);
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

	private void Movement(double delta)
	{
		Vector3 inputDirection = new Vector3();

		if (Input.IsActionPressed(_MOVE_LEFT_INPUT))
		{
			inputDirection.X -= 1.0f;
			_isMoving = true;
			_frameIncrement += 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Right;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (Input.IsActionPressed(_MOVE_RIGHT_INPUT))
		{
			inputDirection.X += 1.0f;
			_isMoving = true;
			_frameIncrement += 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Left;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		
		if (Input.IsActionPressed(_MOVE_DOWN_INPUT))
		{
			inputDirection.Z += .7f;
			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (Input.IsActionPressed(_MOVE_UP_INPUT))
		{
			inputDirection.Z -= .7f;
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

		Velocity = inputDirection;
		MoveAndSlide();
	}

	public void Collide(KinematicCollision2D collision)
	{
		if (collision.GetCollider().HasMethod("Hit"))
		{
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

	private void AttemptStoredLocationApplication()
	{
		////GD.Print("AttemptStoredLocationApplication");
		try
		{
			using (var context = new SaveStateContext())
			{
				var contextState = context.Load();
				var storedLocation = contextState.StoredLocation;
				var rs = new RelocationService();
				if (storedLocation != null)
				{
					////GD.Print($"Biggie applied Position. X: {storedLocation.X}. Y: {storedLocation.Y}");
					//_nodeSelf.Position = new Vector2(storedLocation.X, storedLocation.Y);
				}
			}
		}
		catch (Exception exception)
		{
			////GD.Print($"AttemptStoredLocationApplication exception: {exception}");
		}
	}
	
	private bool IsIdle()
	{
		return 
			!Input.IsActionPressed(_MOVE_UP_INPUT)
			&& !Input.IsActionPressed(_MOVE_RIGHT_INPUT)
			&& !Input.IsActionPressed(_MOVE_DOWN_INPUT)
			&& !Input.IsActionPressed(_MOVE_LEFT_INPUT);
	}
}
