using Godot;
using System;

public partial class Health : HBoxContainer
{
	private HBoxContainer _nodeSelf = null;
	private ProgressBar _nodeProgressBar = null;
	private Label _nodeHpValueLabel = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
