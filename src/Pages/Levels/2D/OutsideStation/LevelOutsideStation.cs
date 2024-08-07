using Godot;
using System;

public partial class LevelOutsideStation : Node2D
{
	private Node2D _level = null;
	private TaxiCar _taxi = null;
	private TextBox _textbox = null;
	private Biggie _biggie = null;

	private float _taxiX = 0f;
	private float _taxiY = 0f;
	private float _taxiVelocity = 1.5f;
	private int _frameInc = 0;

	private float _viewportX = 0f;
	private float _viewportY = 0f;

	private bool _arrivalSceneConcluded = false;

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_level = GetNode<Node2D>("/root/LevelOutsideStation");
		_taxi = GetNode<TaxiCar>("/root/LevelOutsideStation/TextBoxWrapper/TaxiCarContainer/TaxiCar");
		_textbox = GetNode<TextBox>("/root/LevelOutsideStation/TextBoxWrapper/TextBox");
		_biggie = GetNode<Biggie>("/root/LevelOutsideStation/TextBoxWrapper/BiggieContainer/Biggie");
		UpdateTaxiPosition(_taxi.Position.X, _taxi.Position.Y);

		// ASSUME STATIC VIEWPORT
		_viewportX = ((Window)GetViewport()).Size.X;
		_viewportY = ((Window)GetViewport()).Size.Y;

		_biggie.Hide();

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
	}

	public override void _Process(double delta)
	{
		if (_taxiX < (_viewportX / 8))
		{
			// Approach center
			_taxi.SetMovingBit(true);
			UpdateTaxiPosition(_taxiX + _taxiVelocity, _taxiY);
		}
		else if (!_arrivalSceneConcluded)
		{
			_taxi.SetMovingBit(false);

			var processTextBox = _serviceTextBox.CreateTextBox();
			processTextBox.AddDialogue("Alright. Here you are. The Eastbay Station. Destination, I've got to assume the Westbay for that Westbay Cathedral venue.");
			processTextBox.AddDialogue("Everyone raves about the Conductor that place has. I'll get there one day. Hey, I hope you have a TICKET for that here train. You'll need one if you're wanting to board.");
			processTextBox.AddDialogue("Anyways, good luck, have fun.");
			_serviceTextBox.EnqueueProcess(processTextBox);
			_serviceTextBox.ExecuteQueuedProcesses();


			_arrivalSceneConcluded = true;
			_biggie.Show();
			_biggie.CanMove(false);
		}
		else if (_arrivalSceneConcluded && !_textbox.IsOpen())
		{
			// Exit scene
			_taxi.SetMovingBit(true);
			UpdateTaxiPosition(_taxiX + _taxiVelocity, _taxiY);
			_biggie.CanMove(true);
		}
	}

	private void UpdateTaxiPosition(float x, float y)
	{
		_taxiX = x;
		_taxiY = y;
		_taxi.Position = new Vector2(_taxiX, _taxiY);
	}

	private void DebugTaxiPosition()
	{
		//////GD.Print("taxi.Position.X: ", _taxi.Position.X);
		//////GD.Print("taxi.Position.y: ", _taxi.Position.Y);
	}
}
