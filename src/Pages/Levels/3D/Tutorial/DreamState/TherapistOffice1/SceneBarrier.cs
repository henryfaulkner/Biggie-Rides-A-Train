using Godot;
using System;

public partial class SceneBarrier : Node3D
{
	private static readonly string _TEXT = "Proceed through the fog...";
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;
	private HoverTextBox _nodeHoverTextBox = null;
	private HoverTextBoxHelper HoverTextBoxHelper { get; set; }

	public bool CanOpen { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_nodeHoverTextBox = GetNode<HoverTextBox>("../HoverTextBox");
		HoverTextBoxHelper = new HoverTextBoxHelper(_nodeSelf, _nodeInteractableArea, _nodeHoverTextBox, _TEXT);
		CanOpen = false;
	}

	public override void _Process(double delta)
	{
		if (CanOpen)
		{
			HoverTextBoxHelper.Process();

			if (Input.IsActionJustReleased(_INTERACT_INPUT)
				&& HoverTextBoxHelper.RecentlyOpen)
			{
				QueueFree();
			}
		}

	}

	public override void _ExitTree()
	{
		HoverTextBoxHelper.ForceClose();
		base._ExitTree();
	}


}
