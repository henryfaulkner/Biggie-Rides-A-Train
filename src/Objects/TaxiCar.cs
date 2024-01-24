using Godot;
using System;

public partial class TaxiCar : Sprite2D
{
	public static readonly int _spriteFrameChangeInterval = 30;
	public static readonly int _spriteFramesLength = 2;
	private int _frameIncrement = 0;
	private Sprite2D _self = null;
	private bool _isMoving = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_self = GetNode<Sprite2D>(".");
		_isMoving = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_isMoving) {
			_frameIncrement += 1;
			_self.Frame = ReturnSpriteIndex(_frameIncrement);
		}
	}
	
	public int ReturnSpriteIndex(int frameIncrement) 
	{
		var result = (frameIncrement / _spriteFrameChangeInterval) % _spriteFramesLength;
		return result;
	}
	
	public void SetMovingBit(bool bit) {
		_isMoving = bit;
		return;
	}
}
