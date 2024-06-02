using Godot;
using System;

public partial class BaseArrowLeft : Area2D
{
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");

	private Area2D _nodeSelf = null;
	private Area2D _nodeAreaEarlyBad = null;
	private Area2D _nodeAreaEarlyGood = null;
	private Area2D _nodeAreaPerfect = null;
	private Area2D _nodeAreaLateGood = null;
	private Area2D _nodeAreaLateBad = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Area2D>(".");
		_nodeAreaEarlyBad = GetNode<Area2D>("./VBoxContainer/AreaEarlyBad");
		_nodeAreaEarlyGood = GetNode<Area2D>("./VBoxContainer/AreaEarlyGood");
		_nodeAreaPerfect = GetNode<Area2D>("./VBoxContainer/AreaPerfect");
		_nodeAreaLateGood = GetNode<Area2D>("./VBoxContainer/AreaLateGood");
		_nodeAreaLateBad = GetNode<Area2D>("./VBoxContainer/AreaLateBad");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// https://forum.godotengine.org/t/what-is-the-difference-between-physicsprocess-and-process-c/25549
	// https://www.reddit.com/r/godot/comments/178d9hz/area2d_get_overlapping_bodies_is_not_detecting/
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed(_LEFT_INPUT))
		{
			//////GD.Print("LEFT pressed.");
			if (_nodeAreaEarlyBad.HasOverlappingBodies())
			{
				//////GD.Print("LEFT early bad.");
				HandleCollision(Enumerations.HitType.Bad);
			}
			else if (_nodeAreaEarlyGood.HasOverlappingBodies())
			{
				//////GD.Print("LEFT early good.");
				HandleCollision(Enumerations.HitType.Good);
			}
			else if (_nodeAreaPerfect.HasOverlappingBodies())
			{
				//////GD.Print("LEFT perfect.");
				HandleCollision(Enumerations.HitType.Perfect);
			}
			else if (_nodeAreaLateGood.HasOverlappingBodies())
			{
				//////GD.Print("LEFT late good.");
				HandleCollision(Enumerations.HitType.Good);
			}
			else if (_nodeAreaLateBad.HasOverlappingBodies())
			{
				//////GD.Print("LEFT late bad.");
				HandleCollision(Enumerations.HitType.Bad);
			}
			else
			{
				//////GD.Print("LEFT miss.");
				HandleCollision(Enumerations.HitType.Miss);
			}
		}
	}

	[Signal]
	public delegate void DequeueFallingArrowLeftEventHandler(int hit);

	public void HandleCollision(Enumerations.HitType hit)
	{
		//////GD.Print("Left HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowLeft, (int)hit);
		return;
	}
}
