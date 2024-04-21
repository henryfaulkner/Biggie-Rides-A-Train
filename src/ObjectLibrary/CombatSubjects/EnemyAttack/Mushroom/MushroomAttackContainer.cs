using Godot;
using System;

public partial class MushroomAttackContainer : Node2D
{
	private static readonly StringName _NODE_SPORE_FALL = new StringName("res://ObjectLibrary/CombatSubjects/EnemyAttack/Mushroom/SporeFall.tscn");

	private static readonly int _FPS = 60;
	private static readonly int _SECONDS_IN_A_MINUTE = 60;

	public bool IsAttacking { get; set; }
	private SporeFall SporeFallInstance { get; set; }
	private int FrameIndex { get; set; }

	public override void _PhysicsProcess(double _delta)
	{
		if (FrameIndex == 360)
		{
			EndTurn();
			FrameIndex = 0;
		}

		if (IsAttacking)
		{
			FrameIndex += 1;
		}
	}

	public void StartTurn()
	{
		GD.Print("MushroomAttackContainer StartTurn");
		IsAttacking = true;
		SporeFallInstance = SpawnSporeFall();
		AddChild(SporeFallInstance);
	}

	public void EndTurn()
	{
		GD.Print("MushroomAttackContainer EndTurn");
		IsAttacking = false;
		EmitSignal(SignalName.EndEnemyAttackTurn);
		SporeFallInstance.QueueFree();
	}

	[Signal]
	public delegate void ProjectPhysicalDamageEventHandler();
	[Signal]
	public delegate void EndEnemyAttackTurnEventHandler();

	public SporeFall SpawnSporeFall()
	{
		var scene = GD.Load<PackedScene>(_NODE_SPORE_FALL);
		var instance = scene.Instantiate<SporeFall>();
		return instance;
	}
}
