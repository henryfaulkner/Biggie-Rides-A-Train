using Godot;
using System;

public partial class LevelOutsideStation : Node2D
{
	private Node2D _level = null;
	private TaxiCar _taxi = null;
	
	private float _taxiX = 0f;
	private float _taxiY = 0f;
	private float _taxiVelocity = 1.5f;
	private int _frameInc = 0;
	
	private float _viewportX = 0f;
	private float _viewportY = 0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_level = GetNode<Node2D>("/root/LevelOutsideStation");
		_taxi = GetNode<TaxiCar>("/root/LevelOutsideStation/LevelWrapper/BoxContainer/TaxiCar");
		UpdateTaxiPosition(_taxi.Position.X, _taxi.Position.Y);
		
		// ASSUME STATIC VIEWPORT
		_viewportX = ((Window)GetViewport()).Size.X;
		_viewportY = ((Window)GetViewport()).Size.Y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_taxiX < (_viewportX / 8)) {
			UpdateTaxiPosition(_taxiX + _taxiVelocity, _taxiY);
		}
		else 
		{
			_taxi.SetMovingBit(false);
		}
	}
	
	public void UpdateTaxiPosition(float x, float y) {
		_taxiX = x;
		_taxiY = y;
		_taxi.Position = new Vector2(_taxiX, _taxiY); 
	}
	
	public void DebugTaxiPosition() {
		GD.Print("taxi.Position.X: ", _taxi.Position.X);
		GD.Print("taxi.Position.y: ", _taxi.Position.Y);
	}
}
