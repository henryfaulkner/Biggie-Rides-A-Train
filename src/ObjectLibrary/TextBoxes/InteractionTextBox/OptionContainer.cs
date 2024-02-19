using Godot;
using System;

public partial class OptionContainer : HBoxContainer
{
	private HBoxContainer _nodeSelf = null;
	private Label _nodeSelectionMarker = null;
	private Label _nodeOption = null;

	public int Id { get; set; }
	public bool IsSelected { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<HBoxContainer>(".");
		_nodeSelectionMarker = GetNode<Label>("./SelectionMarker");
		_nodeOption = GetNode<Label>("./Option");
	}

	public override void _Process(double delta)
	{
		if (IsSelected)
		{
			_nodeSelectionMarker.Text = "* ";
		}
		else
		{
			_nodeSelectionMarker.Text = "  ";
		}
	}
}
