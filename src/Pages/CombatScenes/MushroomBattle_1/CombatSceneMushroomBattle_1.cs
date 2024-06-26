using Godot;
using System;
using System.Collections.Generic;

public partial class CombatSceneMushroomBattle_1 : Node2D
{
	public static readonly StringName _SCENE_BIGGIE_DEFEAT = new StringName("res://Pages/DefeatScenes/DjBattle/DefeatSceneDjBattle.tscn");
	public static readonly StringName _SCENE_MUSHROOM_FIGHT = new StringName("res://Pages/Levels/3D/Tutorial/DreamState/MushroomFight/Scene_MushroomFight.tscn");

	private static readonly int _MAX_HEALTH_PHYSICAL_BIGGIE = 9;
	private static readonly int _MAX_HEALTH_PHYSICAL_MUSHROOM = 9;
	private static readonly int _MAX_HEALTH_EMOTIONAL_MUSHROOM = 9;

	private Node2D _nodeSelf = null;
	private CombatWrapper _nodeCombatWrapper = null;
	private BiggieCombatMenu _nodeBiggieCombatMenu = null;
	private MushroomAttackContainer _nodeMushroomAttackContainer = null;
	private ChatterTextBox _nodeChatterTextBox = null;

	private Node _nodeMushroomTarget1 = null;
	private Panel _nodeMushroomTarget1Panel = null;
	private Sprite2D _nodeMushroomSprite1 = null;
	public IEnemyAppearance MushroomAppearance1 { get; set; }

	private CombatSingleton _globalCombatSingleton = null;
	private SaveStateService _serviceSaveState = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeCombatWrapper = GetNode<CombatWrapper>("./CombatWrapper");
		_nodeBiggieCombatMenu = GetNode<BiggieCombatMenu>("./CombatWrapper/BiggieCombatMenu");
		_nodeChatterTextBox = GetNode<ChatterTextBox>("./CombatWrapper/ChatterTextBox");


		_nodeMushroomAttackContainer = GetNode<MushroomAttackContainer>("./CombatWrapper/EnemyAttackContainer/EnemyAttackPanel/MushroomAttackContainer");
		_nodeMushroomTarget1 = GetNode<Node>("./CombatWrapper/Panel/MushroomTarget1");
		_nodeMushroomTarget1Panel = GetNode<Panel>("./CombatWrapper/Panel");
		_nodeMushroomSprite1 = GetNode<Sprite2D>("./CombatWrapper/Panel/MushroomTarget1/Sprite2D");
		MushroomAppearance1 = new BasicEnemyAppearance(_nodeMushroomSprite1);
		MushroomAppearance1.ApplyNeutralStyles();

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		_globalCombatSingleton.NewBattle(_MAX_HEALTH_PHYSICAL_BIGGIE, _MAX_HEALTH_PHYSICAL_MUSHROOM, _MAX_HEALTH_EMOTIONAL_MUSHROOM);
		_globalCombatSingleton.AddEnemyTarget(0, "Mushroom 1", _nodeMushroomTarget1Panel, MushroomAppearance1, 9, 9);
		_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy = _globalCombatSingleton.EnemyTargetList[0].BiggiePhysicalAttackProxy;
		_globalCombatSingleton.TargetedBiggieEmotionalAttackProxy = _globalCombatSingleton.EnemyTargetList[0].BiggieEmotionalAttackProxy;
		_globalCombatSingleton.CombatStateMachineService.SetCheckChatterConditions(CheckChatterConditions);
		ChangeBiggieHealthBar();

