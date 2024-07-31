using Godot;
using System;

public partial class Scene_MainTerminal : Node3D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _SHIFT_CAMERA_INPUT = new StringName("shift_camera");

	private Biggie3D _nodeBiggie = null;
	private Node _nodeDefaultPCam = null;
	private Node _nodeMirrorPCam = null;

	public CameraBusiness CameraBusinessInstance { get; set; }

	private MoveToPathTest _nodeMoveToPathTest = null;

	public override void _Ready()
	{
		_nodeBiggie = GetNode<Biggie3D>("./LevelWrapper/TextBoxWrapper/NavigationRegion3D/Biggie3D");
		_nodeDefaultPCam = GetNode<Node>("./DefaultPCam");
		_nodeMirrorPCam = GetNode<Node>("./MirrorPCam");

		CameraBusinessInstance = new CameraBusiness(_nodeBiggie, _nodeDefaultPCam, _nodeMirrorPCam);
		CameraBusinessInstance.SetDefaultCamera();
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (Input.IsActionJustPressed(_SHIFT_CAMERA_INPUT) && CameraBusinessInstance.UsingDefaultPCam)
		{
			CameraBusinessInstance.SetMirrorCamera();
		}
		else if (Input.IsActionJustPressed(_SHIFT_CAMERA_INPUT) && CameraBusinessInstance.UsingMirrorPCam)
		{
			CameraBusinessInstance.SetDefaultCamera();
		}
	}
}
