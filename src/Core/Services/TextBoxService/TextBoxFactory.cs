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
        var instance = scene.Instantiate<InteractionTextBox>();
        return instance;
    }

    public static TextBox SpawnTextBox()
    {
        var scene = GD.Load<PackedScene>(_NODE_INTERACTION_TEXT_BOX);
        var instance = scene.Instantiate<TextBox>();
        return instance;
    }

    public static HoverTextBox SpawnHoverTextBox()
    {
        var scene = GD.Load<PackedScene>(_NODE_INTERACTION_TEXT_BOX);
        var instance = scene.Instantiate<HoverTextBox>();
        return instance;
    }
}