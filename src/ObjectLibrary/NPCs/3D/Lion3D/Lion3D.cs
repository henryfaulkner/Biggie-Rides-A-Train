using Godot;
using System;

public partial class Lion3D : Node3D
{
	private readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Node3D _nodeSelf = null;
	private Area3D _nodeInteractableArea = null;

	private TextBoxService _serviceTextBox = null;

	[Export]
	public float Speed { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");
	}

	[Signal]
	public delegate void InteractLionEventHandler();

	public override void _Process(double _delta)
	{
		if (HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
		{
			GD.Print("Contains Biggie");
			HandleInteractableAreaHover();
			if (Input.IsActionJustPressed(_INTERACT_INPUT))
			{
				GD.Print("Interact Lion");
				EmitSignal(SignalName.InteractLion);
			}
		}
		else
		{
			HandleInteractableAreaLeave();
		}
	}

	public void HandleInteractableAreaHover()
	{
		_serviceTextBox.HoverTextBox.SetText("Use either wasd or arrow keys to move.");
		_serviceTextBox.HoverTextBox.ShowTextBox();
	}

	public void HandleInteractableAreaLeave()
	{
		_serviceTextBox.HoverTextBox.HideTextBox();
		_serviceTextBox.HoverTextBox.ClearText();
	}
}
