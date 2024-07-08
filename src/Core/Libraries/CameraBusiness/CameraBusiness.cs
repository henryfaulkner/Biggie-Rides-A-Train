using System;
using Godot;

public partial class CameraBusiness : Node
{
	private Biggie3D _nodeBiggie = null;
	private Node _nodeDefaultPCam = null;
	private Node _nodeMirrorPCam = null;

	public bool UsingDefaultPCam { get; set; }
	public bool UsingMirrorPCam { get; set; }

	public CameraBusiness(Biggie3D nodeBiggie, Node nodeDefaultPCam, Node nodeMirrorPCam) 
	{
		_nodeBiggie = nodeBiggie;
		_nodeDefaultPCam = nodeDefaultPCam;
		_nodeMirrorPCam = nodeMirrorPCam;
		UsingDefaultPCam = false;
		UsingMirrorPCam = false;
	}

	public void SetDefaultCamera()
	{
		_nodeBiggie.SetDefaultControls();
		_nodeDefaultPCam.Call("set_priority", 10);
		_nodeMirrorPCam.Call("set_priority", 0);
		UsingDefaultPCam = true;
		UsingMirrorPCam = false;
	}

	public void SetMirrorCamera()
	{
		_nodeBiggie.SetMirrorControls();
		_nodeDefaultPCam.Call("set_priority", 0);
		_nodeMirrorPCam.Call("set_priority", 10);
		UsingDefaultPCam = false;
		UsingMirrorPCam = true;
	}

	private bool CheckAndLogIfNull()
	{
		bool result = false;
		if (_nodeBiggie == null)
		{
			GD.Print("CameraBusiness: Biggie is null.");
			result = true;
		}
		if (_nodeDefaultPCam == null)
		{
			GD.Print("CameraBusiness: Default PCam is null.");
			result = true;
		}
		if (_nodeMirrorPCam == null)
		{
			GD.Print("CameraBusiness: Mirror PCam is null.");
			result = true;
		}
		return result;
	}
}
