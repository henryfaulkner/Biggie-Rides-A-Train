using Godot;
using System;

public partial class Scene_Taxi_Approaching_Train : Node3D
{
	private static readonly StringName _SCENE_INTRO_SLEEP = new StringName("res://Pages/Levels/2D/Tutorial/Intro/Scene_Intro_Sleep.tscn");
	private static readonly int _OPEN_TEXTBOX_AT_THIS_INCREMENT = 200;
	private static readonly int _REDIRECT_AT_THIS_INCREMENT = 300;

	private Node3D _nodeSelf = null;
	private CharacterBody3D _nodeTaxi = null;
	private AudioStreamPlayer _nodeAudio = null;

	private int FrameIncrement { get; set; }
	private bool PauseIncrement { get; set; }

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeTaxi = GetNode<CharacterBody3D>("./LevelWrapper/TextBoxWrapper/TaxiCharacterBody3D");
		_nodeAudio = GetNode<AudioStreamPlayer>("./AudioStreamPlayer");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");

		_serviceTextBox.TextBox.HidingTextBox += ContinuePlay;
		_nodeAudio.Play();
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (FrameIncrement == _OPEN_TEXTBOX_AT_THIS_INCREMENT)
		{
			CreateDialogue1(_serviceTextBox.TextBox);

			PauseIncrement = true;
			FrameIncrement += 1;
		}

		if (FrameIncrement == _REDIRECT_AT_THIS_INCREMENT)
		{
			RedirectToIntroSleep();
			return;
		}

		if (!PauseIncrement)
		{
			_nodeTaxi.Velocity = new Vector3(0.8f, 0, 0);
			_nodeTaxi.MoveAndSlide();
			FrameIncrement += 1;
		}
	}

	private void CreateDialogue1(TextBox textBox)
	{
		textBox.AddDialogue("In a land, not unlike our own, one cat is in a taxicar. He appears to be slimbering peacefully. Biggie, the cat in sleep, is approaching his destination.\n\nPress spacebar to advance text forward.");
		textBox.AddDialogue("The boy has yet to consider his fate. The date, he may soon realize, is closer than it may seem.");
		textBox.AddDialogue("You can hear the train whistle, muffled by the space between Biggie and the station. You drift closer to our traveler, who is gathering his thoughts.");
		textBox.ExecuteDialogueQueue();
	}


	private void RedirectToIntroSleep()
	{
		var nextScene = (PackedScene)ResourceLoader.Load(_SCENE_INTRO_SLEEP);
		GetTree().ChangeSceneToPacked(nextScene);
	}

	private void ContinuePlay()
	{
		//GD.Print("ContinuePlay");
		PauseIncrement = false;
	}
}
