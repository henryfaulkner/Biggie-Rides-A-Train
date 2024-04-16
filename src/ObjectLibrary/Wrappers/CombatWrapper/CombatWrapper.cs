using Godot;
using System;

public partial class CombatWrapper : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Panel _nodeSubjectPanel = null;
	private Panel _nodeBasePagePanel = null;
	private FightPageBasePanel _nodeFightPageBasePanel = null;
	private ChatPageBasePanel _nodeChatPageBasePanel = null;
	private HBoxContainer _nodeHudContainerSubject = null;
	private HBoxContainer _nodeHudContainerTargetEnemyAttack = null;
	private HBoxContainer _nodeHudContainerTargetBiggieAttack = null;
	private HBoxContainer _nodeHudContainerTargetBiggieCombatMenu = null;

	private MarginContainer _nodeActionInfo = null;
	private BiggieCombatMenu _nodeBiggieCombatMenu = null;
	private MarginContainer _nodeBiggieCombatMenuTextContainer = null;
	private MarginContainer _nodeEnemyAttackContainer = null;
	private Panel _nodeEnemyAttackPanel = null;
	private BiggieAttackContainer _nodeBiggieAttackContainer = null;
	private Panel _nodeBiggieAttackPanel = null;

	private PanelAnimationHelper HudAnimationHelper { get; set; }
	private static readonly float _HUD_SPEED_SLOW = 5f;
	private static readonly float _HUD_SPEED_FAST = 8f;
	private PanelAnimationHelper MainAnimationHelper { get; set; }
	private static readonly float _MAIN_SPEED = 20f;

	private CombatSingleton _globalCombatSingleton = null;
	private Enumerations.Combat.CombatOptions LastCombatOptionUsed { get; set; }

	public override void _Ready()
	{
		// note the containers wrapping the panels must have the same position and size
		// the containers can use margin to change panel size for animation
		_nodeSubjectPanel = GetNode<Panel>("./TransformContainer/TransformPanel");
		_nodeBasePagePanel = GetNode<Panel>("./BiggieCombatMenu/TextBoxContainer/BasePagePanel");
		_nodeFightPageBasePanel = GetNode<FightPageBasePanel>("./BiggieCombatMenu/TextBoxContainer/FightPagePanel");
		_nodeChatPageBasePanel = GetNode<ChatPageBasePanel>("./BiggieCombatMenu/TextBoxContainer/ChatPagePanel");
		_nodeHudContainerSubject = GetNode<HBoxContainer>("./HudContainer");
		// Need to Queue Free this at some point
		_nodeHudContainerTargetEnemyAttack = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 250);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetBiggieAttack = CreateHudContainerTarget(_nodeHudContainerSubject, 0, -160);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetBiggieCombatMenu = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 0);
		HudAnimationHelper = new PanelAnimationHelper(_HUD_SPEED_SLOW);
		MainAnimationHelper = new PanelAnimationHelper(_MAIN_SPEED);

		_nodeActionInfo = GetNode<MarginContainer>("HudContainer/ActionInfo");
		_nodeBiggieCombatMenu = GetNode<BiggieCombatMenu>("./BiggieCombatMenu");
		_nodeBiggieCombatMenuTextContainer = GetNode<MarginContainer>("./BiggieCombatMenu/TextBoxContainer");
		_nodeEnemyAttackContainer = GetNode<MarginContainer>("./EnemyAttackContainer");
		_nodeEnemyAttackPanel = GetNode<Panel>("./EnemyAttackContainer/EnemyAttackPanel");
		_nodeBiggieAttackContainer = GetNode<BiggieAttackContainer>("./BiggieAttackContainer");
		_nodeBiggieAttackPanel = GetNode<Panel>("./BiggieAttackContainer/BiggieAttackPanel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");

		_nodeFightPageBasePanel.SelectFight += HandleFightSelection;
		_nodeChatPageBasePanel.SelectChat += HandleChatSelection;
		_nodeBiggieCombatMenu.EndBiggieCombatMenuTurn += HandleEndBiggieCombatMenuTurn;
		_nodeBiggieCombatMenu.ShowActionInfo += ShowActionInfo;
		_nodeBiggieCombatMenu.HideActionInfo += HideActionInfo;
		_nodeBiggieAttackContainer.EndBiggieAttackTurn += HandleEndBiggieAttackTurn;

		//ApplyCombatStateMachineEvents();

		HideEnemyAttackContainer();
		HideBiggieAttackContainer();
		HideActionInfo();

		FirstFramePass = true;
	}

	private bool FirstFramePass { get; set; }
	private bool ProcessFirstPass()
	{
		if (FirstFramePass)
		{
			ShowSubjectPanel();
			FirstFramePass = false;
			return true;
		}
		return false;
	}

	public override void _Process(double delta)
	{
		bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
		var currStateId = _globalCombatSingleton.CombatStateMachineService.CurrentCombatState.Id;

		if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatAsk
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatCharm
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightBite
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightScratch
		)
		{
			if (!ProcessFirstPass())
			{
				HideBiggieAttackContainer();
				_nodeBiggieAttackContainer.IsActive = false;
				if (TransitionToEnemyAttack(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartEnemyAttackTurn);
					ShowEnemyAttackContainer();
					EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
					//GD.Print("EmitSignal(SignalName.StartOpponentTurn);");
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieCombatMenu)
		{
			if (!ProcessFirstPass())
			{
				HideEnemyAttackContainer();
				if (TransitionToBiggieCombatMenu(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartBiggieTextTurn);
					ShowBiggieCombatMenuTextContainer();
					EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightScratch
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightBite)
		{
			if (!ProcessFirstPass())
			{
				HideBiggieCombatMenuTextContainer();
				HideActionInfo();
				if (TransitionToBiggieAttack(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartBiggieAttackTurn);
					ShowBiggieAttackContainer();
					_nodeBiggieAttackContainer.IsActive = true;
					EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatAsk
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatCharm)
		{
			if (!ProcessFirstPass())
			{
				HideBiggieCombatMenuTextContainer();
				HideActionInfo();
				if (TransitionToBiggieAttack(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartBiggieAttackTurn);
					ShowBiggieAttackContainer();
					_nodeBiggieAttackContainer.IsActive = true;
					EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}

		if ((currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatAsk
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatCharm
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightScratch
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightBite
			) && Input.IsActionJustPressed(_INTERACT_INPUT)
		)
		{
			EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
		}
	}

	[Signal]
	public delegate void StartEnemyAttackTurnEventHandler();
	[Signal]
	public delegate void StartBiggieTextTurnEventHandler();
	[Signal]
	public delegate void StartBiggieAttackTurnEventHandler();

	public bool TranslateHudEnemyAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack);
		}

		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_SLOW;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack);
		return false;
	}

	public bool TranslateHudBiggieAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack);
		}

		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack);
		return false;
	}

	public bool TranslateHudBiggieCombatMenu(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieCombatMenu);
		}

		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieCombatMenu))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieCombatMenu);
		return false;
	}

	public bool TransformToEnemyAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeSubjectPanel, _nodeEnemyAttackPanel);
		}

		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeEnemyAttackPanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeEnemyAttackPanel))
		{
			_nodeEnemyAttackContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeEnemyAttackPanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeEnemyAttackPanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeEnemyAttackPanel);
		}
		else if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeEnemyAttackPanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeEnemyAttackPanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeEnemyAttackPanel);
		}
		else
		{
			////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeEnemyAttackPanel)}");
		}
		return false;
	}

	public bool TransformToBiggieAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeSubjectPanel, _nodeBiggieAttackPanel);
		}

		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeBiggieAttackPanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeBiggieAttackPanel))
		{
			_nodeBiggieAttackContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeBiggieAttackPanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBiggieAttackPanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeBiggieAttackPanel);
		}
		else if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeBiggieAttackPanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeBiggieAttackPanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeBiggieAttackPanel);
		}
		else
		{
			////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBiggieAttackPanel)}");
		}
		return false;
	}

	public bool TransformToBiggieCombatMenu(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeSubjectPanel, _nodeBasePagePanel);
		}

		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeBasePagePanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeBasePagePanel))
		{
			_nodeBiggieCombatMenuTextContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeBasePagePanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBasePagePanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeBasePagePanel);
		}
		else if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeBasePagePanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeBasePagePanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeBasePagePanel);
		}
		else
		{
			////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBasePagePanel)}");
		}
		return false;
	}

	private HBoxContainer CreateHudContainerTarget(HBoxContainer subject, float diffX, float diffY)
	{
		var result = _nodeHudContainerSubject.Duplicate() as HBoxContainer;
		result.Position = new Vector2(
			result.Position.X + diffX, result.Position.Y + diffY
		);
		return result;
	}

	public void ShowEnemyAttackContainer()
	{
		_nodeEnemyAttackContainer.Show();
	}

	public void HideEnemyAttackContainer()
	{
		_nodeEnemyAttackContainer.Hide();
	}

	public void ShowBiggieAttackContainer()
	{
		_nodeBiggieAttackContainer.Show();
	}

	public void HideBiggieAttackContainer()
	{
		_nodeBiggieAttackContainer.Hide();
	}

	public void ShowBiggieCombatMenuTextContainer()
	{
		_nodeBiggieCombatMenuTextContainer.Show();
		_nodeBiggieCombatMenu.StartTurn();
	}

	public void HideBiggieCombatMenuTextContainer()
	{
		_nodeBiggieCombatMenuTextContainer.Hide();
	}

	public void ShowActionInfo()
	{
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideActionInfo()
	{
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 0);
	}

	public void ShowSubjectPanel()
	{
		_nodeSubjectPanel.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideSubjectPanel()
	{
		_nodeSubjectPanel.Modulate = new Color(1, 1, 1, 0);
	}

	public void HandleFightSelection(int selectedIndex)
	{
		if (selectedIndex != (int)Enumerations.Combat.FightPagePanelOptions.Back)
		{
			EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		}
	}

	public void HandleChatSelection(int selectedIndex)
	{
		if (selectedIndex != (int)Enumerations.Combat.ChatPagePanelOptions.Back)
		{
			EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		}
	}

	private bool TransitionToBiggieAttack(bool skip)
	{
		bool finished = false;
		finished = TranslateHudBiggieAttack(skip);
		finished = TransformToBiggieAttack(skip) && finished;
		return finished;
	}

	private bool TransitionToEnemyAttack(bool skip)
	{
		bool finished = false;
		finished = TranslateHudEnemyAttack(skip);
		finished = TransformToEnemyAttack(skip) && finished;
		return finished;
	}

	private bool TransitionToBiggieCombatMenu(bool skip)
	{
		bool finished = false;
		finished = TranslateHudBiggieCombatMenu(skip);
		finished = TransformToBiggieCombatMenu(skip) && finished;
		return finished;
	}

	private void HandleEndBiggieAttackTurn(double damagePercentage)
	{
		switch (LastCombatOptionUsed)
		{
			case Enumerations.Combat.CombatOptions.Scratch:
				DealPhysicalDamage(1 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.Bite:
				DealPhysicalDamage(2 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.Ask:
				DealEmotionalDamage(1 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.Charm:
				DealEmotionalDamage(2 * damagePercentage);
				break;
			default:
				GD.Print("CombatWrapper HandleEndBiggieAttackTurn LastCombatOptionUsed did not map.");
				break;
		}
	}

	private void HandleEndBiggieCombatMenuTurn(int combatOption)
	{
		LastCombatOptionUsed = (Enumerations.Combat.CombatOptions)combatOption;
	}

	[Signal]
	public delegate void ProjectPhysicalDamageEventHandler();
	public void DealPhysicalDamage(double damage)
	{
		;
		_globalCombatSingleton.BiggiePhysicalAttackProxy.DealDamage(damage);
		GD.Print($"enemy health: {_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetHealthPercentage()}");
		EmitSignal(SignalName.ProjectPhysicalDamage);
	}

	public void DealEmotionalDamage(double damage)
	{
		_globalCombatSingleton.BiggieEmotionalAttackProxy.DealDamage(damage);
	}

	private static readonly StringName _COMBAT_EVENT = new StringName("CombatEvent");
	private void EmitCombatEvent(Enumerations.Combat.StateMachine.Events eventId)
	{
		_globalCombatSingleton.CombatStateMachineService.EmitSignal(_COMBAT_EVENT, (int)eventId);
	}

	// private void ApplyCombatStateMachineEvents()
	// {
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatAsk)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToBiggieChatCharm)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightBite)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToBiggieFightScratch)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToCombatMenu)
	// 		.AddEventHandler(TransitionToBiggieCombatMenuEvent);
	// }

	// private bool TransitionToEnemyAttackEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideBiggieAttackContainer();
	// 		_nodeBiggieAttackContainer.IsActive = false;
	// 		if (TransitionToEnemyAttack(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartEnemyAttackTurn);
	// 			ShowEnemyAttackContainer();
	// 			return true;
	// 		}
	// 	}
	// 	return false;
	// }

	// private bool TransitionToBiggieCombatMenuEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideEnemyAttackContainer();
	// 		if (TransitionToBiggieCombatMenu(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartBiggieTextTurn);
	// 			ShowBiggieCombatMenuTextContainer();
	// 			return true;
	// 		}
	// 	}
	// 	return false;
	// }

	// private void TransitionToBiggieFightEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideBiggieCombatMenuTextContainer();
	// 		HideActionInfo();
	// 		if (TransitionToBiggieAttack(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartBiggieAttackTurn);
	// 			ShowBiggieAttackContainer();
	// 			_nodeBiggieAttackContainer.IsActive = true;
	// 		}
	// 	}
	// }

	// private void TransitionToBiggieChatEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideBiggieCombatMenuTextContainer();
	// 		HideActionInfo();
	// 		if (TransitionToBiggieAttack(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartBiggieAttackTurn);
	// 			ShowBiggieAttackContainer();
	// 			_nodeBiggieAttackContainer.IsActive = true;
	// 		}
	// 	}
	// }
}
