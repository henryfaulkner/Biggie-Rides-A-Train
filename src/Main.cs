using Godot;
using System;

public partial class Main : Node2D
{
	private static readonly StringName _SCENE_INTRO = new StringName("res://Pages/Levels/3D/Tutorial/Intro/Scene_Taxi_Approaching_Train.tscn");

	private static readonly StringName _TAB_INPUT = new StringName("tab");
	private static readonly StringName _ENTER_INPUT = new StringName("enter");
	private static readonly StringName _SPACE_INPUT = new StringName("interact");

	private Button _nodePlay = null;
	private Button _nodeOptions = null;
	private Button _nodeQuit = null;

	private Button[] Buttons { get; set; }
	private int FocusIndex { get; set; }

	public override void _Ready()
	{
		_nodePlay = GetNode<Button>("./TextBoxWrapper/MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/Play");
		_nodeOptions = GetNode<Button>("./TextBoxWrapper/MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/HBoxContainer/Options");
		_nodeQuit = GetNode<Button>("./TextBoxWrapper/MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/HBoxContainer/Quit");
		Buttons = new Button[] { _nodePlay, _nodeOptions, _nodeQuit };
		Buttons[0].GrabFocus();
		FocusIndex = 0;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(_TAB_INPUT))
		{
			int len = Buttons.Length;
			if (FocusIndex == len - 1)
			{
				Buttons[0].GrabFocus();
				FocusIndex = 0;
			}
			else
			{
				Buttons[FocusIndex + 1].GrabFocus();
				FocusIndex += 1;
			}

		}
	}

	public void _on_options_pressed()
	{
		// Replace with function body.
	}


	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}

