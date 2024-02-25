using Godot;
using System;

public partial class BaseArrowUp : Area2D
{
	private static readonly StringName _UP_INPUT = new StringName("move_up");
	
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
		if (Input.IsActionJustPressed(_UP_INPUT))
		{
			GD.Print("UP pressed.");
			if (_nodeAreaEarlyBad.HasOverlappingBodies())
			{
				GD.Print("UP early bad.");
				HandleCollision(Enumerations.HitType.Bad);
			}
			else if (_nodeAreaEarlyGood.HasOverlappingBodies())
			{
				GD.Print("UP early good.");
				HandleCollision(Enumerations.HitType.Good);
			}
			else if (_nodeAreaPerfect.HasOverlappingBodies())
			{
				GD.Print("UP perfect.");
				HandleCollision(Enumerations.HitType.Perfect);
			}
			else if (_nodeAreaLateGood.HasOverlappingBodies())
			{
				GD.Print("UP late good.");
				HandleCollision(Enumerations.HitType.Good);
			}
			else if (_nodeAreaLateBad.HasOverlappingBodies())
			{
				GD.Print("UP late bad.");
				HandleCollision(Enumerations.HitType.Bad);
			}
			else 
			{
				GD.Print("UP miss.");
				HandleCollision(Enumerations.HitType.Miss);
			}
		}
	}

	[Signal]
	public delegate void DequeueFallingArrowUpEventHandler(int hit);

	public void HandleCollision(Enumerations.HitType hit)
	{
		GD.Print("Up HandleCollision");
		EmitSignal(SignalName.DequeueFallingArrowUp, (int)hit);
		return;
	}
}
