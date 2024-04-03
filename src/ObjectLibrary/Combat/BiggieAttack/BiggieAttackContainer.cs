using Godot;
using System;

public partial class BiggieAttackContainer : MarginContainer
{
	#region Fight Zone Frame Details
	public static readonly int _SPRITESHEET_LENGTH = 119;
	public static readonly int _EARLY_TRASH_RANGE_START = 0;
	public static readonly int _EARLY_TRASH_RANGE_END = 9;
	public static readonly int _EARLY_BAD_RANGE_START = 10;
	public static readonly int _EARLY_BAD_RANGE_END = 36;
	public static readonly int _EARLY_GOOD_RANGE_START = 37;
	public static readonly int _EARLY_GOOD_RANGE_END = 53;
	public static readonly int _GREAT_RANGE_START = 54;
	public static readonly int _GREAT_RANGE_END = 58;
	public static readonly int _PERFECT_RANGE_START = 59;
	public static readonly int _PERFECT_RANGE_END = 59;
	public static readonly int _LATE_GOOD_RANGE_START = 60;
	public static readonly int _LATE_GOOD_RANGE_END = 64;
	public static readonly int _LATE_BAD_RANGE_START = 65;
	public static readonly int _LATE_BAD_RANGE_END = 81;
	public static readonly int _LATE_TRASH_RANGE_START = 82;
	public static readonly int _LATE_TRASH_RANGE_END = 118;
	#endregion

	private static readonly StringName _SPACE_INPUT = new StringName("interact");

	private MarginContainer _nodeSelf = null;
	private Panel _nodePanel = null;
	private Sprite2D _nodeFightZoneSprite2D = null;

	private int ZoneSpriteIndex { get; set; }
	public bool IsActive { get; set; }

	private enum DamageZones
	{
		Trash = 0,
		Bad = 1,
		Good = 2,
		Perfect = 3,
	}

	public override void _Ready()
	{
		ZoneSpriteIndex = 0;

		_nodeSelf = GetNode<MarginContainer>(".");
		_nodePanel = GetNode<Panel>("./BiggieAttackPanel");
		_nodeFightZoneSprite2D = GetNode<Sprite2D>("FightZoneSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		ZoneSpriteIndex += 1;
		if (ZoneSpriteIndex == _SPRITESHEET_LENGTH) ZoneSpriteIndex = 0;
		_nodeFightZoneSprite2D.Frame = ZoneSpriteIndex;

		if (Input.IsActionJustPressed(_SPACE_INPUT))
		{
			DealDamage(ZoneSpriteIndex);
		}
	}

	[Signal]
	public delegate void EndBiggieAttackTurnEventHandler(double damagePercentage);
	private void DealDamage(int frameIndex)
	{
		var damageZone = GetDamageZone(frameIndex);
		var damagePercentage = GetDamagePercentage(damageZone);
		EmitSignal(SignalName.EndBiggieAttackTurn, damagePercentage);
	}

	private DamageZones GetDamageZone(int frameIndex)
	{
		bool isTrash = (_EARLY_TRASH_RANGE_START <= frameIndex && frameIndex <= _EARLY_TRASH_RANGE_END)
						|| (_LATE_TRASH_RANGE_START <= frameIndex && frameIndex <= _LATE_TRASH_RANGE_END);
		bool isBad = (_EARLY_BAD_RANGE_START <= frameIndex && frameIndex <= _EARLY_BAD_RANGE_END)
						|| (_LATE_BAD_RANGE_START <= frameIndex && frameIndex <= _LATE_BAD_RANGE_END);
		bool isGood = (_EARLY_GOOD_RANGE_START <= frameIndex && frameIndex <= _EARLY_BAD_RANGE_END)
						|| (_LATE_GOOD_RANGE_START <= frameIndex && frameIndex <= _LATE_GOOD_RANGE_END);
		bool isPerfect = _PERFECT_RANGE_START <= frameIndex && frameIndex <= _PERFECT_RANGE_END;

		if (isTrash)
		{
			GD.Print("DamageZones.Trash");
			return DamageZones.Trash;
		}
		else if (isBad)
		{
			GD.Print("DamageZones.Bad");
			return DamageZones.Bad;
		}
		else if (isGood)
		{
			GD.Print("DamageZones.Good");
			return DamageZones.Good;
		}
		else if (isPerfect)
		{
			GD.Print("DamageZones.Perfect");
			return DamageZones.Perfect;
		}

		GD.Print("DamageZones fallback");
		return DamageZones.Trash;
	}

	private double GetDamagePercentage(DamageZones damageZone)
	{
		double result = 0.0;
		switch (damageZone)
		{
			case DamageZones.Trash:
				result = 0.0;
				break;
			case DamageZones.Bad:
				result = 33.4;
				break;
			case DamageZones.Good:
				result = 66.7;
				break;
			case DamageZones.Perfect:
				result = 100.0;
				break;
			default:
				break;
		}
		return result;
	}
}

