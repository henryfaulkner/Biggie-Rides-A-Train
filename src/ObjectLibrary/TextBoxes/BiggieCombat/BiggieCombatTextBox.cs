using Godot;
using System;

public partial class BiggieCombatTextBox : CanvasLayer
{

	private static readonly StringName _TAB_INPUT = new StringName("tab");
	private static readonly StringName _ENTER_INPUT = new StringName("enter");
	private static readonly StringName _SPACE_INPUT = new StringName("interact");

	private CanvasLayer _nodeSelf = null;
	private Button _nodeFirstOption = null;
	private Button _nodeSecondOption = null;
	private Button _nodeThirdOption = null;
	private Button _nodeExit = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
