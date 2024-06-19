using Godot;
using System;

public partial class Scene_MainTerminal : Node3D
{
	private static readonly StringName _SHIFT_CAMERA_INPUT = new StringName("shift_camera");
	
	private Biggie3D _nodeBiggie = null;
	private Node _nodeDefaultPCam = null;
	private Node _nodeMirrorPCam = null;
	
	public CameraBusiness CameraBusinessInstance { get; set; }
	
	public override void _Ready()
	{
		_nodeBiggie = GetNode<Biggie3D>("./TextBoxWrapper/LevelWrapper/Biggie3D");
		_nodeDefaultPCam = GetNode<Node>("./DefaultPCam");
		_nodeMirrorPCam = GetNode<Node>("./MirrorPCam");
		
		CameraBusinessInstance = new CameraBusiness(_nodeBiggie, _nodeDefaultPCam, _nodeMirrorPCam);
		CameraBusinessInstance.SetDefaultCamera();
	}
	
	public override void _PhysicsProcess(double _delta)
	{
		if (Input.IsActionPressed(_SHIFT_CAMERA_INPUT) && CameraBusinessInstance.UsingDefaultPCam)
		{
			CameraBusinessInstance.SetMirrorCamera();
		} 
		else if (Input.IsActionPressed(_SHIFT_CAMERA_INPUT) && CameraBusinessInstance.UsingMirrorPCam)
		{
			CameraBusinessInstance.SetDefaultCamera();
		}
	} 	
}
