using Godot;
using System;

public partial class TaxiCar : Sprite2D
{
	public static readonly int _spriteFrameChangeInterval = 30;
	public static readonly int _spriteFramesLength = 2;
	private int _frameIncrement = 0;
	private Sprite2D _self = null;
	private bool _isMoving = false;

	public override void _Ready()
	{
		_self = GetNode<Sprite2D>(".");
		_isMoving = true;
	}

	public override void _Process(double _delta)
	{
		if (_isMoving)
		{
			_frameIncrement += 1;
			_self.Frame = ReturnSpriteIndex(_frameIncrement);
		}
	}

	private int ReturnSpriteIndex(int frameIncrement)
	{
		var result = (frameIncrement / _spriteFrameChangeInterval) % _spriteFramesLength;
		return result;
	}

	public bool GetMovingBit()
	{
		return _isMoving;
	}

	public void SetMovingBit(bool bit)
	{
		_isMoving = bit;
		return;
	}
}
