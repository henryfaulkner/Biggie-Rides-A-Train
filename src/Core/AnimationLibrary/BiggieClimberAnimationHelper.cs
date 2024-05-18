using Godot;
using System;

public class BiggieClimberAnimationHelper
{
	private static readonly int _SPRITE_FRAME_IDLE = 0;
	private static readonly int _SPRITE_FRAME_WALK1 = 1;
	private static readonly int _SPRITE_FRAME_WALK2 = 2;
	private static readonly int _SPRITE_FRAME_WALK1_LEFT = 3;
	private static readonly int _SPRITE_FRAME_WALK2_LEFT = 4;

	private Vector3 XOriginAxis { get; set; }
	private Vector3 ZOriginAxis { get; set; }
	private Transform3D ForwardTargetRotation { get; set; }
	private Transform3D RightTargetRotation { get; set; }
	private Transform3D BackwardTargetRotation { get; set; }
	private Transform3D LeftTargetRotation { get; set; }

	private int FrameIncrement { get; set; }

	// Transparent wall textures: "I can get what I’m looking for using an alpha in the albedo while refraction is activated."
	// Source: https://forum.godotengine.org/t/alpha-channels-for-transparency-texturing/19173/3

	private Node DefaultFloor { get; set; }
	private Node DefaultCeiling { get; set; }
	private Node DefaultForwardWall { get; set; }
	private Node DefaultRightWall { get; set; }
	private Node DefaultBackwardWall { get; set; }
	private Node DefaultLeftWall { get; set; }

	public BiggieClimberAnimationHelper(
		Node defaultFloor = null,
		Node defaultCeiling = null,
		Node defaultForwardWall = null,
		Node defaultRightWall = null,
		Node defaultBackwardWall = null,
		Node defaultLeftWall = null
	)
	{
		FrameIncrement = 0;
		XOriginAxis = new Vector3(1, 0, 0);
		ZOriginAxis = new Vector3(0, 0, 1);
		// 1.5708 radians is equivalent to 90 degrees
		//ForwardTargetRotation = new Transform3D.Rotated(ZOriginAxis, 1.5708f);
		//RightTargetRotation = new Transform3D.Rotated(XOriginAxis, 1.5708f);
		//BackwardTargetRotation = new Transform3D.Rotated(ZOriginAxis, -1.5708f);
		//LeftTargetRotation = new Transform3D.Rotated(XOriginAxis, -1.5708f);

		DefaultFloor = defaultFloor;
		DefaultCeiling = defaultCeiling;
		DefaultForwardWall = defaultForwardWall;
		DefaultRightWall = defaultRightWall;
		DefaultBackwardWall = defaultBackwardWall;
		DefaultLeftWall = defaultLeftWall;
	}

	public void ClimbForward(Biggie3D biggie, Node biggieSpriteMeshInstance, double delta)
	{
		biggieSpriteMeshInstance.Call("set_frame", _SPRITE_FRAME_WALK1);
		FrameIncrement += 1;
	}

	public void ClimbRight(Biggie3D biggie, Node biggieSpriteMeshInstance)
	{
		biggieSpriteMeshInstance.Call("set_frame", _SPRITE_FRAME_WALK1);
		FrameIncrement += 1;
	}

	public void ClimbBackward(Biggie3D biggie, Node biggieSpriteMeshInstance)
	{
		biggieSpriteMeshInstance.Call("set_frame", _SPRITE_FRAME_WALK1);
		FrameIncrement += 1;
	}

	public void ClimbLeft(Biggie3D biggie, Node biggieSpriteMeshInstance)
	{
		biggieSpriteMeshInstance.Call("set_frame", _SPRITE_FRAME_WALK1_LEFT);
		FrameIncrement += 1;
	}

	private void RevealAllWalls()
	{

	}
}
