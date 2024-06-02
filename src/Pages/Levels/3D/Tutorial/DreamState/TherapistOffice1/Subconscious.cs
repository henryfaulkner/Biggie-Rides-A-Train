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
	public static readonly float _BIGGIE_SPEED = 1.4f;
	public static readonly float _BIGGIE_SPEED_X_RATIO = 1.0f;
	public static readonly float _BIGGIE_SPEED_Z_RATIO = 0.7f;

	private Node _nodeBiggieSpriteMeshInstance = null;
	private TextBox _nodeTextBox = null;
	private InteractionTextBox _nodeInteractionTextBox = null;

	private bool _isMoving = false;
	private bool _canMove = true;
	private int _frameIncrement = 0;
	private Enumerations.Movement.Directions _currentFrameDirection = Enumerations.Movement.Directions.Down;

	public override void _Ready()
	{
		_nodeBiggieSpriteMeshInstance = GetNode("./SpriteMeshInstance");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("../InteractionTextBox");
	}

	public override void _PhysicsProcess(double _delta)
	{
	}

	private int ReturnSpriteWalkFrame(int frameIncrement)
	{
		var result = (frameIncrement / _SPRITE_FRAME_CHANGE_INTERVAL) % _SPRITE_WALK_FRAME_LENGTH;
		// Skip Idle sprite
		result += 1;
		// Account for sprite flipping
		if (_currentFrameDirection == Enumerations.Movement.Directions.Left) result += 1;
		return result;
	}

	public bool CanMove()
	{
		return _canMove;
	}

	public void CanMove(bool canMove)
	{
		_canMove = canMove;
	}

	public bool ForceWalk(Vector3 target, double delta)
	{
		//GD.Print("Subconscious ForceWalk");
		CanMove(false);
		Vector3 direction = (target - Position).Normalized();
		Vector3 inputDirection = Vector3.Zero;
		////GD.Print($"ForceWalk Direction X:{direction.X} Y:{direction.Y} Z:{direction.Z}");
		// //GD.Print($"target Position X:{target.X} Y:{target.Y} Z:{target.Z}");
		// //GD.Print($"subconscious Position X:{Position.X} Y:{Position.Y} Z:{Position.Z}");

		if (direction.X - 0.5f > 0) // RIGHT
		{
			inputDirection.X = _BIGGIE_SPEED_X_RATIO;
			_isMoving = true;
			_frameIncrement += 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Left;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (direction.X + 0.5f < 0) // LEFT
		{
			inputDirection.X = _BIGGIE_SPEED_X_RATIO;
			_isMoving = true;
			_frameIncrement -= 1;
			_currentFrameDirection = Enumerations.Movement.Directions.Right;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		if (direction.Z + 0.5f < 0) // UP
		{
			inputDirection.Z -= _BIGGIE_SPEED_Z_RATIO;
			_isMoving = true;
			_frameIncrement = 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}
		else if (direction.Z - 0.5f > 0) // DOWN
		{
			inputDirection.Z += _BIGGIE_SPEED_Z_RATIO;
			_isMoving = true;
			_frameIncrement = 1;
			_nodeBiggieSpriteMeshInstance.Call("set_frame", ReturnSpriteWalkFrame(_frameIncrement));
		}

		Velocity = inputDirection * _BIGGIE_SPEED;
		MoveAndCollide(Velocity * (float)delta);

		// opposite the other conditionals in this function
		bool atTarget = direction.X - 0.5f < 0
			&& direction.X + 0.5f > 0
			&& direction.Z + 0.5f > 0
			&& direction.Z - 0.5f < 0;
		CanMove(atTarget);
		//GD.Print($"Subconscious atTarget: {atTarget}");
		return atTarget;
	}
}
