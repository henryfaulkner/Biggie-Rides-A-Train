using Godot;
using System;

public partial class OptionContainer : HBoxContainer
{
	private HBoxContainer _nodeSelf = null;
	private Label _nodeSelectionMarker = null;
	private Label _nodeOption = null;	
	
	public bool IsSelected { get; set; } 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<HBoxContainer>(".");
		_nodeSelectionMarker = GetNode<Label>("./SelectionMarker");
		_nodeOption = GetNode<Label>("./Option");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsSelected) {
			_nodeSelectionMarker.Text = "* ";	
		} 
		else 
		{
			_nodeSelectionMarker.Text = "  ";
		}
	}
}
