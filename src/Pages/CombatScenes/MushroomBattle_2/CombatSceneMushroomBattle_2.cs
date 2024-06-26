using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CombatSceneMushroomBattle_2 : Node2D
{
	public static readonly StringName _SCENE_BIGGIE_DEFEAT = new StringName("res://Pages/DefeatScenes/DjBattle/DefeatSceneDjBattle.tscn");
	public static readonly StringName _SCENE_MUSHROOM_DEFEAT = new StringName("res://Pages/Levels/3D/Tutorial/DreamState/TherapistOffice1/Scene_TherapistOffice1.tscn");

	private static readonly int _MAX_HEALTH_PHYSICAL_BIGGIE = 9;
	private static readonly int _MAX_HEALTH_PHYSICAL_MUSHROOM = 9;
	private static readonly int _MAX_HEALTH_EMOTIONAL_MUSHROOM = 9;

	private Node2D _nodeSelf = null;
	private CombatWrapper _nodeCombatWrapper = null;
	private BiggieCombatMenu _nodeBiggieCombatMenu = null;
	private MushroomAttackContainer _nodeMushroomAttackContainer1 = null;
	private MushroomAttackContainer _nodeMushroomAttackContainer2 = null;
	private ChatterTextBox _nodeChatterTextBox = null;

	private const int EnemyTarget1Id = 0;
	private Node _nodeMushroomTarget1 = null;
	private Panel _nodeMushroomTarget1Panel = null;
	private Sprite2D _nodeMushroomSprite1 = null;
	public IEnemyAppearance MushroomAppearance1 { get; set; }

	private const int EnemyTarget2Id = 1;
	private Node _nodeMushroomTarget2 = null;
	private Panel _nodeMushroomTarget2Panel = null;
	private Sprite2D _nodeMushroomSprite2 = null;
	public IEnemyAppearance MushroomAppearance2 { get; set; }

	private CombatSingleton _globalCombatSingleton = null;
	private SaveStateService _serviceSaveState = null;

	private Queue<EnemyOpponent> EnemyOpponentQueue { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeCombatWrapper = GetNode<CombatWrapper>("./CombatWrapper");
		_nodeBiggieCombatMenu = GetNode<BiggieCombatMenu>("./CombatWrapper/BiggieCombatMenu");
		_nodeChatterTextBox = GetNode<ChatterTextBox>("./CombatWrapper/ChatterTextBox");

		_nodeMushroomAttackContainer1 = GetNode<MushroomAttackContainer>("./CombatWrapper/EnemyAttackContainer/EnemyAttackPanel/MushroomAttackContainer");
		_nodeMushroomTarget1 = GetNode<Node>("./CombatWrapper/Panel/MushroomTarget1");
		_nodeMushroomTarget1Panel = GetNode<Panel>("./CombatWrapper/Panel");
		_nodeMushroomSprite1 = GetNode<Sprite2D>("./CombatWrapper/Panel/MushroomTarget1/Sprite2D");
		MushroomAppearance1 = new BasicEnemyAppearance(_nodeMushroomSprite1);
		MushroomAppearance1.ApplyNeutralStyles();

		_nodeMushroomAttackContainer2 = GetNode<MushroomAttackContainer>("./CombatWrapper/EnemyAttackContainer/EnemyAttackPanel/MushroomAttackContainer2");
		_nodeMushroomTarget2 = GetNode<Node>("./CombatWrapper/Panel2/MushroomTarget2");
		_nodeMushroomTarget2Panel = GetNode<Panel>("./CombatWrapper/Panel2");
		_nodeMushroomSprite2 = GetNode<Sprite2D>("./CombatWrapper/Panel2/MushroomTarget2/Sprite2D");
		MushroomAppearance2 = new BasicEnemyAppearance(_nodeMushroomSprite2);
		MushroomAppearance2.ApplyNeutralStyles();

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		_globalCombatSingleton.NewBattle(_MAX_HEALTH_PHYSICAL_BIGGIE, _MAX_HEALTH_PHYSICAL_MUSHROOM, _MAX_HEALTH_EMOTIONAL_MUSHROOM);
		_globalCombatSingleton.AddEnemyTarget(EnemyTarget1Id, "Mushroom 1", _nodeMushroomTarget1Panel, MushroomAppearance1, 3, 2);
		_globalCombatSingleton.AddEnemyTarget(EnemyTarget2Id, "Mushroom 2", _nodeMushroomTarget2Panel, MushroomAppearance2, 3, 2);
		_globalCombatSingleton.CombatStateMachineService.SetCheckChatterConditions(CheckChatterConditions);
		HandleChangeBiggieHealthBar();

		_nodeCombatWrapper.StartBiggieTextTurn += HandleStartBiggieTextTurn;
		_nodeCombatWrapper.StartEnemyAttackTurn += HandleStartEnemyAttackTurn;
		_nodeCombatWrapper.ProjectPhysicalDamage += () => _globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		_nodeCombatWrapper.ProjectEmotionalDamage += () => _globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		_nodeCombatWrapper.BiggieDefeat += HandleBiggieDefeat;
		_nodeCombatWrapper.EnemyListPhysicalDefeat += HandleMushroomPhysicalDefeat;
		_nodeCombatWrapper.EnemyListEmotionalDefeat += HandleMushroomEmotionalDefeat;

		_nodeMushroomAttackContainer1.ProjectPhysicalDamage += HandleChangeBiggieHealthBar;
		_nodeMushroomAttackContainer1.EndEnemyAttackTurn += HandleEndEnemyAttackTurn;
		_nodeMushroomAttackContainer1.FramesPerRound = 600;
		_nodeMushroomAttackContainer1.HideAndDisableCollision();
		_nodeMushroomAttackContainer1.IsAttacking = false;

		_nodeMushroomAttackContainer2.ProjectPhysicalDamage += HandleChangeBiggieHealthBar;
		_nodeMushroomAttackContainer2.EndEnemyAttackTurn += HandleEndEnemyAttackTurn;
		_nodeMushroomAttackContainer2.FramesPerRound = 600;
		_nodeMushroomAttackContainer2.HideAndDisableCollision();
		_nodeMushroomAttackContainer2.IsAttacking = false;

		EnemyOpponentQueue = new Queue<EnemyOpponent>();
		HandleStartBiggieTextTurn();
	}

	#region Handlers

	public void HandleStartBiggieTextTurn()
	{
		_nodeBiggieCombatMenu.StartTurn();
		_nodeBiggieCombatMenu.Show();
	}

	public void HandleStartEnemyAttackTurn()
	{
		EnemyTarget enemyTarget1 =
			_globalCombatSingleton.EnemyTargetList
			.Where(x => x.Id == EnemyTarget1Id)
			.FirstOrDefault();
		GD.Print("HandleStartEnemyAttackTurn");
		if (enemyTarget1 != null)
		{
			GD.Print("enemyTarget1 != null");
			EnemyOpponentQueue.Enqueue(
				new EnemyOpponent()
				{
					EnemyTarget = enemyTarget1,
					EnemyAttackContainer = _nodeMushroomAttackContainer1,
					Appearance = MushroomAppearance1,
				}
			);
		}

		var enemyTarget2 =
			_globalCombatSingleton.EnemyTargetList
			.Where(x => x.Id == EnemyTarget2Id)
			.FirstOrDefault();
		if (enemyTarget2 != null)
		{
			GD.Print("enemyTarget2 != null");
			EnemyOpponentQueue.Enqueue(
				new EnemyOpponent()
				{
					EnemyTarget = enemyTarget2,
					EnemyAttackContainer = _nodeMushroomAttackContainer2,
					Appearance = MushroomAppearance2,
				}
			);
		}
		_globalCombatSingleton.EnemyTargetList.ForEach(x => x.Appearance.ApplyInactiveStyles());

		if (EnemyOpponentQueue.Any())
		{
			var enemyOpponent = EnemyOpponentQueue.Peek();
			// show
			enemyOpponent.EnemyAttackContainer.ShowAndEnableCollision();
			enemyOpponent.EnemyAttackContainer.StartTurn();
			enemyOpponent.Appearance.ApplyActiveStyles();
		}
	}

	public void HandleEndEnemyAttackTurn()
	{
		// remove and hide current
		if (EnemyOpponentQueue.Any())
		{
			var enemyOpponent = EnemyOpponentQueue.Dequeue();
			// hide
			enemyOpponent.EnemyAttackContainer.HideAndDisableCollision();
			enemyOpponent.Appearance.ApplyInactiveStyles();
		}

		// run next in queue
		if (EnemyOpponentQueue.Any())
		{
			var enemyOpponent = EnemyOpponentQueue.Peek();
			// show
			enemyOpponent.EnemyAttackContainer.ShowAndEnableCollision();
			enemyOpponent.EnemyAttackContainer.StartTurn();
			enemyOpponent.Appearance.ApplyActiveStyles();
		}
		else
		{
			_globalCombatSingleton.EnemyTargetList.ForEach(x => x.Appearance.ApplyNeutralStyles());
			_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishEnemyAttack);
		}
		return;
	}

	public void HandleChangeBiggieHealthBar()
	{
		_nodeCombatWrapper.ChangeBiggieHealthBar();
	}

	public void HandleBiggieDefeat()
	{
		GetTree().ChangeSceneToFile(_SCENE_BIGGIE_DEFEAT);
		return;
	}

	public void HandleMushroomPhysicalDefeat()
	{
		var context = _serviceSaveState.Load();
		context.IsDoubleMushroomDefeated = true;
		_serviceSaveState.Commit(context);

		GetTree().ChangeSceneToFile(_SCENE_MUSHROOM_DEFEAT);
		return;
	}

	public void HandleMushroomEmotionalDefeat()
	{
		var context = _serviceSaveState.Load();
		context.IsMushroomMoved = true;
		_serviceSaveState.Commit(context);

		GetTree().ChangeSceneToFile(_SCENE_MUSHROOM_DEFEAT);
		return;
	}

	#endregion

	private bool explainSpore = false;
	private bool ask1 = false;
	private bool ask2 = false;
	private bool charm1 = false;
	private bool charm2 = false;
	private bool scratch1 = false;
	private bool scratch2 = false;
	private bool bite1 = false;
	private bool bite2 = false;
	public bool CheckChatterConditions()
	{
		////GD.Print("CombatSceneMushroomBattle_1 CheckChatterConditions is false");
		var currState = _globalCombatSingleton.CombatStateMachineService.CurrentCombatState;

		if (currState.Id == Enumerations.Combat.StateMachine.States.BiggieChatSpecialAttack)
		{
			var enemyEmotionalHealthPercentage = _globalCombatSingleton.TargetedBiggieEmotionalAttackProxy.GetTargetHealthPercentage();
			if (enemyEmotionalHealthPercentage > 50 && !ask1)
			{
				_nodeChatterTextBox.AddDialogue("You ask whether the mushroom would prefer a spot away from the door.");
				_nodeChatterTextBox.AddDialogue("You get no response.");
				if (!explainSpore)
				{
					_nodeChatterTextBox.AddDialogue("The mushroom starts to expel some [color=red]damaging[/color] spores.");
					explainSpore = true;
				}
				_nodeChatterTextBox.ExecuteDialogueQueue();
				ask1 = true;
				return true;
			}
			else if (enemyEmotionalHealthPercentage > 0 && !ask2)
			{
				_nodeChatterTextBox.AddDialogue("You suggest that 2 feet to the right is more damp.");
				_nodeChatterTextBox.AddDialogue("The mushroom appears to shuffle its feet.”");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				ask2 = true;
				return true;
			}
			else if (_globalCombatSingleton.TargetedBiggieEmotionalAttackProxy.IsTargetDefeated())
			{
				_nodeChatterTextBox.AddDialogue("Persuaded by Biggie’s suggestion, the mushroom decides to move to the damp spot 2 feet away. The mycelium colony may remember this.");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				return true;
			}
		}

		if (currState.Id == Enumerations.Combat.StateMachine.States.BiggieChatSpecialChat)
		{
			var enemyEmotionalHealthPercentage = _globalCombatSingleton.TargetedBiggieEmotionalAttackProxy.GetTargetHealthPercentage();
			if (enemyEmotionalHealthPercentage > 50 && !charm1)
			{
				_nodeChatterTextBox.AddDialogue("The mushroom does not respond to your romantic gestures.");
				if (!explainSpore)
				{
					_nodeChatterTextBox.AddDialogue("The mushroom starts to expel some [color=red]damaging[/color] spores.");
					explainSpore = true;
				}
				_nodeChatterTextBox.ExecuteDialogueQueue();
				charm1 = true;
				return true;
			}
			else if (enemyEmotionalHealthPercentage > 0 && !charm2)
			{
				_nodeChatterTextBox.AddDialogue("The mushroom appears to be blushing.");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				charm2 = true;
				return true;
			}
			else if (_globalCombatSingleton.TargetedBiggieEmotionalAttackProxy.IsTargetDefeated())
			{
				_nodeChatterTextBox.AddDialogue("Taken by Biggie’s grace, the mushroom decides to let him pass. The mycelium colony may remember this.");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				return true;
			}
		}

		if (currState.Id == Enumerations.Combat.StateMachine.States.BiggieFightAttack)
		{
			var enemyPhysicalHealthPercentage = _globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.GetTargetHealthPercentage();
			if (enemyPhysicalHealthPercentage > 50 & !scratch1)
			{
				_nodeChatterTextBox.AddDialogue("The scratches irritate the mushroom. She seems a little more red than before.");
				if (!explainSpore)
				{
					_nodeChatterTextBox.AddDialogue("The mushroom starts to expel some [color=red]damaging[/color] spores.");
					explainSpore = true;
				}
				_nodeChatterTextBox.ExecuteDialogueQueue();
				scratch1 = true;
				return true;
			}
			else if (enemyPhysicalHealthPercentage > 0 && !scratch2)
			{
				_nodeChatterTextBox.AddDialogue("The mushroom is not looking so good.");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				scratch2 = true;
				return true;
			}
			else if (_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.IsTargetDefeated())
			{
				_nodeChatterTextBox.AddDialogue("Mushroom crumbs are scattered about the floor. The mycelium colony may remember this.");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				return true;
			}
		}

		if (currState.Id == Enumerations.Combat.StateMachine.States.BiggieFightChat)
		{
			var enemyPhysicalHealthPercentage = _globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.GetTargetHealthPercentage();
			if (enemyPhysicalHealthPercentage > 50 && !bite1)
			{
				_nodeChatterTextBox.AddDialogue("The mushroom does not taste very good.");
				_nodeChatterTextBox.AddDialogue("But maybe with some butter…");
				if (!explainSpore)
				{
					_nodeChatterTextBox.AddDialogue("The mushroom starts to expel some [color=red]damaging[/color] spores.");
					explainSpore = true;
				}
				_nodeChatterTextBox.ExecuteDialogueQueue();
				bite1 = true;
				return true;
			}
			else if (enemyPhysicalHealthPercentage > 0 && !bite2)
			{
				_nodeChatterTextBox.AddDialogue("Less and less mushroom stands between you and the door.");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				bite2 = true;
				return true;
			}
			else if (_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.IsTargetDefeated())
			{
				_nodeChatterTextBox.AddDialogue("Mushroom crumbs are scattered about the floor. The mycelium colony may remember this.");
				_nodeChatterTextBox.ExecuteDialogueQueue();
				return true;
			}
		}
		return false;
	}

	private static readonly StringName _NODE_MUSHROOM_ATTACK_CONTAINER = new StringName("res://ObjectLibrary/CombatSubjects/EnemyAttack/Mushroom/MushroomAttackContainer.tscn");
	private MushroomAttackContainer InstantiateMushroomAttackContainer()
	{
		var scene = GD.Load<PackedScene>(_NODE_MUSHROOM_ATTACK_CONTAINER);
		var instance = scene.Instantiate<MushroomAttackContainer>();
		return instance;
	}
}
