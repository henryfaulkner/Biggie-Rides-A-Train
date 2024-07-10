using Godot;
using System;

public partial class WoodSign2 : Node3D
{
	private static readonly string _TEXT = "Use spacebar to interact with your surroundings.";

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;
	private HoverTextBoxHelper HoverTextBoxHelper { get; set; }

	private TextBoxService _serviceTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
		HoverTextBoxHelper = new HoverTextBoxHelper(_nodeSelf, _nodeInteractableArea, _serviceTextBox.HoverTextBox, _TEXT);
	}

	public override void _Process(double delta)
	{
		HoverTextBoxHelper.Process();
	}
}
