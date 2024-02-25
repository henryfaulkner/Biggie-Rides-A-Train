using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class DjAttackContainer : MarginContainer
{
	private static readonly int _BPM = 120;
	private static readonly int _FPS = 60;
	private static readonly int _SECONDS_IN_A_MINUTE = 60;
	private TimeSpan OneOverBPS => TimeSpan.FromSeconds(60 / _BPM);

	private MarginContainer _nodeSelf = null;
	private BiggieDjCombat _nodeBiggie = null;
	private BaseArrowUp _nodeBaseArrowUp = null;
	private BaseArrowRight _nodeBaseArrowRight = null;
	private BaseArrowDown _nodeBaseArrowDown = null;
	private BaseArrowLeft _nodeBaseArrowLeft = null;

	private CompositionParsingService CompositionParsingService { get; set; }
	private FallingArrowFactory FallingArrowFactory { get; set; }
	private Queue<CharacterBody2D> FallingArrowUpQueue { get; set; }
	private Queue<CharacterBody2D> FallingArrowRightQueue { get; set; }
	private Queue<CharacterBody2D> FallingArrowDownQueue { get; set; }
	private Queue<CharacterBody2D> FallingArrowLeftQueue { get; set; }

	public bool IsAttacking { get; set; }
	public Enumerations.DjCombatRounds CurrentRound { get; set; }
	public int MissCount { get; set; }
	public int BadCount { get; set; }
	public int GoodCount { get; set; }
	public int PerfectCount { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<MarginContainer>(".");
		_nodeBiggie = GetNode<BiggieDjCombat>("./BiggieDjCombat");
		_nodeBaseArrowUp = GetNode<BaseArrowUp>("./BaseArrowUp");
		_nodeBaseArrowRight = GetNode<BaseArrowRight>("./BaseArrowRight");
		_nodeBaseArrowDown = GetNode<BaseArrowDown>("./BaseArrowDown");
		_nodeBaseArrowLeft = GetNode<BaseArrowLeft>("./BaseArrowLeft");

		FallingArrowFactory = new FallingArrowFactory(
			_nodeBaseArrowUp.Position,
			_nodeBaseArrowRight.Position,
			_nodeBaseArrowDown.Position,
			_nodeBaseArrowLeft.Position
		);
		FallingArrowUpQueue = new Queue<CharacterBody2D>();
		FallingArrowRightQueue = new Queue<CharacterBody2D>();
		FallingArrowDownQueue = new Queue<CharacterBody2D>();
		FallingArrowLeftQueue = new Queue<CharacterBody2D>();

		FrameCounter = 0;
		// String.Concat(charList);  
		CompositionParsingService = new CompositionParsingService();
		CompositionParsingService.SetNewComposition("composition-1.txt");
		IsAttacking = true;
		CurrentRound = Enumerations.DjCombatRounds.RoundOne;

		_nodeBiggie.DequeueFallingArrowUp += DequeueUpAndCountMiss;
		_nodeBiggie.DequeueFallingArrowRight += DequeueRightAndCountMiss;
		_nodeBiggie.DequeueFallingArrowDown += DequeueDownAndCountMiss;
		_nodeBiggie.DequeueFallingArrowLeft += DequeueLeftAndCountMiss;

		_nodeBaseArrowUp.DequeueFallingArrowUp += DequeueUpAndCountHit;
		_nodeBaseArrowRight.DequeueFallingArrowRight += DequeueRightAndCountHit;
		_nodeBaseArrowDown.DequeueFallingArrowDown += DequeueDownAndCountHit;
		_nodeBaseArrowLeft.DequeueFallingArrowLeft += DequeueLeftAndCountHit;
	}

	private int FrameCounter { get; set; }
	public override void _Process(double delta)
	{
		if ((FrameCounter % (_FPS * _SECONDS_IN_A_MINUTE / _BPM)) == 0)
		{
			if (IsAttacking)
			{
				switch (CurrentRound)
				{
					case Enumerations.DjCombatRounds.RoundOne:
						AttackRoundOne();
						break;
					case Enumerations.DjCombatRounds.RoundTwo:
						break;
					default:
						break;
				}
			}
		}

		FrameCounter += 1;
	}

	public void AttackRoundOne()
	{
		var nextToken = CompositionParsingService.GetNextToken();
		if (nextToken.Count > 0 && nextToken[0] == CompositionTokens.EOC)
		{
			CompositionParsingService.SetNewComposition("composition-1.txt");
			IsAttacking = false;
			CurrentRound = Enumerations.DjCombatRounds.RoundTwo;
			return;
		}
		GenerateAttack(nextToken);
	}

	private void GenerateAttack(List<char> tokens)
	{
		if (CompositionParsingService.IsBeatToken(tokens[0]))
		{
			switch (tokens[0])
			{
				case CompositionTokens.Beat:
					GD.Print("Beat");
					break;
				case CompositionTokens.TwoBeats:
					GD.Print("TwoBeats");
					break;
				case CompositionTokens.FourBeats:
					GD.Print("FourBeats");
					break;
				default:
					GD.Print("A Beat Token did not map.");
					break;
			}
			return;
		}
		else
		{
			string str = "";
			foreach (char token in tokens)
			{
				switch (token)
				{
					case CompositionTokens.Up:
						str = str + "Up";
						var instanceUp = FallingArrowFactory.SpawnFallingArrowUp();
						FallingArrowUpQueue.Enqueue(instanceUp);
						_nodeSelf.AddChild(instanceUp);
						break;
					case CompositionTokens.Right:
						str = str + "Right";
						var instanceRight = FallingArrowFactory.SpawnFallingArrowRight();
						FallingArrowRightQueue.Enqueue(instanceRight);
						_nodeSelf.AddChild(instanceRight);
						break;
					case CompositionTokens.Down:
						str = str + "Down";
						var instanceDown = FallingArrowFactory.SpawnFallingArrowDown();
						FallingArrowDownQueue.Enqueue(instanceDown);
						_nodeSelf.AddChild(instanceDown);
						break;
					case CompositionTokens.Left:
						str = str + "Left";
						var instanceLeft = FallingArrowFactory.SpawnFallingArrowLeft();
						FallingArrowLeftQueue.Enqueue(instanceLeft);
						_nodeSelf.AddChild(instanceLeft);
						break;
					default:
						GD.Print("A FallingArrow Token did not map.");
						break;
				}
			}
			GD.Print(str);
		}
	}

	public void DequeueUpAndCountMiss() { DequeueUpAndCountHit((int)Enumerations.HitType.Miss); }
	public void DequeueUpAndCountHit(int hitInt)
	{
		GD.Print($"DequeueUpAndCountHit {hitInt}");
		var hit = (Enumerations.HitType)hitInt;
		IncrementHitCount(hit);
		var instance = FallingArrowUpQueue.Dequeue();
		instance.QueueFree();
	}

	public void DequeueRightAndCountMiss() { DequeueRightAndCountHit((int)Enumerations.HitType.Miss); }	
	public void DequeueRightAndCountHit(int hitInt)
	{
		GD.Print($"DequeueRightAndCountHit {hitInt}");
		var hit = (Enumerations.HitType)hitInt;
		IncrementHitCount(hit);
		var instance = FallingArrowRightQueue.Dequeue();
		instance.QueueFree();
	}
	
	public void DequeueDownAndCountMiss() { DequeueDownAndCountHit((int)Enumerations.HitType.Miss); }	
	public void DequeueDownAndCountHit(int hitInt)
	{
		GD.Print($"DequeueDownAndCountHit {hitInt}");
		var hit = (Enumerations.HitType)hitInt;
		IncrementHitCount(hit);
		var instance = FallingArrowDownQueue.Dequeue();
		instance.QueueFree();
	}

	public void DequeueLeftAndCountMiss() { DequeueLeftAndCountHit((int)Enumerations.HitType.Miss); }	
	public void DequeueLeftAndCountHit(int hitInt)
	{
		GD.Print($"DequeueLeftAndCountHit {hitInt}");
		var hit = (Enumerations.HitType)hitInt;
		IncrementHitCount(hit);
		var instance = FallingArrowLeftQueue.Dequeue();
		instance.QueueFree();
	}

	private void IncrementHitCount(Enumerations.HitType hit)
	{
		switch (hit)
		{
			case Enumerations.HitType.Miss:
				MissCount += 1;
				break;
			case Enumerations.HitType.Bad:
				BadCount += 1;
				break;
			case Enumerations.HitType.Good:
				GoodCount += 1;
				break;
			case Enumerations.HitType.Perfect:
				PerfectCount += 1;
				break;
			default:
				GD.Print("A HitType did not map.");
				break;
		}
	}
}
