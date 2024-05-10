using Godot;
using System;

public partial class Subconscious : CharacterBody3D
{
	private static readonly StringName _MOVE_LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _MOVE_UP_INPUT = new StringName("move_up");
	private static readonly StringName _MOVE_RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _MOVE_DOWN_INPUT = new StringName("move_down");

	private static readonly int _SPRITE_FRAME_IDLE = 0;
	private static readonly int _SPRITE_FRAME_WALK1 = 1;
	private static readonly int _SPRITE_FRAME_WALK2 = 2;
	private static readonly int _SPRITE_FRAME_WALK1_RIGHT = 3;
	private static readonly int _SPRITE_FRAME_WALK2_RIGHT = 4;
	public static readonly int _SPRITE_FRAME_CHANGE_INTERVAL = 45;
	public static readonly int _SPRITE_WALK_FRAME_LENGTH = 2;
	public static readonly float _BIGGIE_SPEED = 4800f;
	public static readonly float _BIGGIE_SPEED_X_RATIO = 1.0f;
	public static readonly float _BIGGIE_SPEED_Z_RATIO = 0.7f;

	private Node _nodeBiggieSpriteMeshInstance = null;
	private TextBox _nodeTextBox = null;
	private InteractionTextBox _nodeInteractionTextBox = null;

	public override void _Ready()
	{
		_nodeBiggieSpriteMeshInstance = GetNode("./SpriteMeshInstance");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("../InteractionTextBox");
	}

	public override void _PhysicsProcess(double _delta)
	{
	}
}
