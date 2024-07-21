using Godot;
using System;

public partial class Biggie : CharacterBody2D
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
	public static readonly float _BIGGIE_SPEED = 400f;

	private Biggie _nodeSelf = null;
	private Sprite2D _nodeBiggieSprites = null;

	private bool _isMoving = false;
	private bool _canMove = true;
	private int _frameIncrement = 0;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Biggie>(".");
		_nodeBiggieSprites = GetNode<Sprite2D>("./BiggieSprites");

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");

		//AttemptStoredLocationApplication();
	}

	public override void _Process(double delta)
	{
		if (_canMove
			&& _nodeSelf.IsVisibleInTree()
			&& (!_serviceTextBox.TextBox.IsOpen())
			&& (!_serviceTextBox.InteractionTextBox.IsOpen))
		{
			var collision = Movement(delta);
			if (collision != null)
			{
				Collide(collision);
			}
		}
	}

	private int ReturnSpriteWalkFrame(int frameIncrement, bool isLeft = false)
	{
		var result = (frameIncrement / _SPRITE_FRAME_CHANGE_INTERVAL) % _SPRITE_WALK_FRAME_LENGTH;
		// Skip Idle sprite
		result += 1;
		// Account for sprite flipping
		if (isLeft) result += 2;
		return result;
	}

	private KinematicCollision2D Movement(double delta)
	{
		Vector2 inputDirection = Input.GetVector(_MOVE_LEFT_INPUT, _MOVE_RIGHT_INPUT, _MOVE_UP_INPUT, _MOVE_DOWN_INPUT);
		Velocity = inputDirection * _BIGGIE_SPEED;
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			Velocity = Velocity.Slide(collision.GetNormal());
		}

		if (Input.IsActionPressed(_MOVE_LEFT_INPUT))
		{
			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSprites.Frame = ReturnSpriteWalkFrame(_frameIncrement, true);
		}
		else if (Input.IsActionPressed(_MOVE_UP_INPUT))
		{
			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSprites.Frame = ReturnSpriteWalkFrame(_frameIncrement);
		}
		else if (Input.IsActionPressed(_MOVE_RIGHT_INPUT))
		{
			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSprites.Frame = ReturnSpriteWalkFrame(_frameIncrement);
		}
		else if (Input.IsActionPressed(_MOVE_DOWN_INPUT))
		{
			_isMoving = true;
			_frameIncrement += 1;
			_nodeBiggieSprites.Frame = ReturnSpriteWalkFrame(_frameIncrement);
		}
		else
		{
			_isMoving = false;
			_frameIncrement = 0;
			_nodeBiggieSprites.Frame = _SPRITE_FRAME_IDLE;
		}

		return collision;
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
					_nodeSelf.Position = new Vector2(storedLocation.X, storedLocation.Y);
				}
			}
		}
		catch (Exception exception)
		{
			//////GD.Print($"AttemptStoredLocationApplication exception: {exception}");
		}
	}
}
