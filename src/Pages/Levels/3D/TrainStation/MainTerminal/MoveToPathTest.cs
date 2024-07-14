using Godot;
using System;

public partial class MoveToPathTest : Node3D
{
	private Lion3D _nodeLion = null;
	private Node3D _nodePathParent = null;
	private Path3D _nodePath = null;
	private PathFollow3D _nodePathFollow = null;

	private bool SyncBit { get; set; }

	public override void _Ready()
	{
		_nodeLion = GetNode<Lion3D>("./Lion3D");
		_nodePathParent = GetNode<Node3D>(".");
		_nodePath = GetNode<Path3D>("./Path3D");
		_nodePathFollow = GetNode<PathFollow3D>("./Path3D/PathFollow3D");
		SyncBit = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (SyncBit)
		{
			if (_nodeLion.Position == _nodePath.Position)
			{
				SetLionAsPathFollowChild(_nodeLion, _nodePathFollow);
				SyncBit = false;
			}
			else
			{
				_nodeLion.Position.MoveToward(_nodePath.Position, (float)delta);
			}
		}

		if (_nodePathFollow.ProgressRatio == 1.0f)
		{
			DetachLionFromPath(_nodeLion, _nodePathParent);
		}
	}

	public void StartMoveTowardPath()
	{
		SyncBit = true;
	}

	public void SetLionAsPathFollowChild(Node3D node, PathFollow3D pathFollow)
	{
		pathFollow.AddChild(node);
		node.Owner = pathFollow;
	}

	public void DetachLionFromPath(Node3D node, Node3D pathParent)
	{
		pathParent.AddChild(node);
		node.Owner = pathParent;
	}
}