		_nodeCombatWrapper.StartBiggieTextTurn += StartBiggieTextTurn;
		_nodeCombatWrapper.StartEnemyAttackTurn += StartEnemyAttackTurn;
		_nodeCombatWrapper.ProjectPhysicalDamage += () => _globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		_nodeCombatWrapper.ProjectEmotionalDamage += () => _globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		_nodeCombatWrapper.BiggieDefeat += HandleBiggieDefeat;
		_nodeCombatWrapper.EnemyListPhysicalDefeat += HandleMushroomPhysicalDefeat;
		_nodeCombatWrapper.EnemyListEmotionalDefeat += HandleMushroomEmotionalDefeat;
		_nodeMushroomAttackContainer.ProjectPhysicalDamage += ChangeBiggieHealthBar;
		_nodeMushroomAttackContainer.EndEnemyAttackTurn += EndEnemyAttackTurn;
		_nodeMushroomAttackContainer.FramesPerRound = 600;

		_nodeMushroomAttackContainer.Hide();
		_nodeMushroomAttackContainer.IsAttacking = false;
		StartBiggieTextTurn();
	}

	public void StartBiggieTextTurn()
	{
		_nodeBiggieCombatMenu.StartTurn();
		_nodeBiggieCombatMenu.Show();
	}

	public void StartEnemyAttackTurn()
	{
		_nodeMushroomAttackContainer.Visible = true;
		//_nodeHitCallout.Visible = true;
		_nodeMushroomAttackContainer.StartTurn();
		//GD.Print("CombatSceneMushroomBattle_1 StartEnemyAttackTurn");
	}

	public void EndEnemyAttackTurn()
	{
		_nodeMushroomAttackContainer.Visible = false;
		//_nodeHitCallout.Visible = false;
		//_nodeMushroomAttackContainer.EndTurn();
		ChangeBiggieHealthBar();

		if (_globalCombatSingleton.EnemyPhysicalAttackProxy.IsTargetDefeated())
		{
			////GD.Print("Biggie Physical Defeat");
			HandleBiggieDefeat();
			return;
		}

		_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishEnemyAttack);
		return;
	}

	public void ChangeBiggieHealthBar()
	{
		////GD.Print("Start ChangeBiggieHealthBar");
		_nodeCombatWrapper.ChangeBiggieHealthBar();
		////GD.Print($"End ChangeBiggieHealthBar {_nodeBiggieHealthBar.Value}");
	}

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
			if (enemyEmotionalHealthPercentage > 0.50f && !ask1)
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
			if (enemyEmotionalHealthPercentage > 0.50f && !charm1)
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
			if (enemyPhysicalHealthPercentage > 0.50f & !scratch1)
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
			if (enemyPhysicalHealthPercentage > 0.50f && !bite1)
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

	public bool CheckForBiggeDefeat()
	{
		return _globalCombatSingleton.EnemyPhysicalAttackProxy.IsTargetDefeated() && !_nodeChatterTextBox.IsOpen();
	}

	public bool CheckForMushroomPhysicalDefeat()
	{
		return !_nodeChatterTextBox.IsOpen()
			&& _globalCombatSingleton.AreAnyEnemiesPhysicallyDefeated()
			&& _globalCombatSingleton.AreAllEnemiesDefeated();
	}

	public bool CheckForMushroomEmotionalDefeat()
	{
		return !_nodeChatterTextBox.IsOpen()
			&& !_globalCombatSingleton.AreAnyEnemiesPhysicallyDefeated()
			&& _globalCombatSingleton.AreAllEnemiesDefeated();
	}

	public void HandleBiggieDefeat()
	{
		GetTree().ChangeSceneToFile(_SCENE_BIGGIE_DEFEAT);
		return;
	}

	public void HandleMushroomPhysicalDefeat()
	{
		var context = _serviceSaveState.Load();
		context.IsMushroomDead = true;
		_serviceSaveState.Commit(context);

		GetTree().ChangeSceneToFile(_SCENE_MUSHROOM_FIGHT);
		return;
	}

	public void HandleMushroomEmotionalDefeat()
	{
		var context = _serviceSaveState.Load();
		context.IsMushroomMoved = true;
		_serviceSaveState.Commit(context);

		GetTree().ChangeSceneToFile(_SCENE_MUSHROOM_FIGHT);
		return;
	}
}
