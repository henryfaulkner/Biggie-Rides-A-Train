using Godot;
using System;

public partial class Biggie : Sprite2D
{
	private static readonly int _SPRITE_FRAME_IDLE = 0;
	private static readonly int _SPRITE_FRAME_WALK1 = 1;
	private static readonly int _SPRITE_FRAME_WALK2 = 2; 
	public static readonly int _SPRITE_FRAME_CHANGE_INTERVAL = 30;
	public static readonly int _SPRITE_WALK_FRAME_LENGTH = 2;
	public static readonly float _BIGGIE_VELOCITY = 5f;
	
	private Sprite2D _nodeSelf = null;
	private bool _isMoving = false;
	private int _frameIncrement = 0;
	
	private static readonly StringName _MOVE_LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _MOVE_UP_INPUT = new StringName("move_up");
	private static readonly StringName _MOVE_RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _MOVE_DOWN_INPUT = new StringName("move_down");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Biggie>(".");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Movement(_nodeSelf.Position.X, _nodeSelf.Position.Y);
		
		if (_isMoving) 
		{
			_frameIncrement += 1;
			_nodeSelf.Frame = ReturnSpriteWalkFrame(_frameIncrement);
		}
		else 
		{
			_frameIncrement = 0;
			_nodeSelf.Frame = _SPRITE_FRAME_IDLE;
		}
	}
	
	private int ReturnSpriteWalkFrame(int frameIncrement) 
	{
		var result = (frameIncrement / _SPRITE_FRAME_CHANGE_INTERVAL) % _SPRITE_WALK_FRAME_LENGTH;
		result += 1;
		return result;
	}
	
	private void Movement(float currX, float currY) 
	{
		if (Input.IsActionPressed(_MOVE_LEFT_INPUT)) 
		{
			_isMoving = true;
			UpdateBiggiePosition(currX - _BIGGIE_VELOCITY, currY);
			// Flip sprites
		} 
		else if (Input.IsActionPressed(_MOVE_UP_INPUT)) 
		{
			_isMoving = true;
			UpdateBiggiePosition(currX, currY - _BIGGIE_VELOCITY);
		}
		else if (Input.IsActionPressed(_MOVE_RIGHT_INPUT)) 
		{
			_isMoving = true;
			UpdateBiggiePosition(currX + _BIGGIE_VELOCITY, currY);
		}
		else if (Input.IsActionPressed(_MOVE_DOWN_INPUT)) 
		{
			_isMoving = true;
			UpdateBiggiePosition(currX, currY + _BIGGIE_VELOCITY);
		} 
		else 
		{
			_isMoving = false;
		}
	}
	
	private void UpdateBiggiePosition(float x, float y) {
		_nodeSelf.Position = new Vector2(x, y); 
	}
}
