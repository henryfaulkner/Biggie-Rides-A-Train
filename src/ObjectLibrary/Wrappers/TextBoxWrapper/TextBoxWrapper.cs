using Godot;
using System;

public partial class TextBoxWrapper : Node2D
{
	public static readonly Vector2I _windowSize = new Vector2I(2048, 1024);
	// https://www.reddit.com/r/godot/comments/17tqipk/in_c_beware_using_strings_in_inputisactionpressed/
	private static readonly StringName _RESTART_SCENE_INPUT = new StringName("restart_scene");
	private static readonly StringName _TERMINATE_GAME_INPUT = new StringName("terminate_game");
	private static readonly StringName _CLEAR_SAVE_STATE_INPUT = new StringName("clear_save_state");

	public override void _Ready()
	{
		((Window)GetViewport()).Size = _windowSize;
		((Window)GetViewport()).Unresizable = true;
	}

	public override void _Process(double delta)
	{
		// ASSUMING INPUTMAP HAS A MAPPING FOR restart_scene
		if (Input.IsActionJustPressed(_RESTART_SCENE_INPUT))
		{
			////GD.Print("Restart Scene Input");
			GetTree().ReloadCurrentScene();
		}

		// ASSUMING INPUTMAP HAS A MAPPING FOR terminate_game
		if (Input.IsActionJustPressed(_TERMINATE_GAME_INPUT))
		{
			////GD.Print("Terminate Game Input");
			var nextScene = (PackedScene)ResourceLoader.Load("res://Main.tscn");
			GetTree().ChangeSceneToPacked(nextScene);
		}

		// ASSUMING INPUTMAP HAS A MAPPING FOR clear_save_state
		if (Input.IsActionJustPressed(_CLEAR_SAVE_STATE_INPUT))
		{
			////GD.Print("Clear Save State Input");
			using (var context = new SaveStateContext())
			{
				context.Clear();
			}
		}
	}
}
