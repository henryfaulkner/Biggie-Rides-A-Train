using Godot;
using System;

public partial class DefeatSceneDjBattle : Node2D
{
	private static readonly StringName _LEVEL_MAIN_MENU = new StringName("res://Main.tscn");

	private static readonly StringName _TAB_INPUT = new StringName("tab");
	private static readonly StringName _ENTER_INPUT = new StringName("enter");
	private static readonly StringName _SPACE_INPUT = new StringName("interact");

	private Button _nodeMainMenuButton = null;

	private Button[] Buttons { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeMainMenuButton = GetNode<Button>("MarginContainer/VBoxContainer/MarginContainer/MainMenuButton");
		Buttons = new Button[] { _nodeMainMenuButton };
		Buttons[0].GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_main_menu_button_pressed()
	{
		using (var context = new SaveStateService())
		{
			context.Clear();
		}
		RedirectToMainMenu();
	}

	private void RedirectToMainMenu()
	{
		var root = GetTree().Root;

		// Remove the current level
		var level = root.GetNode("DefeatSceneDjBattle");
		root.RemoveChild(level);
		level.CallDeferred("free");

		// Add the next level
		var nextLevelResource = GD.Load<PackedScene>(_LEVEL_MAIN_MENU);
		var nextLevel = nextLevelResource.Instantiate<Node>();
		root.AddChild(nextLevel);
		nextLevel.Owner = root;
	}
}




