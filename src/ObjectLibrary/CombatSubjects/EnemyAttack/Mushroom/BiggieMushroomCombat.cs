using Godot;
using System;

public partial class BiggieMushroomCombat : CharacterBody2D
{
	private static readonly int _SPEED = 20;

	private static readonly StringName _MOVE_LEFT_INPUT = new StringName("move_left");
	private static readonly StringName _MOVE_UP_INPUT = new StringName("move_up");
	private static readonly StringName _MOVE_RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _MOVE_DOWN_INPUT = new StringName("move_down");

	private MushroomAttackContainer _nodeMushroomAttackContainer = null;
	private CollisionShape2D _nodeBiggieCollisionShape = null;

	private CombatSingleton _globalCombatSingleton = null;

	private Enumerations.Movement.Directions EnumDirection { get; set; }

	public override void _Ready()
	{
		_nodeMushroomAttackContainer = GetNode<MushroomAttackContainer>("..");
		_nodeBiggieCollisionShape = GetNode<CollisionShape2D>("./CollisionShape2D");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_nodeMushroomAttackContainer.IsAttacking) return;

		Vector2 inputDirection = GetInput();
		Velocity = inputDirection * _SPEED;
		MoveAndSlide();
	}

	private Vector2 GetInput()
	{
		Vector2 biggieSize = _nodeBiggieCollisionShape.Shape.GetRect().Size;
		Vector2 biggiePos = Position;
		Vector2 borderSize = _globalCombatSingleton.EnemyAttackPanelService.Size;
		Vector2 borderPos = _globalCombatSingleton.EnemyAttackPanelService.Position;

		Vector2 inputDirection = new Vector2();
		bool result = false;

		if (biggiePos.X - (biggieSize.X / 2) > borderPos.X && Input.IsActionPressed(_MOVE_LEFT_INPUT))
		{
			inputDirection.X = -_SPEED;
			result = true;
		}
		else if (biggiePos.X + (biggieSize.X / 2) < borderPos.X + borderSize.X && Input.IsActionPressed(_MOVE_RIGHT_INPUT))
		{
			inputDirection.X = _SPEED;
			result = true;
		}

		if (biggiePos.Y + (biggieSize.Y / 2) < borderPos.Y + borderSize.Y && Input.IsActionPressed(_MOVE_DOWN_INPUT))
		{
			inputDirection.Y += _SPEED;
			result = true;
		}
		else if (biggiePos.Y - (biggieSize.Y / 2) > borderPos.Y && Input.IsActionPressed(_MOVE_UP_INPUT))
		{
			inputDirection.Y = -_SPEED;
			result = true;
		}

		if (!result) inputDirection = Vector2.Zero;

		return inputDirection;
	}

	private bool IsHittingCombatBorders()
	{
		Vector2 biggieSize = _nodeBiggieCollisionShape.Shape.GetRect().Size;
		Vector2 biggiePos = Position;
		Vector2 borderSize = _globalCombatSingleton.EnemyAttackPanelService.Size;
		Vector2 borderPos = _globalCombatSingleton.EnemyAttackPanelService.Position;
		bool result = false;

		//GD.Print($"biggieSize {biggieSize.X} {biggieSize.Y}");
		//GD.Print($"biggiePos {biggiePos.X} {biggiePos.Y}");
		//GD.Print($"borderSize {borderSize.X} {borderSize.Y}");
		//GD.Print($"borderPos {borderPos.X} {borderPos.Y}");

		// hit up
		// biggiePos.y - biggieSize.y < pos.y
		if (biggiePos.Y < borderPos.Y && EnumDirection == Enumerations.Movement.Directions.Up)
		{
			////GD.Print($"IsHittingCombatBorders UP");
			result = true;
		}
		// hit right
		// biggiePos.x + biggieSize.x > pos.x + size.x
		if (biggiePos.X + biggieSize.X > borderPos.X + borderSize.X && EnumDirection == Enumerations.Movement.Directions.Right)
		{
			////GD.Print($"IsHittingCombatBorders RIGHT");
			result = true;
		}
		// hit bottom
		// biggiePos.y + biggieSize.y > pos.y + size.y 
		if (biggiePos.Y + biggieSize.Y > borderPos.Y + borderSize.Y && EnumDirection == Enumerations.Movement.Directions.Down)
		{
			////GD.Print($"IsHittingCombatBorders DOWN");
			result = true;
		}
		// hit left 
		// biggiePos.x < pos.x
		if (biggiePos.X < borderPos.X && EnumDirection == Enumerations.Movement.Directions.Left)
		{
			////GD.Print($"IsHittingCombatBorders LEFT");
			result = true;
		}

		////GD.Print($"IsHittingCombatBorders ${result}");
		return result;
	}
}
