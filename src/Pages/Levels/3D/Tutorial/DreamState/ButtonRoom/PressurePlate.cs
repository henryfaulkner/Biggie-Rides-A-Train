using Godot;
using System;

public partial class PressurePlate : MeshInstance3D
{
	private Area3D _nodeInteractableArea = null;
	private AudioStreamPlayer3D _nodeAudio = null;
	private Vector3 PressedPosition { get; set; }
	
	private bool _pressed = false;
	
	public override void _Ready()
	{
		_nodeInteractableArea = GetNode<Area3D>("./Area3D");
		_nodeAudio = GetNode<AudioStreamPlayer3D>("./AudioStreamPlayer3D");
		PressedPosition = new Vector3(Position.X,
									-(Scale.Y / 2),
									Position.Z);
	}
	
	public override void _PhysicsProcess(double _delta)
	{
		if (_pressed) return;
		if (HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
		{
			Press();
		}
	}
	
	[Signal]
	public delegate void PressedEventHandler();
	
	private void Press()
	{
		GD.Print("Call Press");
		_pressed = true;
		_nodeAudio.Play();
		Position = PressedPosition;
		EmitSignal(SignalName.Pressed);
	}
}
