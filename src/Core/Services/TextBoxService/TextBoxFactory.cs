using System;
using Godot;

public static class TextBoxFactory
{
	private static readonly StringName _NODE_INTERACTION_TEXT_BOX = new StringName("res://ObjectLibrary/TextBoxes/InteractionTextBox/InteractionTextBox.tscn");
	private static readonly StringName _NODE_TEXT_BOX = new StringName("res://ObjectLibrary/TextBoxes/TextBox/TextBox.tscn");
	private static readonly StringName _NODE_HOVER_TEXT_BOX = new StringName("res://ObjectLibrary/TextBoxes/HoverTextBox/HoverTextBox.tscn");

	public static InteractionTextBox SpawnInteractionTextBox()
	{
		var scene = GD.Load<PackedScene>(_NODE_INTERACTION_TEXT_BOX);
		if (scene == null)
		{
			GD.PrintErr("Failed to load InteractionTextBox scene.");
			return null;
		}

		var instance = scene.Instantiate<InteractionTextBox>();
		if (instance == null)
		{
			GD.PrintErr("Failed to instantiate InteractionTextBox.");
		}

		return instance;
	}

	public static TextBox SpawnTextBox()
	{
		var scene = GD.Load<PackedScene>(_NODE_TEXT_BOX);
		if (scene == null)
		{
			GD.PrintErr("Failed to load TextBox scene.");
			return null;
		}

		var instance = scene.Instantiate<TextBox>();
		if (instance == null)
		{
			GD.PrintErr("Failed to instantiate TextBox.");
		}

		return instance;
	}

	public static HoverTextBox SpawnHoverTextBox()
	{
		var scene = GD.Load<PackedScene>(_NODE_HOVER_TEXT_BOX);
		if (scene == null)
		{
			GD.PrintErr("Failed to load HoverTextBox scene.");
			return null;
		}

		var instance = scene.Instantiate<HoverTextBox>();
		if (instance == null)
		{
			GD.PrintErr("Failed to instantiate HoverTextBox.");
		}

		return instance;
	}
}
