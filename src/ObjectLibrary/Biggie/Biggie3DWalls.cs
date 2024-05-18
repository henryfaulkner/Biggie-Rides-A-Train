using Godot;
using System;

public partial class Biggie3DWalls : CharacterBody3D
{
	private static readonly StringName _MOVE_LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _MOVE_UP_INPUT = new StringName("move_up");
	private static readonly StringName _MOVE_RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _MOVE_DOWN_INPUT = new StringName("move_down");

	private static readonly int _SPRITE_FRAME_IDLE = 0;

	private Node _nodeBiggieSpriteMeshInstance = null;
	private TextBox _nodeTextBox = null;
	private InteractionTextBox _nodeInteractionTextBox = null;

	private bool _isMoving = false;
	private bool _canMove = true;
	private int _frameIncrement = 0;
	private Enumerations.Movement.Directions _currentFrameDirection = Enumerations.Movement.Directions.Down;

	private Vector3 GyroVector { get; set; }

	private SaveStateService _serviceSaveState = null;

	public override void _Ready()
	{
		_nodeBiggieSpriteMeshInstance = GetNode("./SpriteMeshInstance");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("../InteractionTextBox");

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
	}

	public override void _Process(double delta)
	{

	}

	// Fuck this. This will be a walk and rotation animation,
	// then do a scene transition.

	// The plane's effect on Biggie's movement is based what
	// Biggie's right- and down- inputs will result in. left- and up-
	// inputs will be the negatives.
	//
	// example: the default game plane vector would be (1, 0, 1) 
	// because right input increases Biggie's X position and down input
	// increases Biggie's Z position.
	//
	// example: the game plane vector if the player rotates against the default 
	// wall would be ()
	public void ShiftPlane(Vector3 newPlaneVector)
	{
		GyroVector = newPlaneVector.Normalized();
		RotateSprite(newPlaneVector);
	}

	private void RotateSprite(Vector3 newPlaneVector)
	{

	}
}
