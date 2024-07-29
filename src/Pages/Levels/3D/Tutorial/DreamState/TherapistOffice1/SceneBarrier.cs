using Godot;
using System;

public partial class SceneBarrier : Node3D
{
	private static readonly string _TEXT = "Proceed through the fog...";
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;
	private HoverTextBoxHelper HoverTextBoxHelper { get; set; }

	private TextBoxService _serviceTextBox = null;

	public bool CanOpen { get; set; }

	private bool InitBit { get; set; }

	public SceneBarrier()
	{
		CanOpen = false;
		InitBit = false;
	}

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
	}

	public override void _Process(double delta)
	{
		if (!InitBit)
		{
			var hoverTextBox = _serviceTextBox.CreateHoverTextBox();
			HoverTextBoxHelper = new HoverTextBoxHelper(_nodeSelf, _nodeInteractableArea, hoverTextBox, _TEXT);
			InitBit = true;
		}

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
