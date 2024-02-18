using Godot;
using System;

public partial class BasePanel : Panel
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");
	
	private Panel _nodeSelf = null;
	private VBoxContainer _nodeFightOption = null;
	private VBoxContainer _nodeChatOption = null;
	private VBoxContainer _nodeExitOption = null;
	
	private SelectionHelper _selectionHelper = null; 
	public bool IsOpen { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Panel>(".");
		_nodeFightOption = GetNode<VBoxContainer>("./MarginContainer/OptionContainer/FightOptionContainer");
		_nodeChatOption = GetNode<VBoxContainer>("./MarginContainer/OptionContainer/ChatOptionContainer");
		_nodeExitOption = GetNode<VBoxContainer>("./MarginContainer/OptionContainer/ExitOptionContainer");
		
		_selectionHelper = new SelectionHelper();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!IsOpen) return; 
		if (Input.IsActionJustPressed(_INTERACT_INPUT)) 
		{
			//HandleInteraction();
		}
		if (Input.IsActionJustPressed(_LEFT_INPUT)) 
		{
			_selectionHelper.ShiftSelectionLeft();
		}
		if (Input.IsActionJustPressed(_RIGHT_INPUT)) 
		{
			_selectionHelper.ShiftSelectionRight();
		}
	}
}
