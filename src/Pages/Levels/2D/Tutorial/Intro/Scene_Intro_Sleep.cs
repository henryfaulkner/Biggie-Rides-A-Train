using Godot;
using System;

public partial class Scene_Intro_Sleep : Node2D
{
	private static readonly StringName _SCENE_DREAM_STATE = new StringName("res://Pages/Levels/3D/Tutorial/DreamState/DreamRoom1/Scene_Dream_Room1.tscn");
	private static readonly int _OPEN_TEXTBOX_AT_THIS_INCREMENT = 100;
	private static readonly int _REDIRECT_AT_THIS_INCREMENT = 200;

	private Node2D _nodeSelf = null;

	private TextBoxService _serviceTextBox = null;

	private int FrameIncrement { get; set; }
	private bool PauseIncrement { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		var biggie = GetNode<Biggie>("TextBoxWrapper/Biggie");
		biggie.CanMove(false);

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
		//_serviceTextBox.TextBox.HidingTextBox += ContinuePlay;
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (FrameIncrement == _OPEN_TEXTBOX_AT_THIS_INCREMENT)
		{
			CreateDialogue1();

			PauseIncrement = true;
			FrameIncrement += 1;
		}

		if (FrameIncrement == _REDIRECT_AT_THIS_INCREMENT)
		{
			RedirectToDreamState();
			return;
		}

		if (!PauseIncrement)
		{
			FrameIncrement += 1;
		}
	}

	private void CreateDialogue1()
	{
		var processTextBox = _serviceTextBox.CreateTextBox();
		processTextBox.AddDialogue("Biggie is fast asleep. Let's visit him in his dreams.");
		_serviceTextBox.EnqueueProcess(processTextBox);
		_serviceTextBox.ExecuteQueuedProcesses();
	}


	private void RedirectToDreamState()
	{
		var nextScene = (PackedScene)ResourceLoader.Load(_SCENE_DREAM_STATE);
		GetTree().ChangeSceneToPacked(nextScene);
	}

	private void ContinuePlay()
	{
		PauseIncrement = false;
	}
}
