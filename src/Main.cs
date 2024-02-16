using Godot;
using System;

public partial class Main : Node2D
{
	private static readonly StringName _LEVEL_OUTSIDE_STATION = new StringName("res://Levels/OutsideStation/LevelOutsideStation.tscn");

	private static readonly StringName _TAB_INPUT = new StringName("tab");
	private static readonly StringName _ENTER_INPUT = new StringName("enter");
	private static readonly StringName _SPACE_INPUT = new StringName("interact");

	private Button _nodePlay = null;
	private Button _nodeOptions = null;
	private Button _nodeQuit = null;

	private Button[] Buttons { get; set; }
	private int FocusIndex { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodePlay = GetNode<Button>("./LevelWrapper/MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/Play");
		_nodeOptions = GetNode<Button>("./LevelWrapper/MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/HBoxContainer/Options");
		_nodeQuit = GetNode<Button>("./LevelWrapper/MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/HBoxContainer/Quit");
		Buttons = new Button[] { _nodePlay, _nodeOptions, _nodeQuit };
		Buttons[0].GrabFocus();
		FocusIndex = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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

