using Godot;
using System;

public partial class BaseArrowRight : Area2D
{
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

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
		if (Input.IsActionJustPressed(_RIGHT_INPUT))
		{
			//////GD.Print("RIGHT pressed.");
			if (_nodeAreaEarlyBad.HasOverlappingBodies())
			{
				//////GD.Print("RIGHT early bad.");
				HandleCollision(Enumerations.HitType.Bad);
			}
			else if (_nodeAreaEarlyGood.HasOverlappingBodies())
			{
				//////GD.Print("RIGHT early good.");
				HandleCollision(Enumerations.HitType.Good);
			}
			else if (_nodeAreaPerfect.HasOverlappingBodies())
			{
				//////GD.Print("RIGHT perfect.");
				HandleCollision(Enumerations.HitType.Perfect);
			}
			else if (_nodeAreaLateGood.HasOverlappingBodies())
			{
				//////GD.Print("RIGHT late good.");
				HandleCollision(Enumerations.HitType.Good);
			}
			else if (_nodeAreaLateBad.HasOverlappingBodies())
			{
				//////GD.Print("RIGHT late bad.");
				HandleCollision(Enumerations.HitType.Bad);
			}
			else
			{
				//////GD.Print("RIGHT miss.");
				HandleCollision(Enumerations.HitType.Miss);
			}
		}
	}

	[Signal]
	public delegate void DequeueFallingArrowRightEventHandler(int hit);

	public void HandleCollision(Enumerations.HitType hit)
	{
		//////GD.Print("Right HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowRight, (int)hit);
		return;
	}
}
