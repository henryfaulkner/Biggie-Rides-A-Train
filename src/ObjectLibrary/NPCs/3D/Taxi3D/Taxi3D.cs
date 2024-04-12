using Godot;
using System;

public partial class Taxi3D : Node3D
{
	private static readonly int _SPRITE_FRAME_CHANGE_INTERVAL = 30;
	private static readonly int _SPRITE_SHEET_LENGTH = 2;
	private Node3D _nodeSelf = null;
	private Sprite3D _nodeSprites = null;

	public bool IsMoving { get; set; }
	private int FrameIncrement { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeSprites = GetNode<Sprite3D>("./StaticBody3D/Sprite3D");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsMoving)
		{
			FrameIncrement += 1;
			_nodeSprites.Frame = ReturnSpriteIndex(FrameIncrement);
		}
	}

	private int ReturnSpriteIndex(int frameIncrement)
	{
		return (frameIncrement / _SPRITE_FRAME_CHANGE_INTERVAL) % _SPRITE_SHEET_LENGTH;
	}
}
