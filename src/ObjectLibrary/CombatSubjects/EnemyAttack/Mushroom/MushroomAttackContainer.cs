using Godot;
using System;

public partial class MushroomAttackContainer : EnemyAttackContainer
{
	private static readonly StringName _NODE_SPORE_FALL = new StringName("res://ObjectLibrary/CombatSubjects/EnemyAttack/Mushroom/SporeFall.tscn");

	private static readonly int _FPS = 60;
	private static readonly int _SECONDS_IN_A_MINUTE = 60;

	public bool IsAttacking { get; set; }
	private SporeFall SporeFallInstance { get; set; }
	private int FrameIndex { get; set; }

	public int FramesPerRound { get; set; }

	private AudioStreamPlayer _nodeHitAudio = null;

	public override void _Ready()
	{
		_nodeHitAudio = GetNode<AudioStreamPlayer>("./Hit_AudioStreamPlayer");
	}

	public override void _PhysicsProcess(double _delta)
	{
		ProcessTurn();
	}

	public override void StartTurn()
	{
		GD.Print("MushroomAttackContainer StartTurn");
		IsAttacking = true;
		SporeFallInstance = SpawnSporeFall();
		AddChild(SporeFallInstance);
		SporeFallInstance.SporeHitBiggie += HandleSporeHitBiggie;
	}

	public override void ProcessTurn()
	{
		if (FrameIndex == FramesPerRound)
		{
			EndTurn();
			FrameIndex = 0;
		}

		if (IsAttacking)
		{
			FrameIndex += 1;
		}
	}

	public override void EndTurn()
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

	private void HandleSporeHitBiggie()
	{
		_nodeHitAudio.Play();
		EmitSignal(SignalName.ProjectPhysicalDamage);
	}
}
