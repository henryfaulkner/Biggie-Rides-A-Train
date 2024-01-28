using Godot;
using System;

public partial class HiddenWall : HBoxContainer
{
	private static readonly int _OPEN_WALL_Y = -500;
	
	private HBoxContainer_nodeSelf = null;
	private StaticBody2D _nodeBookCase = null;
	private Area2D _nodeInteractableArea = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
