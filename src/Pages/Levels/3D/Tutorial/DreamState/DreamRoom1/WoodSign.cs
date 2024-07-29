using Godot;
using System;

public partial class WoodSign : Node3D
{
	private static readonly string _TEXT = "Use either wasd or arrow keys to move.";

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;
	private HoverTextBoxHelper HoverTextBoxHelper { get; set; }

	private TextBoxService _serviceTextBox = null;

	private bool InitBit { get; set; }

	public WoodSign()
	{
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

		HoverTextBoxHelper.Process();
	}
}
