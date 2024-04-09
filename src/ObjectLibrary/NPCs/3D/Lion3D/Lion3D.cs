using Godot;
using System;

public partial class Lion3D : Node3D
{
	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;
	private HoverTextBox _nodeHoverTextBox = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_nodeHoverTextBox = GetNode<HoverTextBox>("../HoverTextBox");
	}

	public override void _Process(double delta)
	{
		if (_nodeInteractableArea.GetOverlappingBodies().Count > 1)
		{
			HandleInteractableAreaHover();
		}
		else
		{
			HandleInteractableAreaLeave();
		}
	}

	public void HandleInteractableAreaHover()
	{
		_nodeHoverTextBox.SetText("Use either wasd or arrow keys to move.");
		_nodeHoverTextBox.ShowTextBox();
	}

	public void HandleInteractableAreaLeave()
	{
		_nodeHoverTextBox.HideTextBox();
		_nodeHoverTextBox.ClearText();
	}
}
